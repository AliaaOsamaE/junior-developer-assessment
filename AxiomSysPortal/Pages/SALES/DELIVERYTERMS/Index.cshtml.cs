using AxiomSysPortal.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace AxiomSysPortal.Pages.SALES.DELIVERYTERMS;

public class IndexModel : BasePageModel
{
    public IndexModel(IConfiguration configuration, IStringLocalizer<SharedResource> localizer)
        : base(configuration, localizer)
    {
    }

    public class DeliveryTerm
    {
        public int Code { get; set; }
        public string SName { get; set; } = "";
        public string BName { get; set; } = "";
        public int? Days { get; set; }
        public bool Active { get; set; } = true;
    }

    public List<DeliveryTerm> Terms { get; set; } = new List<DeliveryTerm>();

    [BindProperty]
    public DeliveryTerm Input { get; set; } = new DeliveryTerm();

    private string ControllerUrl
    {
        get { return ApiUri + "Sales/SaDeliveryTerm/"; }
    }

    public async Task OnGet()
    {
        try
        {
            var response = await Helper.GetRequest(ControllerUrl + "GetSaDeliveryTerms", Token);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Terms = JsonConvert.DeserializeObject<List<DeliveryTerm>>(body) ?? new List<DeliveryTerm>();
            }
            else
            {
                TempData["ErrorMessage"] = GetErrorMessage(response, null);
            }
        }
        catch (HttpRequestException)
        {
            TempData["ErrorMessage"] = _localizer["UnexpectedError"].Value;
        }
    }

    public async Task<IActionResult> OnGetDeliveryTerm(int code)
    {
        var response = await Helper.GetRequest(ControllerUrl + "GetSaDeliveryTermByCode/" + code, Token);

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var body = await response.Content.ReadAsStringAsync();
        var term = JsonConvert.DeserializeObject<DeliveryTerm>(body);

        if (term == null)
        {
            return NotFound();
        }

        return new JsonResult(new
        {
            code = term.Code,
            sName = term.SName,
            bName = term.BName,
            days = term.Days,
            active = term.Active
        });
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            HttpResponseMessage? response = null;

            if (Request.Form.ContainsKey("AddDetail"))
            {
                response = await Helper.PostRequest(ControllerUrl + "AddSaDeliveryTerm", JsonConvert.SerializeObject(Input), Token);
            }
            else if (Request.Form.ContainsKey("EditDetail"))
            {
                response = await Helper.PutRequest(ControllerUrl + "EditSaDeliveryTerm", JsonConvert.SerializeObject(Input), Token);
            }
            else if (Request.Form.ContainsKey("DeleteDetail"))
            {
                var code = Request.Form["DeleteCode"].ToString();
                response = await Helper.DeleteRequest(ControllerUrl + "DeleteSaDeliveryTerm/" + code, Token);
            }

            if (response != null)
            {
                await SetResponseMessage(response);
            }
        }
        catch (HttpRequestException)
        {
            TempData["ErrorMessage"] = _localizer["UnexpectedError"].Value;
        }

        return RedirectToPage(new { culture = CultureHelper.CurrentCulture });
    }
}
