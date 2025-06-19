namespace ComponentSelector.Application.Contracts;

public class SearchBuildDto
{
    public required string CPUTitle { get; set; }
    public required string MotherboardTitle { get; set; }
    public required string RAMTitle { get; set; }
    public required string StorageTitle { get; set; }
    public required string GPUTitle { get; set; }
    public required string PSUTitle { get; set; }
    public required string CaseTitle { get; set; }

}
