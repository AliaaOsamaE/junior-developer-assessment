namespace AxiomSysSL.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get { return GetClaimAsInt("Id"); }
    }

    public int? CompanyCode
    {
        get { return GetClaimAsInt("CompanyCode"); }
    }

    private int? GetClaimAsInt(string claimType)
    {
        var value = _httpContextAccessor.HttpContext?.User.FindFirst(claimType)?.Value;

        if (int.TryParse(value, out int number))
        {
            return number;
        }

        return null;
    }
}
