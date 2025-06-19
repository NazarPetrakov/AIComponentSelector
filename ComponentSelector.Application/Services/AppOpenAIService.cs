using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.IServices;
using Newtonsoft.Json;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using ComponentSelector.Domain.Exceptions;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;
using ComponentSelector.Application.Contracts.OpenAi;


namespace ComponentSelector.Application.Services;

public class AppOpenAIService : IAppOpenAIService
{
    private readonly IOpenAIService _client;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AppOpenAIService(IOpenAIService openAIService, IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
        _client = openAIService;
        // _openAIService.SetDefaultModelId(Models.Gpt_4o);
        _client.SetDefaultModelId(Models.Gpt_4_1);
    }
    public async Task ClearChatHistory(string userId)
    {
        string? threadId = await _userService.GetChatThreadIdAsync(userId);

        if (threadId is null)
        {
            return;
        }

        DeletionStatusResponse? deletionResponse = await _client.Beta.Threads.ThreadDelete(threadId);

        if (deletionResponse is not null)
        {
            if (!deletionResponse.Successful)
            {
                throw new OpenAiException($"Open AI thread deleting failed. Error: {deletionResponse.Error?.Message}");
            }
        }

        await _userService.DeleteChatThreadIdAsync(userId);
    }
    public async Task<List<BotMessageDto>> GetUserChatMessages(string userId)
    {
        string? threadId = await _userService.GetChatThreadIdAsync(userId);

        if (threadId is null)
        {
            return [];
        }

        var messages = await _client.Beta.Messages.ListMessages(threadId);

        var messageDtos = messages?.Data?.Select(x => new BotMessageDto()
        {
            Role = x.Role,
            Message = x.Content?.FirstOrDefault()?.Text?.Value ?? "No message"
        });

        return messageDtos is null ? [] : messageDtos.ToList();
    }
    public async Task<string> AskBotAsync(string userId, string userMessage)
    {
        string assistantId = _configuration.GetSection("OpenAi").GetValue<string>("assistantId")
            ?? throw new InvalidConfigurationException("OpenAi assistant id is missing in configuration."); ;
        AssistantResponse? assistant = await _client.Beta.Assistants.AssistantRetrieve(assistantId)
            ?? throw new ItemNotFoundException($"Assistant with id {assistantId} not found.");

        string? threadId = await _userService.GetChatThreadIdAsync(userId);

        if (threadId is null)
        {
            ThreadResponse? thread = await _client.Beta.Threads.ThreadCreate();
            if (!thread.Successful)
            {
                throw new OpenAiException($"Open AI thread creating failed. Message: {thread?.Error?.Message}.");
            }
            threadId = thread.Id;
            await _userService.UpdateChatThreadIdAsync(userId, threadId);
        }

        await _client.Beta.Messages.CreateMessage(threadId, new MessageCreateRequest()
        {
            Content = new MessageContentOneOfType(userMessage)
        });

        RunResponse? createdRun = await _client.Beta.Runs.RunCreate(threadId, new RunCreateRequest()
        {
            AssistantId = assistantId
        });

        if (!createdRun.Successful)
        {
            throw new OpenAiException($"Open AI running failed. Message: {createdRun?.Error?.Message}.");
        }

        RunResponse retrievedRun;
        do
        {
            await Task.Delay(1000);
            retrievedRun = await _client.Beta.Runs.RunRetrieve(threadId, createdRun.Id);
        }
        while (retrievedRun.Status != "completed" && retrievedRun.Status != "failed");

        if (retrievedRun.Status == "failed")
            throw new OpenAiException("OpenAI assistant run failed.");

        var messages = await _client.Beta.Messages.ListMessages(threadId);

        var lastMessage = messages?.Data?.FirstOrDefault(m => m.Role == "assistant");

        return lastMessage?.Content?.FirstOrDefault()?.Text?.Value ?? "No response.";
    }
    public async Task<ChatBuildResponse> GetChatBuildAsync(ChatBuildRequest request)
    {
        string systemMessage = @"
            You are a PC building assistant. This JSON object is the ONLY output you must return. Nothing before or after.  
            You must return ONLY a valid JSON object matching the exact structure below. Do not include any additional text, comments, explanations, markdown, or formatting.  
            Do NOT wrap the output in ```json or any similar code block syntax.  
            The JSON must be fully parsable and syntactically valid. Use double quotes for all strings. Do not omit commas or use trailing commas.

            ### GPU Brand Priority Rules:
            1. Preferred GPU brands:
            - For NVIDIA: ASUS, MSI, Gigabyte, ZOTAC, PNY
            - For AMD: ASUS, MSI, Gigabyte, Sapphire, PowerColor
            
            2. GPU title format examples:
            - ""ASUS ROG Strix GeForce RTX 4080 OC Edition""
            - ""Gigabyte AORUS Radeon RX 7900 XT""
            - ""MSI GeForce RTX 4070 Ti Gaming X Trio""

            3. Chip field must always include:
            - For NVIDIA: Full ""GeForce"" naming (e.g. ""GeForce RTX 4080"")
            - For AMD: Full ""Radeon"" naming (e.g. ""Radeon RX 7900 XT"")

            Important: The ""characteristics"" field for each component must be a list (array) of objects, where each object has two properties:
            - ""attributeName"" (string)
            - ""attributeValue"" (string, number, or appropriate type)

            The characteristics list for each component **must include compatibility-relevant specifications** as follows:

            CPU:
            - brand, model, socket, cores, threads, base_clock, boost_clock, TDP, supported_memory_type

            Motherboard:
            - brand, model, chipset, socket, form_factor, memory_type_supported, max_memory, memory_slots, pcie_version, storage_interfaces

            RAM:
            - brand, model, memory_type, capacity, speed, latency, modules, voltage

            Storage:
            - brand, model, type (e.g., NVMe SSD, SATA HDD), capacity, interface, read_speed, write_speed

            GPU:
            - brand, chip, vram, interface, pcie_version, length_mm, power_draw, power_connectors

            PSU:
            - brand, model, wattage, efficiency_certification, modular, connectors_CPU, connectors_GPU

            Case:
            - brand, model, form_factor_supported, gpu_max_length_mm, cooler_max_height_mm, drive_bays, fan_mounts

            ### Strict JSON Formatting Rules:
            1. For multi-value fields like ""drive_bays"", ""fan_mounts"" or ""storage_interfaces"":
            Use a SINGLE string with values separated by commas/slashes: 
            ""drive_bays"": ""2 x 3.5, 1 x 2.5""

            Use the following overall structure:
            {
            ""build"": {
                ""CPU"": { 
                ""title"": ""..."", 
                ""price"": ..., 
                ""characteristics"": [ { ""attributeName"": ""..."", ""attributeValue"": ""..."" }, ... ]
                },
                ""Motherboard"": { 
                ""title"": ""..."", 
                ""price"": ..., 
                ""characteristics"": [ { ""attributeName"": ""..."", ""attributeValue"": ""..."" }, ... ]
                },
                ""RAM"": { 
                ""title"": ""..."", 
                ""price"": ..., 
                ""characteristics"": [ { ""attributeName"": ""..."", ""attributeValue"": ""..."" }, ... ]
                },
                ""Storage"": { 
                ""title"": ""..."", 
                ""price"": ..., 
                ""characteristics"": [ { ""attributeName"": ""..."", ""attributeValue"": ""..."" }, ... ]
                },
                ""GPU"": { 
                ""title"": ""..."", 
                ""price"": ..., 
                ""characteristics"": [ { ""attributeName"": ""..."", ""attributeValue"": ""..."" }, ... ]
                },
                ""PSU"": { 
                ""title"": ""..."", 
                ""price"": ..., 
                ""characteristics"": [ { ""attributeName"": ""..."", ""attributeValue"": ""..."" }, ... ]
                },
                ""Case"": { 
                ""title"": ""..."", 
                ""price"": ..., 
                ""characteristics"": [ { ""attributeName"": ""..."", ""attributeValue"": ""..."" }, ... ]
                }
            },
            ""totalPrice"": ...,
            ""compatibility"": {
                ""CPU"": ""..."",
                ""Motherboard"": ""..."",
                ""RAM"": ""..."",
                ""GPU"": ""..."",
                ""PSU"": ""..."",
                ""Storage"": ""..."",
                ""Case"": ""...""
            }
            }

            Instructions:
            - Stay within the user's budget.
            - Use accurate, full official names.
            - Ensure components are compatible.

            Compatibility explanations should be in " + request.Lang + @"
            All compatibility notes must be written in this language.
            ";

        ChatCompletionCreateResponse? completionResult = await _client.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(systemMessage),
                ChatMessage.FromUser($"Build a PC for ${request.Price}. " +
                    $"The main purpose of this PC is: {request.Purpose}. Extra info: {request.Description}"),
            },
        });

        if (!completionResult.Successful)
        {
            throw new OpenAiException("Something went wrong during open ai chat request.");
        }

        string responseContent = completionResult.Choices.First().Message.Content ?? "No content";

        string jsonResponse = responseContent.Trim()
            .TrimStart("```json".ToCharArray()).TrimEnd("```".ToCharArray()).Trim();

        try
        {
            var result = JsonConvert.DeserializeObject<ChatBuildResponse>(jsonResponse);

            if (result == null)
            {
                throw new JsonException("Deserialized object is null.");
            }

            return result;
        }
        catch (JsonException ex)
        {
            throw new JsonException("Failed to parse AI response as JSON.", ex);
        }
    }
}
