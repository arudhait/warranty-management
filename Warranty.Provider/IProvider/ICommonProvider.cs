using Warranty.Common.BussinessEntities;
using Warranty.Common.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;

namespace Warranty.Provider.IProvider
{
    public interface ICommonProvider
    {
        #region Encrypt Properties

        string Protect(int value);

        string ProtectShort(short value);
        string ProtectInt64(Int64 value);
        int UnProtect(string value);
        Int64 UnprotectInt64(string protectedValue);
        string ProtectString(string value);

        string UnProtectString(string value);

        long UnProtectLong(string value);
        #endregion

        List<MenuModel> GetMenuList(SessionProviderModel sessionProviderModel);
        List<UserTypeMastModel> GetRoleDropdownList(SessionProviderModel sessionProvider);
        ResponseModel ChangeOrResetPassword(UserMastModel userData, bool isChangePwd, string IP);
       
        ResponseModel PasswordValidation(UserMastModel userData, bool isChangePwd);

        List<StateMastModel> GetStateList();

        List<DistrictMastModel> GetDistrictList();

        List<CustMastModel> GetCustomerList();
        List<SupplierMasterModel> GetSupplierList();

        List<ActionMastModel> GetActionMastList();

        List<BreakdownStatusMastModel> GetBreakdownTypeList();

        List<EnggMastModel> GetEngineerList();
        List<CustMastModel> GetDocotorList();
        List<ModelMasterModel> GetModelList();
        List<ProductMasterModel> GetProductList();
        List<EnggMastModel> GetEngeeList();
        List<ContractTypeMasterModel> GetContractTypeList();
        ResponseModel CovertExcelToModel<TDATA>(Stream file, ref List<TDATA> tDATAs) where TDATA : new();
        bool IsHaveAccess(short tableId, int roleId);
    }
}
