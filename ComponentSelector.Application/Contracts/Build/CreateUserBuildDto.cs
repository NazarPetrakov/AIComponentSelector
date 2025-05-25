namespace ComponentSelector.Application.Contracts.Build;

public class CreateUserBuildDto
{
    public int? CPUId { get; set; }

    public int? MotherboardId { get; set; }

    public int? RAMId { get; set; }

    public int? StorageId { get; set; }

    public int? GPUId { get; set; }

    public int? PSUId { get; set; }

    public int? CaseId { get; set; }
}
