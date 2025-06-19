namespace ComponentSelector.Domain.Entities;

public class Build : BaseEntity
{
    public int? CPUId { get; set; }
    public Component? CPU { get; set; }

    public int? MotherboardId { get; set; }
    public Component? Motherboard { get; set; }

    public int? RAMId { get; set; }
    public Component? RAM { get; set; }

    public int? StorageId { get; set; }
    public Component? Storage { get; set; }

    public int? GPUId { get; set; }
    public Component? GPU { get; set; }

    public int? PSUId { get; set; }
    public Component? PSU { get; set; }

    public int? CaseId { get; set; }
    public Component? Case { get; set; }

    public int UserId { get; set; }
    public required AppUser User { get; set; }

    public double TotalPrice { get; set; }

}
