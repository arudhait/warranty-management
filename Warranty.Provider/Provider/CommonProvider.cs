using AutoMapper;
using Warranty.Common.BussinessEntities;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Repository;
using Microsoft.AspNetCore.DataProtection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using System.Runtime.CompilerServices;

namespace Warranty.Provider.Provider
{
    public class CommonProvider : ICommonProvider
    {
        #region Variable
        private IDataProtector _IDataProtector;
        private readonly IMapper _mapper;
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ISessionManager _sessionManager;
        protected ICommonProvider _commonProvider;
        #endregion

        #region Constructor
        public CommonProvider(IDataProtectionProvider dataProtector, IMapper mapper)
        {
            _IDataProtector = dataProtector.CreateProtector(AppCommon.Protection);
            _mapper = mapper;
        }
        #endregion

        #region Encrypt/Decrypt

        public string Protect(int value)
        {
            return _IDataProtector.Protect(value.ToString());
        }
        public string ProtectInt64(Int64 value)
        {
            return _IDataProtector.Protect(value.ToString());
        }
        public string ProtectShort(short value)
        {
            return _IDataProtector.Protect(value.ToString());
        }
        public int UnProtect(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string data = _IDataProtector.Unprotect(value);
                data = data ?? "0";
                return Convert.ToInt32(data);
            }
            else
                return 0;

        }
        public Int64 UnprotectInt64(string protectedValue)
        {
            string decryptedValue = _IDataProtector.Unprotect(protectedValue);

            if (Int64.TryParse(decryptedValue, out Int64 result))
            {
                return result;
            }
            else
            {

                throw new InvalidOperationException("Failed to convert decrypted value to Int64.");
            }
        }

