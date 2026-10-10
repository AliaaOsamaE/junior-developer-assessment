using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AxiomSysPortal.Infrastructure;

public class CultureRouteConvention : IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        var oldSelectors = model.Selectors.ToList();

        foreach (var selector in oldSelectors)
        {
            var newSelector = new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Order = -1,
                    Template = AttributeRouteModel.CombineTemplates("{culture:regex(^(en|ar)$)}", selector.AttributeRouteModel?.Template)
                }
            };

            model.Selectors.Add(newSelector);
        }
    }
}
