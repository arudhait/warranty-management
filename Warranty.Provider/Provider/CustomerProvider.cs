using AutoMapper;
using System.Linq.Dynamic.Core;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Models;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class CustomerProvider : ICustomerProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CustomerProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<CustMastModel> GetList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<CustMastModel> model = new DatatablePageResponseModel<CustMastModel>
            {
                data = new List<CustMastModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from p in unitOfWork.CustMast.GetAll()
                                select new CustMastModel()
                                {
                                    CustId = p.CustId,
                                    DoctorName = p.DoctorName,
                                    HospitalName = p.HospitalName,
                                    PostalAddress = p.PostalAddress,
                                    Designation = p.Designation,
                                    MobileNo = p.MobileNo,
                                    PhoneNo = p.PhoneNo,
                                    Email = p.Email,
                                    Pincode = p.Pincode,
                                    StateId = p.StateId,
                                    StateName = p.State.StateName,
                                    DistrictId = p.DistrictId,
                                    DistrictName = p.District.DistrictName,
                                    City = p.City,
                                    CityStatePin = $"{p.City}, {p.State.StateName}, {p.Pincode}",
                                    PndtCertiNo = p.PndtCertiNo,
                                    RegDate = p.RegDate,
                                    IsActive = (bool)p.IsActive,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDatestring = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) ||
                    x.HospitalName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.CustId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CustomerProvider=>GetList");
            }
            return model;
        }
        public CustMastModel GetById(int id)
        {
            CustMastModel model = new CustMastModel();
            try
            {
                var data = unitOfWork.CustMast.GetAll(x => x.CustId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<CustMastModel>(data);
                    model.EncId = _commonProvider.Protect(id);
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "CustomerProvider=>GetById");
            }
            return model;
        }
        public ResponseModel Save(CustMastModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.CustId = _commonProvider.UnProtect(inputModel.EncId);
                if (unitOfWork.CustMast.Any(x => x.CustId != inputModel.CustId && x.DoctorName == inputModel.DoctorName))
                {
                    model.IsSuccess = false;
                    model.Message = "Customer already exists with this name/email address";
                    return model;
                }
                var _temp = unitOfWork.CustMast.GetAll(x => x.CustId == inputModel.CustId).FirstOrDefault();
                CustMast tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.CustMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Customer added successfully";
                }
                else
                {
                    unitOfWork.CustMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Customer updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "CustomerProvider=>Save");
            }
            return model;
        }
        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                CustMast customer = unitOfWork.CustMast.GetAll(x => x.CustId == id).FirstOrDefault();
                if (customer != null)
                {

                    returnResult.Message = "Customer deleted successfully.";
                    customer.IsActive = false;
                    unitOfWork.CustMast.Update(customer, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Customer record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "CustomerProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
