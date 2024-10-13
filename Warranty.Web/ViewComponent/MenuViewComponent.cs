using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Microsoft.AspNetCore.Mvc;

namespace Warranty.Web.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        ICommonProvider _commonProvider;
        ISessionManager _sessionManager;
        public MenuViewComponent(ICommonProvider commonProvider, ISessionManager sessionManager)
        {
            _commonProvider = commonProvider;
            _sessionManager = sessionManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
                                                                                          {
            SessionProviderModel sessionProviderModel = new SessionProviderModel
            {
                UserId = _sessionManager.UserId,
                RoleId = _sessionManager.RoleId,              
            };
            return View(_commonProvider.GetMenuList(sessionProviderModel));
        }
    }
}
