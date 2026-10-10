using AxiomSysBL.IServices.ISalesService;
using AxiomSysBL.Models.Sales;
using AxiomSysSL.Filters;
using AxiomSysSL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiomSysSL.Controllers.Sales;

[Authorize]
[Route("api/Sales/[controller]")]
[ApiController]
[AuthorizeCompany]
public class SaDeliveryTermController : Controller
{
    private readonly ISaDeliveryTermService _saDeliveryTermService;
    private readonly ICurrentUserService _currentUserService;

    public SaDeliveryTermController(ISaDeliveryTermService saDeliveryTermService, ICurrentUserService currentUserService)
    {
        _saDeliveryTermService = saDeliveryTermService;
        _currentUserService = currentUserService;
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        userId = _currentUserService.UserId ?? 0;
        return userId > 0;
    }

    [HttpGet("GetSaDeliveryTerms")]
    public IActionResult GetSaDeliveryTerms()
    {
        return Ok(_saDeliveryTermService.GetSaDeliveryTerms());
    }

    [HttpGet("GetSaDeliveryTermByCode/{code}")]
    public IActionResult GetSaDeliveryTermByCode(int code)
    {
        var model = _saDeliveryTermService.GetSaDeliveryTermByCode(code);
        if (model == null)
        {
            return NotFound();
        }
        return Ok(model);
    }

    [HttpPost("AddSaDeliveryTerm")]
    public IActionResult AddSaDeliveryTerm(SaDeliveryTermModel model)
    {
        if (!TryGetCurrentUserId(out var userId)) return Unauthorized();
        return Ok(_saDeliveryTermService.AddSaDeliveryTerm(model, userId));
    }

    [HttpPut("EditSaDeliveryTerm")]
    public IActionResult EditSaDeliveryTerm(SaDeliveryTermModel model)
    {
        if (!TryGetCurrentUserId(out var userId)) return Unauthorized();
        return Ok(_saDeliveryTermService.EditSaDeliveryTerm(model, userId));
    }

    [HttpDelete("DeleteSaDeliveryTerm/{code}")]
    public IActionResult DeleteSaDeliveryTerm(int code)
    {
        if (!TryGetCurrentUserId(out _)) return Unauthorized();
        return Ok(_saDeliveryTermService.DeleteSaDeliveryTerm(code));
    }
}
