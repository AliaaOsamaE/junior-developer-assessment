using System.ComponentModel.DataAnnotations;

namespace AxiomSysBL.Models.Sales;

public class SaDeliveryTermModel
{
    [Range(1, 999)]
    public int Code { get; set; }

    [Required]
    [MaxLength(60)]
    public string SName { get; set; } = "";

    [Required]
    [MaxLength(60)]
    public string BName { get; set; } = "";

    [Range(0, 9999)]
    public int? Days { get; set; }

    public bool Active { get; set; } = true;
}
