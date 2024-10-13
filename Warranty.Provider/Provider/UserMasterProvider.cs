using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Models;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class UserMasterProvider : IUserMasterProvider
    {
        #region Variable
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public UserMasterProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Login
        public AuthResultModel Authentication(LoginModel login, string Ip)
        {
            AuthResultModel model = new AuthResultModel();
            try
            {
                var userData = unitOfWork.UserMast.Get(x => x.UserName.ToLower() == login.UserName.ToLower() && x.IsActive == true);
                if (userData != null)
                {
                    int loginAtmpt = GetLoginAttampt(login.UserName);
                    if (loginAtmpt >= 3)
                    {
                        model.IsSuccess = false;
                        model.Message = "Failed to Login, Your account is blocked for 15 min.!";
                        return model;
                    }
                    if (PasswordHash.ValidatePassword(AES.DecryptAES(login.UserPassword), userData.UserPassword))
                    {
                        model.UserId = userData.UserId;
                        model.Username = userData.UserName;
                        model.RoleId = userData.RoleId;
                        model.RoleName = userData.Role.UserTypeName;
                        model.Emailid = userData.Email;
                        model.Username = userData.UserName;
                        model.IsSuccess = true;

                        if (userData.RoleId == 3)
                        {
                            var engineerData = unitOfWork.EnggMast.Get(e => e.EnggId.ToString().ToLower() == userData.EnggId.ToString().ToLower());

                            model.EnggId = (int)engineerData.EnggId;
                            if (engineerData != null)
                            {
                                if (engineerData.EnggId.ToString().ToLower() == model.EnggId.ToString().ToLower())
                                {
                                    model.EnggId = engineerData.EnggId;
                                }
                                else
                                {
                                    model.IsSuccess = false;
                                    model.Message = "Failed to Login, Email mismatch!";
                                    return model;
                                }
                            }



                            else
                            {
                                model.IsSuccess = false;
                                model.Message = "Failed to Login, Engineer data not found!";
                                return model;
                            }
                        }
                        else if (userData.RoleId == 1)
                        {
                            model.EnggId = -1;
                        }

                        RemoveLoginFailure(login.UserName);
                    }
                    else
                    {
                        model.IsSuccess = false;
                        int loginAttempt = InsertLoginFailure(login, userData.UserId, Ip);
                        if (loginAttempt < 3)
                            model.Message = "Failed to Login, " + (3 - loginAttempt) + " attempt left!";
                        else
                            model.Message = "Failed to Login, Your account is blocked for 15 min.!";
                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "Invalid Username or Password!";
                }
            }
            catch (Exception ex)
            {
                model.Message = AppCommon.ErrorMessage;
                model.IsSuccess = false;
                AppCommon.LogException(ex, "UserMasterProvider => Authentication");
            }
            return model;
        }
        public ResponseModel VerifyUsername(LoginModel model)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                // Attempt to find the user by email
                var _user = unitOfWork.UserMast
                    .GetAll(x => x.Email == model.Emailid && x.IsActive)
                    .FirstOrDefault();

                if (_user != null)
                {
                    if (_user.IsActive)
                    {
                        // Create a reset password link
                        string resetLink = AppCommon.AppUrl + "Account/Reset/" +
                            _commonProvider.ProtectString(_user.UserId.ToString() + "|" + DateTime.Now.Ticks.ToString());

                        string mailBody = $@"Hello {_user.UserName},<br><br>
                            Click the link below to reset your password.<br><br> 
                            <a href='{resetLink}'>{resetLink}</a><br><br>                                   
                            <b>Note:</b> This link will expire within 30 minutes.<br> 
                            If the link is not clickable, please copy and paste it into your browser.<br><br>
                            <b>Thanks & Regards</b><br>
                            {AppCommon.ApplicationLongTitle}";

                        EmailSender.SendEmail(_user.Email, AppCommon.ApplicationTitle, "Reset Password", mailBody);
                        res.IsSuccess = true;
                        res.Message = "A reset password link has been sent to your email.";
                    }
                    else
                    {
                        res.Message = $"'{_user.UserName}' is inactive. Please contact the administrator.";
                    }
                }
                else
                {
                    res.Message = "Invalid email address!";
                }
            }
            catch (Exception ex)
            {
                res.Message = "An error occurred while processing your request.";
                // Consider logging the exception here for further analysis
            }
            return res;
        }


        public ResponseModel InsertLoginHistory(int userId, string sessionId, string ip)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                LoginHistory history = new LoginHistory
                {
                    UserMasterId = userId,
                    LoggedOn = AppCommon.CurrentDate,
                    Status = true,
                    SessionId = sessionId,
                };
                unitOfWork.LoginHistory.Insert(history, 1, ip);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvuder=>InsertLoginHistory");
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
            }
            return model;
        }
        public UserMastModel GetUserById(int id)
        {
            UserMastModel model = new UserMastModel();
            try
            {
                if (id > 0)
                {
                    UserMast user = unitOfWork.UserMast.GetAll(x => x.UserId == id).FirstOrDefault();
                    if (user != null)
                    {

                        model = _mapper.Map<UserMastModel>(user);
                        //model.Email = model.UserName;
                        model.EncId = _commonProvider.Protect(model.UserId);
                        model.UserName = model.UserName;
                        model.Email = model.Email;
                        model.IsActive = model.IsActive;
                        if (user.Role != null)
                            model.UserTypeName = user.Role.UserTypeName;

                    }
                }
            }

            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider=>GetUserById");
            }
            return model;
        }

        public ResponseModel SaveResetPassword(UserMastModel userModel, SessionProviderModel sessionProviderModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(userModel.EncId))
                    userModel.UserId = _commonProvider.UnProtect(userModel.EncId);
                var _temp = unitOfWork.UserMast.GetAll(x => x.UserId == userModel.UserId).FirstOrDefault();
                UserMast userMaster = _temp != null ? _temp : new UserMast();
                if (_temp != null)
                {
                    var passwordRes = _commonProvider.PasswordValidation(userModel, false);
                    if (!passwordRes.IsSuccess)
                        return passwordRes;
                    userMaster.UserPassword = PasswordHash.CreateHash(userModel.UserPassword);
                    unitOfWork.UserMast.Update(userMaster, sessionProviderModel.UserId, sessionProviderModel.Ip);
                    model.Message = "Reset password successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "UserMasterProvider=>SaveResetPassword");
            }
            return model;
        }

        public ResponseModel SendRessetPasswordMail(int id)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var _user = unitOfWork.UserMast.GetAll(x => x.UserId == id).FirstOrDefault();
                if (_user != null)
                {
                    if (_user.IsActive)
                    {
                        string resetLink = AppCommon.AppUrl + "Account/Reset/" + _commonProvider.ProtectString(_user.UserId.ToString() + "|" + AppCommon.CurrentDate.Ticks.ToString());

                        string mailBody = $@"Hello {_user.UserName}<br><br>
                                    
                                        Click below link to reset your password. <br><br> 
                                        <a href='{resetLink}'>{resetLink}</a><br>   <br>                                   
                                        <b>Note:</b> Above link will expire with in 30 min.<br> 
                                        If link is not clickable then please copy and paste in your browser.<br><br>
                                        <b>Thanks & Regard</b><br>
                                        {AppCommon.ApplicationTitle}
                                    ";

                        EmailSender.SendEmail(_user.Email, AppCommon.ApplicationTitle, "Reset Password", mailBody);
                        res.IsSuccess = true;
                        res.Message = "Reset password link sent to your email. Please check your inbox/junk folder.";
                    }
                    else
                        res.Message = "'" + _user.UserName + "' is inactive, Please contact administrator.";
                }
                else
                    res.Message = "User data not found. Please verify!";

            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider=>SendRessetPasswordMail");
                res.IsSuccess = false;
                res.Message = AppCommon.ErrorMessage;
            }
            return res;
        }

        #endregion

        #region Private Methods
        private int GetLoginAttampt(string userName)
        {
            int count = 0;
            try
            {
                DateTime currentdatetime = DateTime.Now.AddMinutes(-15);
                count = unitOfWork.LoginFailure.GetAll(x => x.Username == userName && x.CreatedOn > currentdatetime).Count();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider=>GetLoginAttampt");
            }
            return count;
        }
        private void RemoveLoginFailure(string userName)
        {
            try
            {
                var loginFailHistory = unitOfWork.LoginFailure.GetAll(x => x.Username == userName);
                if (loginFailHistory != null)
                {
                    unitOfWork.LoginFailure.DeleteAll(loginFailHistory);
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider=>RemoveLoginFailure");
            }
        }
        private int InsertLoginFailure(LoginModel login, int userId, string Ip)
        {
            try
            {
                LoginFailure failure = new LoginFailure
                {
                    Username = login.UserName
                };
                unitOfWork.LoginFailure.Insert(failure, userId, Ip);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider=>InsertLoginFailure");
            }
            return GetLoginAttampt(login.UserName);
        }
        #endregion

        #region IUserMasterProvider
        public DatatablePageResponseModel<UserMastModel> GetList(DatatablePageRequestModel datatablepageReqest)
        {
            DatatablePageResponseModel<UserMastModel> returnResult = new DatatablePageResponseModel<UserMastModel>
            {
                data = new List<UserMastModel>(),
                draw = datatablepageReqest.Draw
            };
            try
            {
                var userList = (from u in unitOfWork.UserMast.GetAll(x => x.UserId != (int)Enumeration.Role.SuperAdmin)
                                select new UserMastModel()
                                {
                                    UserId = u.UserId,
                                    EnggId = u.EnggId,
                                    UserName = u.UserName,
                                    UserPassword = u.UserPassword,
                                    RoleId = u.RoleId,
                                    UserTypeName = u.Role.UserTypeName,
                                    Email = u.Email,
                                    IsActive = u.IsActive,
                                    CreatedDate = u.CreatedDate,
                                    CreatedOnString = u.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = u.CreatedBy
                                }).ToList();
                returnResult.recordsTotal = userList.Count();
                if (datatablepageReqest.RoleId > 0)
                {
                    userList = userList.Where(x => x.RoleId == datatablepageReqest.RoleId).ToList();
                }
                // Filter by ExtraSearch if provided
                if (!string.IsNullOrEmpty(datatablepageReqest.ExtraSearch))
                {
                    int status = Convert.ToInt32(datatablepageReqest.ExtraSearch);
                    if (status == 1)
                        userList = userList.Where(x => x.UserStatus).ToList();
                    else if (status == 2)
                        userList = userList.Where(x => !x.UserStatus).ToList();
                }
                if (!string.IsNullOrEmpty(datatablepageReqest.SearchText))
                {
                    string searchText = datatablepageReqest.SearchText.ToLower();
                    userList = userList.Where(x =>
                        x.UserName.ToLower().Contains(datatablepageReqest.SearchText.ToLower()) ||
                        x.Email.ToLower().Contains(datatablepageReqest.SearchText.ToLower())
                    ).ToList();
                }
                returnResult.recordsFiltered = userList.Count();
                //sorting
                if (!string.IsNullOrEmpty(datatablepageReqest.SortColumnName))
                {
                    userList = userList.AsQueryable().OrderBy(datatablepageReqest.SortColumnName + " " + datatablepageReqest.SortDirection).ToList();
                }
                //paging
                returnResult.data = userList.Skip(datatablepageReqest.StartIndex).Take(datatablepageReqest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.UserId);
                    //var loginHistory = unitOfWork.LoginHistory.GetAll(c => c.UserMasterId == x.UserMasterId).OrderByDescending(c => c.LoggedOn).FirstOrDefault();
                    //if (loginHistory != null) x.LastVisited = loginHistory.LoggedOn;
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider => GetList");
            }
            return returnResult;
        }
        public UserMastModel GetById(int id)
        {
            UserMastModel model = new();
            try
            {
                var data = unitOfWork.UserMast.GetAll(x => x.UserId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<UserMastModel>(data);
                    model.EncId = _commonProvider.Protect(id);

                    if (data.RoleId == 3)  
                    {
                        model.RoleId = 3;  
                        model.IsActive = true;
                    }
                    if (data.RoleId == 2) 
                    {
                        model.RoleId = 2;  
                        model.IsActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider=>GetById");
            }
            return model;
        }
        public ResponseModel Save(UserMastModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.UserId = _commonProvider.UnProtect(inputModel.EncId);
                var _temp = unitOfWork.UserMast.GetAll(x => x.UserId == inputModel.UserId).FirstOrDefault();

                var user = unitOfWork.UserMast.GetAll(x => x.UserId == inputModel.UserId).Select(x => new
                {
                    x.UserId,  
                    x.EnggId   
                }).FirstOrDefault();

                // Declare the EnggId variable outside the conditional scope
                int? enggId = null;

                if (user != null)
                {
                    // Use the fetched EnggId
                    enggId = user.EnggId; // This will store the EnggId
                }

                if (unitOfWork.UserMast.Any(x => x.UserId != inputModel.UserId && x.UserName == inputModel.UserName))
                {
                    model.IsSuccess = false;
                    model.Message = "User already exist with this username/email address";
                    return model;
                }
                if (string.IsNullOrEmpty(inputModel.UserPassword))
                {
                    inputModel.UserPassword = _temp.UserPassword;
                }
                UserMast tableData = _mapper.Map(inputModel, _temp);

                if (_temp == null)
                {
                    var passwordRes = _commonProvider.PasswordValidation(inputModel, false);
                    if (!passwordRes.IsSuccess)
                        return passwordRes;
                    tableData.UserPassword = PasswordHash.CreateHash(inputModel.UserPassword);
                    tableData.IsActive = true;
                    tableData.RoleId = 2;
                    unitOfWork.UserMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "User added successfully";
                }
                else
                {
                    // Now you can use the fetched EnggId
                    if (enggId != null)
                    {
                        tableData.EnggId = enggId; // Keep the same EnggId during the update
                    }
                    tableData.IsActive = true;
                    tableData.RoleId = 2;
                    unitOfWork.UserMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "User updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "UserMasterProvider=>Save");
            }
            return model;
        }
        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                UserMast product = unitOfWork.UserMast.GetAll(x => x.UserId == id).FirstOrDefault();
                if (product != null)
                {

                    returnResult.Message = "User deleted successfully.";
                    product.IsActive = false;
                    unitOfWork.UserMast.Update(product, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "product record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "UserMasterProvider=>Delete");
            }
            return returnResult;
        }
        #endregion

        #region User Method
        public DatatablePageResponseModel<UserMastModel> GetUserList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<UserMastModel> model = new DatatablePageResponseModel<UserMastModel>
            {
                data = new List<UserMastModel>(),
                draw = datatablePageRequest.Draw
            };
            try
            {
                var listData = (from u in unitOfWork.UserMast.GetAll(x => x.RoleId != (int)Enumeration.Role.SuperAdmin)
                                select new UserMastModel()
                                {
                                    UserId = u.UserId,
                                    UserName = u.UserName,
                                    UserPassword = u.UserPassword,
                                    RoleId = u.RoleId,
                                    UserTypeName = u.Role.UserTypeName,
                                    Email = u.Email,
                                    IsActive = u.IsActive,
                                    CreatedDate = u.CreatedDate,
                                    CreatedOnString = u.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = u.CreatedBy
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (datatablePageRequest.UserTypeId > 0)
                    listData = listData.Where(w => w.RoleId == datatablePageRequest.UserTypeId).ToList();
                if (!string.IsNullOrEmpty(datatablePageRequest.ExtraSearch))
                {
                    int status = Convert.ToInt32(datatablePageRequest.ExtraSearch);
                    if (status == 1)
                        listData = listData.Where(x => (bool)x.IsActive).ToList();
                    else if (status == 2)
                        listData = listData.Where(x => (bool)!x.IsActive).ToList();

                }
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    string searchText = datatablePageRequest.SearchText.ToLower();
                    listData = listData.Where(x =>
                    (x.UserName != null && x.UserName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())) ||
                       (x.Email != null && x.Email.ToLower().Contains(datatablePageRequest.SearchText.ToLower()))
                ).ToList();

                }

                model.recordsFiltered = listData.Count();
                //sorting
                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName))
                {
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();
                }
                //paging
                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.UserId);
                   
                    var loginHistory = unitOfWork.LoginHistory.GetAll(c => c.UserMasterId == x.UserId).OrderByDescending(c => c.LoggedOn).FirstOrDefault();
                    if (loginHistory != null) x.LastVisited = loginHistory.LoggedOn;
                    return x;
                }).ToList();
            }

            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider => GetList");
            }
            return model;
        }
        #endregion


    }
}
