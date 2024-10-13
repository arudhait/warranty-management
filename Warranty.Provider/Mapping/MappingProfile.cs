using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;
using Warranty.Repository.Models;

namespace Warranty.Provider.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ModelMasterModel, ModelMast>();
            CreateMap<ModelMast, ModelMasterModel>();

            CreateMap<ContractTypeMasterModel, ContractTypeMast>();
            CreateMap<ContractTypeMast, ContractTypeMasterModel>();

            CreateMap<UserMastModel, UserMast>();
            CreateMap<UserMast, UserMastModel>();

            CreateMap<MenuModel, Menu>();
            CreateMap<Menu, MenuModel>();        
            
            CreateMap<StateMastModel,StateMast>();
            CreateMap<StateMast,StateMastModel>();

            CreateMap<DistrictMastModel, DistrictMast>();
            CreateMap<DistrictMast, DistrictMastModel>();

            CreateMap<UserTypeMastModel, UserTypeMast>();
            CreateMap<UserTypeMast, UserTypeMastModel>();

            CreateMap<EnggMastModel, EnggMast>();
            CreateMap<EnggMast, EnggMastModel>();

            CreateMap<BreakdownStatusMastModel, BreakdownStatusMast>();
            CreateMap<BreakdownStatusMast, BreakdownStatusMastModel>();

            CreateMap<CustMastModel, CustMast>();
            CreateMap<CustMast, CustMastModel>();

            CreateMap<BreakdownDetModel,BreakdownDet>();
            CreateMap<BreakdownDet, BreakdownDetModel>();

            CreateMap<ActionMastModel, ActionMast>();
            CreateMap<ActionMast, ActionMastModel>();

            CreateMap<WarrantyDetailsModel, WarrantyDet>();
            CreateMap<WarrantyDet, WarrantyDetailsModel>();

            CreateMap<ModelDetailModel, ModelDet>();
            CreateMap<ModelDet, ModelDetailModel>();

            CreateMap<ProbDetailModel, ProbDet>();
            CreateMap<ProbDet, ProbDetailModel>();

            CreateMap<ContractDetModel, ContractDet>();
            CreateMap<ContractDet, ContractDetModel>();

            CreateMap<TerritoryAllocationModel, TerritoryAllocation>();
            CreateMap<TerritoryAllocation, TerritoryAllocationModel>();

            CreateMap<ProductMasterModel, ProductMaster>();
            CreateMap<ProductMaster, ProductMasterModel>();

            CreateMap<SupplierMasterModel, SupplierMaster>();
            CreateMap<SupplierMaster, SupplierMasterModel>();

            CreateMap<InwardOutwardModel, InwardOutward>();
            CreateMap<InwardOutward, InwardOutwardModel>();

            CreateMap<InwardOutwardItemModel, InwardOutwardItem>();
            CreateMap<InwardOutwardItem, InwardOutwardItemModel>();

            CreateMap<LedgerModel, Ledger>();
            CreateMap<Ledger, LedgerModel>();
        }
    }
}
