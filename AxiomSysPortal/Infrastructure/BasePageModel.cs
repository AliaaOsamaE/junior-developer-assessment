using System.Net;
using AxiomSysPortal.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace AxiomSysPortal.Infrastructure;

public class BasePageModel : PageModel
{
    protected readonly IConfiguration _configuration;
    protected readonly IStringLocalizer<SharedResource> _localizer;

    public BasePageModel(IConfiguration configuration, IStringLocalizer<SharedResource> localizer)
    {
        _configuration = configuration;
        _localizer = localizer;
    }

    protected string ApiUri
    {
        get { return _configuration.GetValue<string>("Uri") ?? ""; }
    }

    protected string? Token
    {
        get { return Helper.GetClaimValue(User, "token"); }
    }

    protected async Task<ResponseModel?> ReadResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var body = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<ResponseModel>(body);
    }

    protected async Task SetResponseMessage(HttpResponseMessage response)
    {
        var result = await ReadResponse(response);

        if (result != null && result.status)
        {
            TempData["SuccessMessage"] = GetMessage(result);
        }
        else
        {
            TempData["ErrorMessage"] = GetErrorMessage(response, result);
        }
    }

    protected string? GetMessage(ResponseModel result)
    {
        if (CultureHelper.IsArabic)
        {
            return result.messageAr;
        }
        return result.messageEn;
    }

    protected string GetErrorMessage(HttpResponseMessage response, ResponseModel? result)
    {
        if (result != null && !string.IsNullOrEmpty(GetMessage(result)))
        {
            return GetMessage(result)!;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return _localizer["SessionExpired"];
        }

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            return _localizer["AccessDenied"];
        }

        return _localizer["UnexpectedError"];
    }
}
