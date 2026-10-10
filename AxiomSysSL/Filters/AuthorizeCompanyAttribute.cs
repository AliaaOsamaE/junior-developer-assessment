using AxiomSysSL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AxiomSysSL.Filters;

public class AuthorizeCompanyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var currentUser = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

        if (currentUser.CompanyCode == null || currentUser.CompanyCode <= 0)
        {
            context.Result = new ForbidResult();
        }
    }
}
