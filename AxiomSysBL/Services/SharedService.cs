using AxiomSysBL.IServices;
using AxiomSysBL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace AxiomSysBL.Services;

public class SharedService : ISharedService
{
    private readonly ILogger<SharedService> _logger;

    public SharedService(ILogger<SharedService> logger)
    {
        _logger = logger;
    }

    public ResponseModel HandleException(Exception ex)
    {
        _logger.LogError(ex, "Error in business layer");

        if (ex is DbUpdateException && ex.InnerException is OracleException oracleEx && oracleEx.Number == 1)
        {
            return new ResponseModel
            {
                status = false,
                messageAr = "هذا الكود موجود بالفعل.",
                messageEn = "A record with this code already exists."
            };
        }

        return new ResponseModel
        {
            status = false,
            messageAr = "حدث خطأ غير متوقع، حاول مرة أخرى.",
            messageEn = "An unexpected error occurred. Please try again."
        };
    }
}
