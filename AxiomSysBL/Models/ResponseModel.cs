namespace AxiomSysBL.Models;

public class ResponseModel
{
    public bool status { get; set; }
    public object? data { get; set; }
    public string? messageAr { get; set; }
    public string? messageEn { get; set; }
}
