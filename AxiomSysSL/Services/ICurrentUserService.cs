namespace AxiomSysSL.Services;

public interface ICurrentUserService
{
    int? UserId { get; }
    int? CompanyCode { get; }
}
