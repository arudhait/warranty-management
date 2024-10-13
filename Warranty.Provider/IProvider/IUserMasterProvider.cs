using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IUserMasterProvider
    {
        AuthResultModel Authentication(LoginModel login, string Ip);
        ResponseModel VerifyUsername(LoginModel model);
        ResponseModel InsertLoginHistory(int userId, string sessionId, string ip);
        UserMastModel GetUserById(int id);
        ResponseModel SaveResetPassword(UserMastModel userModel, SessionProviderModel sessionProviderModel);
        ResponseModel SendRessetPasswordMail(int id);
        DatatablePageResponseModel<UserMastModel> GetUserList(DatatablePageRequestModel datatablePageRequest);
        UserMastModel GetById(int id);
        ResponseModel Save(UserMastModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
