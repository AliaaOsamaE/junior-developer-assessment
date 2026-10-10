using System.Security.Claims;
using AxiomSysPortal.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace AxiomSysPortal.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LoginModel(IConfiguration configuration, IStringLocalizer<SharedResource> localizer)
    {
        _configuration = configuration;
        _localizer = localizer;
    }

    [BindProperty]
    public int UserId { get; set; } = 1;

    [BindProperty]
    public int CompanyCode { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public string? ErrorMessage { get; set; }

    public class LoginResponse
    {
        public string token { get; set; } = "";
        public DateTime expiresUtc { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (UserId <= 0 || CompanyCode <= 0)
        {
            ErrorMessage = _localizer["Required"];
            return Page();
        }

        var url = _configuration.GetValue<string>("Uri") + "Auth/DevLogin";
        var json = JsonConvert.SerializeObject(new { userId = UserId, companyCode = CompanyCode });

        HttpResponseMessage response;
        try
        {
            response = await Helper.PostRequest(url, json, null);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = _localizer["LoginFailed"];
            return Page();
        }

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = _localizer["LoginFailed"];
            return Page();
        }

        var body = await response.Content.ReadAsStringAsync();
        var login = JsonConvert.DeserializeObject<LoginResponse>(body);

        if (login == null || string.IsNullOrEmpty(login.token))
        {
            ErrorMessage = _localizer["LoginFailed"];
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, UserId.ToString()),
            new Claim("Id", UserId.ToString()),
            new Claim("CompanyCode", CompanyCode.ToString()),
            new Claim("token", login.token)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var properties = new AuthenticationProperties { ExpiresUtc = login.expiresUtc };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

        if (Url.IsLocalUrl(ReturnUrl))
        {
            return LocalRedirect(ReturnUrl);
        }

        return LocalRedirect("/" + CultureHelper.CurrentCulture);
    }
}
