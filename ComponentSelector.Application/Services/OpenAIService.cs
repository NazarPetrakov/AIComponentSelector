using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Exceptions;
using Newtonsoft.Json;
using OpenAI.Chat;

namespace ComponentSelector.Application.Services;

public class OpenAIService : IOpenAIService
{
    private readonly string _apiKey;
    private readonly ChatClient _client;

    public OpenAIService()
    {
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
           ?? throw new ItemNotFoundException("API key not found in environment variables");

        _client = new ChatClient(model: "gpt-4o-mini", apiKey: _apiKey);
        // _client = new ChatClient(model: "gpt-4o", apiKey: _apiKey);
    }
    public async Task<ChatBuildResponse> GetChatBuildAsync(ChatBuildRequest request)
    {
        string systemMessage = @"
            You are a PC building assistant. Your task is to generate a PC build recommendation strictly in **valid JSON format** that fits the user's **budget** and **purpose**.
            Output ONLY a valid JSON object in the structure below. Do NOT include explanations or markdown.

            Always use full official names for all components, including in the ""chip"" field for the GPU.
            For example:
            For NVIDIA, the ""chip"" must start with ""GeForce"" (e.g., ""GeForce RTX 4080"").
            For AMD, the ""chip"" must start with ""Radeon"" (e.g., ""Radeon RX 7900 XT"").
            Never omit these brand identifiers from the ""chip"" field.

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
        ChatCompletion completion = await _client.CompleteChatAsync(new List<ChatMessage> {
            ChatMessage.CreateSystemMessage(systemMessage),
            ChatMessage.CreateUserMessage($"Build a PC for ${request.Price}. " +
                "The main purpose of this PC is: {request.Purpose}. {request.Description} ")
        },);
        string responseContent = completion.Content[0].Text.ToString();

        string jsonResponse = responseContent.Trim()
            .TrimStart("```json".ToCharArray()).TrimEnd("```".ToCharArray()).Trim();
        var result = JsonConvert.DeserializeObject<ChatBuildResponse>(jsonResponse) ??
            new ChatBuildResponse();

        return result;
    }
}
