namespace AxiomSysDL.Models.SupplyChain;

public class SaDeliveryTerm
{
    public int Code { get; set; }
    public string SName { get; set; } = "";
    public string BName { get; set; } = "";
    public int? Days { get; set; }
    public int? ActiveFlag { get; set; }
    public int EntryUser { get; set; }
    public DateTime EntryDate { get; set; }
    public int? ChangeUser { get; set; }
    public DateTime? ChangeDate { get; set; }
}
