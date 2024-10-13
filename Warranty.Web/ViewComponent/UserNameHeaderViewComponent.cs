using Warranty.Common.BussinessEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Warranty.Web.ViewComponents
{
    public class UserNameHeaderViewComponent : ViewComponent
    {
        #region Variable
        public ISessionManager _sessionManager = null;
        protected ICommonProvider _commonProvider;
        #endregion

        #region Constructor

        public UserNameHeaderViewComponent(ICommonProvider commonProvider, ISessionManager sessionManager)
        {
            _commonProvider = commonProvider;
            _sessionManager = sessionManager;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserHeaderDetailViewModel userMasterViewModel = new UserHeaderDetailViewModel() { UserHeaderDetail = new UserHeaderDetailModel() };
            userMasterViewModel.RoleId = _sessionManager.RoleId;
            userMasterViewModel.UserHeaderDetail.FullName = _sessionManager.Username;
            userMasterViewModel.UserHeaderDetail.RoleName = _sessionManager.RoleName;
            return View(userMasterViewModel);
        }
    }
}
