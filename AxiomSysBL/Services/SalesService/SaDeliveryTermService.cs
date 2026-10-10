using System.Text.RegularExpressions;
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
        var terms = _uowSC.GetRepository<SaDeliveryTerm>().GetAll()
            .OrderBy(x => x.Code)
            .ToList();

        return terms
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
        var nameError = ValidateNames(model);
        if (nameError != null)
        {
            return nameError;
        }

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
        var nameError = ValidateNames(model);
        if (nameError != null)
        {
            return nameError;
        }

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

    private ResponseModel? ValidateNames(SaDeliveryTermModel model)
    {
        if (!Regex.IsMatch(model.SName, @"^[\u0600-\u06FF0-9\s\-\.\(\)/]+$"))
        {
            return new ResponseModel
            {
                status = false,
                messageAr = "الاسم العربي يجب أن يكون باللغة العربية فقط.",
                messageEn = "The Arabic name must be in Arabic only."
            };
        }

        if (!Regex.IsMatch(model.BName, @"^[A-Za-z0-9\s\-\.\(\)/]+$"))
        {
            return new ResponseModel
            {
                status = false,
                messageAr = "الاسم الإنجليزي يجب أن يكون باللغة الإنجليزية فقط.",
                messageEn = "The English name must be in English only."
            };
        }

        return null;
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
