using Warranty.Common.BussinessEntities;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Warranty.Web.Controllers;
using Warranty.Common.BusinessEntitiess;

namespace Warranty.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Variable
        private IDataProtector _protector;
        private IUserMasterProvider _userMasterProvider;
        public const string Temp_Success = "Success";
        public const string Temp_Error = "Error";
        #endregion

        #region  Constructor

        public AccountController(IUserMasterProvider userMasterProvider, ISessionManager sessionManager, ICommonProvider commonProvider, IDataProtectionProvider provider) : base(commonProvider, sessionManager)
        {
            _userMasterProvider = userMasterProvider;
            _protector = provider.CreateProtector(AppCommon.Protection);
        }
        #endregion
        public IActionResult Index()
        {
            CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
            _sessionManager.CaptchaCode = captcha.CatpchaCode;
            LoginModel model = new LoginModel
            {
                CaptchaImage = captcha.CaptchaBase64,
            };
            return View(model);
        }
        [HttpPost]
        public JsonResult Login(LoginModel model)
        {
            AuthResultModel res = new AuthResultModel();
            if (_sessionManager.CaptchaCode == model.CaptchaCode)
            {
                model.SessionId = _sessionManager.GetSessionId();
                res = _userMasterProvider.Authentication(model, _sessionManager.GetIP());
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "Invalid captcha.";
            }

            if (res.IsSuccess)
            {
                _sessionManager.UserId = res.UserId;
                _sessionManager.Username = res.Username;
                _sessionManager.RoleId = res.RoleId;
                _sessionManager.Emailid = res.Emailid;
                _sessionManager.RoleName = res.RoleName;
                _sessionManager.EnggId = res.EnggId;

                _userMasterProvider.InsertLoginHistory(res.UserId, _sessionManager.GetSessionId(), _sessionManager.GetIP());
            }
            return Json(res);
        }

        public JsonResult ValidateCaptcha(string captcha)
        {
            if (_sessionManager.CaptchaCode == captcha)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult RefreshCaptcha()
        {
            CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
            _sessionManager.CaptchaCode = captcha.CatpchaCode;
            return Json(captcha.CaptchaBase64);
        }

        public IActionResult ForgotPassword(string Id)
        {
            CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
            _sessionManager.CaptchaCode = captcha.CatpchaCode;
            MasterViewModel model = new MasterViewModel();
            model.Login = new LoginModel();
            model.Login.CaptchaImage = captcha.CaptchaBase64;

            if (!string.IsNullOrEmpty(Id))
            {
                string parm = _commonProvider.UnProtectString(Id);
                if (parm == "1")
                    TempData[Temp_Error] = "The link was expired, please re-generate new link!";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ResetPass(MasterViewModel model)
        {
            AuthResultModel response = new AuthResultModel();

            // Validate if the email field is empty
            if (string.IsNullOrEmpty(model.Login.Emailid))
            {
                response.Message = "Email is required!";
                response.IsSuccess = false;
                // Generate a new Captcha
                CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
                _sessionManager.CaptchaCode = captcha.CatpchaCode;
                model.Login.CaptchaCode = captcha.CaptchaBase64;
                return Json(response);
            }

            // Validate Captcha
            if (string.IsNullOrEmpty(model.Login.CaptchaCode))
            {
                response.Message = "Captcha is required!";
                response.IsSuccess = false;
                // Generate a new Captcha
                CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
                _sessionManager.CaptchaCode = captcha.CatpchaCode;
                model.Login.CaptchaCode = captcha.CaptchaBase64;
                return Json(response);
            }

            // Check if the Captcha is correct
            if (model.Login.CaptchaCode == _sessionManager.CaptchaCode)
            {
                // Call the VerifyUsername method with the email provided
                ResponseModel res = _userMasterProvider.VerifyUsername(model.Login);
                response.Message = res.Message;
                response.IsSuccess = res.IsSuccess;
            }
            else
            {
                response.Message = "Invalid Captcha!";
                response.IsSuccess = false;
            }

            // Generate a new Captcha for the next request
            CaptchaResult newCaptcha = Captcha.Generate(CaptchaType.Simple);
            _sessionManager.CaptchaCode = newCaptcha.CatpchaCode;
            model.Login.CaptchaCode = newCaptcha.CaptchaBase64;

            return Json(response);
        }


        [HttpPost]
        public JsonResult SendRessetPasswordMail(string id)
        {
            return Json(_userMasterProvider.SendRessetPasswordMail(_commonProvider.UnProtect(id)));
        }

        [HttpGet]
        public IActionResult Reset(string id)

        {
            if (!string.IsNullOrEmpty(id))
            {
                string parm = "";
                string[] parms;
                try
                {
                    parm = _protector.Unprotect(id);
                    parms = parm.Split('|');
                    DateTime dt = new DateTime(Convert.ToInt64(parms[1]));
                    dt = dt.AddMinutes(30);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Account");
                }
                ResetPasswordModel model = new ResetPasswordModel();
                var user = _userMasterProvider.GetUserById(Convert.ToInt32(parms[0]));
                if (user != null)
                {
                    model = new ResetPasswordModel
                    {
                        EncId = _protector.Protect(user.UserId.ToString()),
                        Username = user.Email
                    };
                    CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
                    _sessionManager.CaptchaCode = captcha.CatpchaCode;
                    model.CaptchaImage = captcha.CaptchaBase64;
                    model.CaptchaCode = "";
                }
                else
                    return RedirectToAction("Index", "Account");

                return View(model);
            }
            else
                return RedirectToAction("Index", "Account");
        }

        public IActionResult Logout()
        {
            try
            {
                _sessionManager.ClearSession();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        public IActionResult Reset(ResetPasswordModel model)
        {
            if (!string.IsNullOrEmpty(model.EncId))
            {
                ResponseModel returnResult = new ResponseModel();

                if (_sessionManager.CaptchaCode == model.CaptchaCode)
                {
                    string userId = _protector.Unprotect(model.EncId);
                    UserMastModel userMaster = new UserMastModel
                    {
                        UserId = Convert.ToInt32(userId),
                        UserPassword = AES.DecryptAES(model.Password),
                        ConfirmPassword = AES.DecryptAES(model.ConfirmPassword),
                        EncId = model.EncId
                    };
                    returnResult = _commonProvider.ChangeOrResetPassword(userMaster, false, _sessionManager.GetIP());

                    if (returnResult.IsSuccess)
                        return RedirectToAction("Index", "Account");
                    else
                    {
                        model.Message = returnResult.Message;
                        model.IsSuccess = false;

                    }
                }
                else
                    model.Message = "Invalid Captcha!";
                CaptchaResult captcha = Captcha.Generate(CaptchaType.Simple);
                _sessionManager.CaptchaCode = captcha.CatpchaCode;
                model.CaptchaImage = captcha.CaptchaBase64;
                model.CaptchaCode = "";
                return View(model);
            }
            else
                return RedirectToAction("Index", "Account");
        }

        public PartialViewResult ChangePassword()
        {
            UserMastViewModel model = new UserMastViewModel
            {
                UserMaster = new UserMastModel
                {
                    UserId = 0,
                    EncId = _commonProvider.Protect(_sessionManager.UserId)
                }
            };
            return PartialView("_ChangePassword", model);
        }

        [HttpPost]
        public JsonResult ChangePassword(UserMastViewModel model)
        {
            return Json(_commonProvider.ChangeOrResetPassword(model.UserMaster, true, _sessionManager.GetIP()));
        }
    }
}
