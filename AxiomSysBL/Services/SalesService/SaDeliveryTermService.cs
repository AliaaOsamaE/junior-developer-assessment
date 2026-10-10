using AxiomSysBL.IServices;
using AxiomSysBL.IServices.ISalesService;
using AxiomSysBL.Models;
using AxiomSysBL.Models.Sales;
using AxiomSysDL.Contexts;
using AxiomSysDL.Models.SupplyChain;
using AxiomSysDL.Repositories;

namespace AxiomSysBL.Services.SalesService;

public class SaDeliveryTermService : ISaDeliveryTermService
{
    private readonly IUnitOfWork<SupplyChainContext> _uowSC;
    private readonly ISharedService _sharedService;

    public SaDeliveryTermService(IUnitOfWork<SupplyChainContext> uowSC, ISharedService sharedService)
    {
        _uowSC = uowSC;
        _sharedService = sharedService;
    }

    public List<SaDeliveryTermModel> GetSaDeliveryTerms()
    {
        return _uowSC.GetRepository<SaDeliveryTerm>().GetAll()
            .OrderBy(x => x.Code)
            .Select(x => new SaDeliveryTermModel
            {
                Code = x.Code,
                SName = x.SName,
                BName = x.BName,
                Days = x.Days,
                Active = x.ActiveFlag == 1
            })
            .ToList();
    }

    public SaDeliveryTermModel? GetSaDeliveryTermByCode(int code)
    {
        var entity = _uowSC.GetRepository<SaDeliveryTerm>().GetById(code);
        if (entity == null)
        {
            return null;
        }

        return new SaDeliveryTermModel
        {
            Code = entity.Code,
            SName = entity.SName,
            BName = entity.BName,
            Days = entity.Days,
            Active = entity.ActiveFlag == 1
        };
    }

    public ResponseModel AddSaDeliveryTerm(SaDeliveryTermModel model, int userId)
    {
        try
        {
            var repository = _uowSC.GetRepository<SaDeliveryTerm>();

            if (repository.GetById(model.Code) != null)
            {
                return new ResponseModel
                {
                    status = false,
                    messageAr = "هذا الكود موجود بالفعل.",
                    messageEn = "A record with this code already exists."
                };
            }

            var entity = new SaDeliveryTerm
            {
                Code = model.Code,
                SName = model.SName,
                BName = model.BName,
                Days = model.Days,
                ActiveFlag = model.Active ? 1 : 0,
                EntryUser = userId,
                EntryDate = DateTime.Now
            };

            repository.Add(entity);
            _uowSC.Commit();

            return new ResponseModel
            {
                status = true,
                data = entity.Code,
                messageAr = "تمت الإضافة بنجاح.",
                messageEn = "Added successfully."
            };
        }
        catch (Exception ex)
        {
            return _sharedService.HandleException(ex);
        }
    }

    public ResponseModel EditSaDeliveryTerm(SaDeliveryTermModel model, int userId)
    {
        try
        {
            var repository = _uowSC.GetRepository<SaDeliveryTerm>();
            var entity = repository.GetById(model.Code);

            if (entity == null)
            {
                return NotFoundResponse();
            }

            entity.SName = model.SName;
            entity.BName = model.BName;
            entity.Days = model.Days;
            entity.ActiveFlag = model.Active ? 1 : 0;
            entity.ChangeUser = userId;
            entity.ChangeDate = DateTime.Now;

            repository.Update(entity);
            _uowSC.Commit();

            return new ResponseModel
            {
                status = true,
                data = entity.Code,
                messageAr = "تم التعديل بنجاح.",
                messageEn = "Updated successfully."
            };
        }
        catch (Exception ex)
        {
            return _sharedService.HandleException(ex);
        }
    }

    public ResponseModel DeleteSaDeliveryTerm(int code)
    {
        try
        {
            var repository = _uowSC.GetRepository<SaDeliveryTerm>();
            var entity = repository.GetById(code);

            if (entity == null)
            {
                return NotFoundResponse();
            }

            repository.Remove(entity);
            _uowSC.Commit();

            return new ResponseModel
            {
                status = true,
                data = code,
                messageAr = "تم الحذف بنجاح.",
                messageEn = "Deleted successfully."
            };
        }
        catch (Exception ex)
        {
            return _sharedService.HandleException(ex);
        }
    }

    private ResponseModel NotFoundResponse()
    {
        return new ResponseModel
        {
            status = false,
            messageAr = "السجل غير موجود.",
            messageEn = "Record not found."
        };
    }
}
