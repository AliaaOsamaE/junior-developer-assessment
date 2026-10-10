using AxiomSysBL.Models;
using AxiomSysBL.Models.Sales;

namespace AxiomSysBL.IServices.ISalesService;

public interface ISaDeliveryTermService
{
    List<SaDeliveryTermModel> GetSaDeliveryTerms();
    SaDeliveryTermModel? GetSaDeliveryTermByCode(int code);
    ResponseModel AddSaDeliveryTerm(SaDeliveryTermModel model, int userId);
    ResponseModel EditSaDeliveryTerm(SaDeliveryTermModel model, int userId);
    ResponseModel DeleteSaDeliveryTerm(int code);
}
