using AutoMapper;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using System.Linq.Dynamic.Core;
using Warranty.Repository.Repository;
using Warranty.Repository.Models;

namespace Warranty.Provider.Provider
{
    public class EngineerProvider : IEngineerProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public EngineerProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<EnggMastModel> GetList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<EnggMastModel> model = new DatatablePageResponseModel<EnggMastModel>
            {
                data = new List<EnggMastModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from p in unitOfWork.EnggMast.GetAll()
                                select new EnggMastModel()
                                {
                                    EnggId = p.EnggId,
                                    EnggName = p.EnggName,
                                    IsActive = p.IsActive,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDatestring = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    Ip = p.Ip,
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.EnggName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.EnggId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "EnginnerProvider=>GetList");
            }
            return model;
        }
        public EnggMastModel GetById(int id)
        {
            EnggMastModel model = new EnggMastModel();
            try
            {
                var data = unitOfWork.EnggMast.GetAll(x => x.EnggId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<EnggMastModel>(data);
                    model.EncId = _commonProvider.Protect(id);
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "EnginnerProvider=>GetById");
            }
            return model;
        }

        public EnggMastModel GetByAllocationId(int id)
        {
            EnggMastModel model = new();
            try
            {
                // Get all EnggMast records and check if EnggId exists in UserMast
                var engg = unitOfWork.EnggMast.GetAll(x => x.EnggId == id).FirstOrDefault();

                if (engg != null)
                {
                    // Map the UserMast data to the UserMastModel
                    model = _mapper.Map<EnggMastModel>(engg);
                    model.EncId = _commonProvider.Protect(id);
                    model.EnggName = engg.EnggName;

                    // Check if EnggId from EnggMast exists in UserMast
                    var data = unitOfWork.UserMast.GetAll(x => x.EnggId == engg.EnggId).FirstOrDefault();

                    if (data != null)
                    {
                        // Map the UserMast data to the UserMastModel
                        model.UserMastModel = _mapper.Map<UserMastModel>(data);

                        // Add a null check before accessing properties
                        if (model.UserMastModel != null)
                        {
                            model.UserMastModel.Email = model.UserMastModel.Email;
                            model.UserMastModel.UserName = model.UserMastModel.UserName;
                            model.UserMastModel.RoleId = model.UserMastModel.RoleId;
                            model.UserMastModel.IsActive = model.UserMastModel.IsActive;
                        }
                    }
                    else
                    {
                        // Ensure that the UserMastModel is not null before assigning values
                        model.UserMastModel = new UserMastModel();
                        model.UserMastModel.RoleId = 3;
                        model.UserMastModel.IsActive = true;
                    }
                }

            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "UserMasterProvider=>GetByAllocationId");
            }

            return model;
        }

        public ResponseModel Save(EnggMastModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.EnggId = _commonProvider.UnProtect(inputModel.EncId);
                if (unitOfWork.EnggMast.Any(x => x.EnggId != inputModel.EnggId && x.EnggName == inputModel.EnggName))
                {
                    model.IsSuccess = false;
                    model.Message = "Enginner already exists with this name/email address";
                    return model;
                }
                var _temp = unitOfWork.EnggMast.GetAll(x => x.EnggId == inputModel.EnggId).FirstOrDefault();
                EnggMast tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.EnggMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Enginner added successfully";
                }
                else
                {
                    unitOfWork.EnggMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Enginner updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "EnginnerProvider=>Save");
            }
            return model;
        }
        public ResponseModel SaveAllocation(EnggMastModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.EnggId = _commonProvider.UnProtect(inputModel.EncId);
                    var engineer = unitOfWork.EnggMast.GetAll(x => x.EnggId == inputModel.EnggId).FirstOrDefault(); // Adjust the condition as per your logic
                if (engineer == null)
                {
                    model.IsSuccess = false;
                    model.Message = "Engineer not found";
                    return model;
                }

                // Store EnggId in a variable
                var enggId = engineer.EnggId;

                
                if (unitOfWork.UserMast.Any(x => x.UserPassword == inputModel.UserMastModel.UserPassword && x.UserId != inputModel.UserMastModel.UserId))
                {
                    model.IsSuccess = false;
                    model.Message = "User already exists with this username/email address";
                    return model;
                }

               

                var _temp = unitOfWork.UserMast.GetAll()
                    .FirstOrDefault(x => x.EnggId == inputModel.EnggId);

             
                if (_temp != null)
                {
                    inputModel.UserMastModel.UserId = _temp.UserId; // This is the UserId you need
                }
               
                if (string.IsNullOrEmpty(inputModel.UserMastModel.UserPassword))
                {
                    inputModel.UserMastModel.UserPassword = _temp?.UserPassword;
                }

              
                UserMast tableData = _mapper.Map(inputModel.UserMastModel, _temp);

                if (_temp == null)
                {
                    var passwordRes = _commonProvider.PasswordValidation(inputModel.UserMastModel, false);
                    if (!passwordRes.IsSuccess)
                        return passwordRes;

                    // Set password and other properties
                    tableData.UserPassword = PasswordHash.CreateHash(inputModel.UserMastModel.UserPassword);
                    tableData.IsActive = true;
                    tableData.RoleId = 3;
                    tableData.EnggId = enggId;
                    unitOfWork.UserMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "User added successfully";

                    unitOfWork.Save();
                    model.IsSuccess = true;

                    var userMasterId = tableData.UserId;
                }
                else
                {
                    var userId = _temp.UserId;

                    var passwordRes = _commonProvider.PasswordValidation(inputModel.UserMastModel, false);
                    if (!passwordRes.IsSuccess)
                        return passwordRes;

                  
                    tableData.UserPassword = PasswordHash.CreateHash(inputModel.UserMastModel.UserPassword);

                    tableData.UserId = inputModel.UserMastModel.UserId;
                    tableData.RoleId = 3;
                    tableData.IsActive = true;
                    tableData.EnggId = enggId;

                    unitOfWork.UserMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "User updated successfully";

                    unitOfWork.Save();
                    model.IsSuccess = true;
                }

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
                EnggMast enginner = unitOfWork.EnggMast.GetAll(x => x.EnggId == id).FirstOrDefault();
                if (enginner != null)
                {

                    returnResult.Message = "Enginner deleted successfully.";
                    enginner.IsActive = false;
                    unitOfWork.EnggMast.Update(enginner, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Enginner record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "EnginnerProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