        public string ProtectString(string value)
        {
            value = value ?? string.Empty;
            return _IDataProtector.Protect(value);
        }
        public string UnProtectString(string value)
        {
            return _IDataProtector.Unprotect(value);
        }
        public long UnProtectLong(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string data = _IDataProtector.Unprotect(value);
                data = data ?? "0";
                return Convert.ToInt64(data);
            }
            else
                return 0;
        }

        #endregion

        #region Private Methods
        private string GetFormattedColumnName(string inputstring)
        {
            return inputstring.Replace(" ", "").Replace("(mm/dd/yyyy)", "").Replace("_", "").Replace("-", "").Replace("=", "").Replace("#", "").Replace("(", "").Replace(")", "").Replace("/", "").Replace("\\", "").Replace(".", "").Replace(":", "").Replace("<br>", "").Replace("?", "").Trim();
        }
        private bool IsWholeRowEmpty(ExcelWorksheet sheet, int rowIndex, int totalColumns)
        {
            for (int i = 1; i <= totalColumns; i++)
            {
                if (sheet.Cells[rowIndex, i].Value != null && !string.IsNullOrEmpty(sheet.Cells[rowIndex, i].Value.ToString()) && !string.IsNullOrWhiteSpace(sheet.Cells[rowIndex, i].Value.ToString()))
                    return false;
            }
            return true;
        }
        #endregion

        #region Common Provider
        public List<MenuModel> GetMenuList(SessionProviderModel sessionProviderModel)
        {
            List<MenuModel> list = new List<MenuModel>();
            try
            {
                var menu = unitOfWork.Menu.GetAll(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToList();
                if (sessionProviderModel.RoleId != (int)Enumeration.Role.SuperAdmin)
                    menu = menu.Where(x => x.MenuRoles.Any(c => c.RoleId == sessionProviderModel.RoleId)).ToList();

                list = _mapper.Map<List<MenuModel>>(menu);
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetMenuList");
            }
            return list;
        }
        public ResponseModel ChangeOrResetPassword(UserMastModel userData, bool isChangePwd, string IP)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                userData.UserId = UnProtect(userData.EncId ?? string.Empty);
                returnResult = PasswordValidation(userData, isChangePwd);
                if (returnResult.IsSuccess)
                {
                    var user = unitOfWork.UserMast.Get(x => x.UserId == userData.UserId);
                    user.UserPassword = PasswordHash.CreateHash(userData.UserPassword);
                    unitOfWork.UserMast.Update(user, user.UserId, IP);
                    unitOfWork.Save();
                    returnResult.Message = "Password " + (isChangePwd ? "changed" : "reset") + " successfully.";
                    returnResult.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "CommonProvider => ChangeOrResetPassword");
            }
            return returnResult;
        }
        public ResponseModel PasswordValidation(UserMastModel userData, bool isChangePwd)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                if (userData.UserId == 0 && isChangePwd)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "User not exist.";
                    return returnResult;
                }
                else if (userData.UserPassword.Length < 8)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "New password require min. 8 char.";
                    return returnResult;
                }
                else if (userData.UserPassword != userData.ConfirmPassword)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "New & confirm password is not matched.";
                    return returnResult;
                }
                else
                {
                    if (isChangePwd)
                    {
                        var user = unitOfWork.UserMast.GetAll(x => x.UserId == userData.UserId).FirstOrDefault();
                        if (user != null)
                        {
                            if (PasswordHash.ValidatePassword(userData.OldPassword, user.UserPassword))
                                returnResult.IsSuccess = true;
                            else
                            {
                                returnResult.IsSuccess = false;
                                returnResult.Message = "Invalid old password";
                                return returnResult;
                            }
                        }
                        else
                        {
                            returnResult.IsSuccess = false;
                            returnResult.Message = "User not exist.";
                            return returnResult;
                        }
                    }
                    else
                        returnResult.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider => PasswordValidation");
            }
            return returnResult;
        }
        public List<UserTypeMastModel> GetRoleDropdownList(SessionProviderModel sessionProvider)
        {
            List<UserTypeMastModel> result = new List<UserTypeMastModel>();
            try
            {
                result = (from r in unitOfWork.UserTypeMast.GetAll(x => x.RoleId != (int)Enumeration.Role.SuperAdmin)
                          select new UserTypeMastModel()
                          {
                              UserTypeId = r.RoleId,
                              UserTypeName = r.UserTypeName,
                          }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetRoleDropdownList");
            }
            return result;
        }

        public ResponseModel CovertExcelToModel<TDATA>(Stream file, ref List<TDATA> tDATAs) where TDATA : new()
        {
            ResponseModel returnResult = new ResponseModel();
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheets currentSheet = package.Workbook.Worksheets;
            ExcelWorksheet workSheet = currentSheet.FirstOrDefault();
            try
            {
                package.Load(file);
                ImportColumnWithDataModel hwdModel = new ImportColumnWithDataModel();
                hwdModel.ColumnWithData = new List<KeyValuePair<string, List<string>>>();
                using (currentSheet = package.Workbook.Worksheets)
                {
                    using (workSheet = currentSheet.First())
                    {
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            hwdModel.ColumnWithData.Add(new KeyValuePair<string, List<string>>(workSheet.Cells[1, i].Text.ToLower(), new List<string>()));
                        }

                        if (noOfRow <= 1)
                        {
                            returnResult.Message = "<span style='color:red'>No data available in uploaded file.</span>";
                            returnResult.IsSuccess = false;
                            return returnResult;
                        }

                        var hObj = new KeyValuePair<string, List<string>>();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            int hIndx = 1;
                            if (!IsWholeRowEmpty(workSheet, rowIterator, noOfCol))
                            {
                                foreach (var header in hwdModel.ColumnWithData)
                                {
                                    hObj = hwdModel.ColumnWithData.FirstOrDefault(t => t.Key.ToLower().Trim() == header.Key.ToLower().Trim());
                                    hObj.Value.Add(Convert.ToString(workSheet.Cells[rowIterator, hIndx].Value));
                                    hIndx++;
                                }
                            }
                        }


                        if (hwdModel != null && hwdModel.ColumnWithData != null && hwdModel.ColumnWithData.Count() > 0)
                        {
                            var DyObjectsList = new List<dynamic>();
                            int rowCount = hwdModel.ColumnWithData.FirstOrDefault().Value.Count();
                            PropertyInfo prop = null;
                            for (int i = 0; i < rowCount; i++)
                            {
                                var dynModel = new ExpandoObject() as IDictionary<string, Object>;
                                TDATA tdata = new TDATA();
                                foreach (var header in hwdModel.ColumnWithData)
                                {
                                    prop = tdata.GetType().GetProperty(GetFormattedColumnName(header.Key), BindingFlags.Public | BindingFlags.Instance);
                                    if (prop != null && prop.CanWrite)
                                        prop.SetValue(tdata, header.Value[i], null);
                                }
                                tDATAs.Add(tdata);
                            }
                        }


                    }
                }
            }

            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>CovertExcelToModel");
            }
            if (tDATAs.Any())
            {
                returnResult.Message = "Success";
                returnResult.IsSuccess = true;
            }
            else
            {
                returnResult.Message = "<span style='color:red'>No data available in uploaded file.</span>";
                returnResult.IsSuccess = true;
            }
            return returnResult;
        }

        public bool IsHaveAccess(short tableId, int roleId)
        {
            try
            {
                return unitOfWork.MenuRole.Any(x => x.MenuId == tableId && x.RoleId == roleId);
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>IsHaveAccess");
                return false;
            }
        }
        #endregion


        #region DropDown List
        public List<StateMastModel> GetStateList()
        {
            List<StateMastModel> list = new List<StateMastModel>();
            try
            {
                list = _mapper.Map<List<StateMastModel>>(unitOfWork.StateMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }

        public List<CustMastModel> GetDocotorList()
        {
            List<CustMastModel> list = new List<CustMastModel>();
            try
            {
                list = _mapper.Map<List<CustMastModel>>(unitOfWork.CustMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }
        public List<ModelMasterModel> GetModelList()
        {
            List<ModelMasterModel> list = new List<ModelMasterModel>();
            try
            {
                list = _mapper.Map<List<ModelMasterModel>>(unitOfWork.ModelMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }
        public List<ProductMasterModel> GetProductList()
        {
            List<ProductMasterModel> list = new List<ProductMasterModel>();
            try
            {
                list = _mapper.Map<List<ProductMasterModel>>(unitOfWork.ProductMaster.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductList");
            }
            return list;
        }
        public List<ContractTypeMasterModel> GetContractTypeList()
        {
            List<ContractTypeMasterModel> list = new List<ContractTypeMasterModel>();
            try
            {
                list = _mapper.Map<List<ContractTypeMasterModel>>(unitOfWork.ContractTypeMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }
        public List<EnggMastModel> GetEngeeList()
        {
            List<EnggMastModel> list = new List<EnggMastModel>();
            try
            {
                list = _mapper.Map<List<EnggMastModel>>(unitOfWork.EnggMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }
        public List<DistrictMastModel> GetDistrictList()
        {
            List<DistrictMastModel> list = new List<DistrictMastModel>();
            try
            {
                list = _mapper.Map<List<DistrictMastModel>>(unitOfWork.DistrictMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }

        public List<CustMastModel> GetCustomerList()
        {
            List<CustMastModel> list = new List<CustMastModel>();
            try
            {
                list = _mapper.Map<List<CustMastModel>>(unitOfWork.CustMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }

        public List<SupplierMasterModel> GetSupplierList()
        {
            List<SupplierMasterModel> list = new List<SupplierMasterModel>();
            try
            {
                list = _mapper.Map<List<SupplierMasterModel>>(unitOfWork.SupplierMaster.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetSupplierList");
            }
            return list;
        }

        public List<ActionMastModel> GetActionMastList()
        {
            List<ActionMastModel> list = new List<ActionMastModel>();
            try
            {
                list = _mapper.Map<List<ActionMastModel>>(unitOfWork.ActionMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }

        public List<BreakdownStatusMastModel> GetBreakdownTypeList()
        {
            List<BreakdownStatusMastModel> list = new List<BreakdownStatusMastModel>();
            try
            {
                list = _mapper.Map<List<BreakdownStatusMastModel>>(unitOfWork.BreakdownStatusMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }

        public List<EnggMastModel> GetEngineerList()
        {
            List<EnggMastModel> list = new List<EnggMastModel>();
            try
            {
                list = _mapper.Map<List<EnggMastModel>>(unitOfWork.EnggMast.GetAll().ToList());
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CommonProvider=>GetProductTypeList");
            }
            return list;
        }
        #endregion
    }
}
