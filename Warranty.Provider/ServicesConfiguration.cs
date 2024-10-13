using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Provider.IProvider;
using Warranty.Provider.Mapping;
using Warranty.Provider.Provider;
using Warranty.Repository;

namespace Warranty.Provider
{
    public static class ServicesConfiguration
    {
        public static void AddProviderServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddRepositoryService(configuration);
            services.AddSingleton(mapper);
            services.AddTransient<ICommonProvider, CommonProvider>();
            services.AddTransient<IEngineerProvider, EngineerProvider>();
            services.AddTransient<IBreakdownStatusMasterProvider, BreakdownStatusMasterProvider>();
            services.AddTransient<IModelMasterProvider, ModelMasterProvider>();
            services.AddTransient<IUserMasterProvider, UserMasterProvider>();
            services.AddTransient<IDashBoardProvider, DashBoardProvider>();
            services.AddTransient<ICustomerProvider, CustomerProvider>();
            services.AddTransient<IContractTypeMasterProvider, ContractTypeMasterProvider>();
            services.AddTransient<IStateMastProvider, StateMastProvider>();
            services.AddTransient<IDistrictMastProvider,DistrictMastProvider>();
            services.AddTransient<IWarrantyListProvider,WarrantyListProvider>();
            services.AddTransient<IDue_ExpiredWarrantyProvider,Due_ExpiredWarrantyProvider>();         
            services.AddTransient<IContractListProvider,ContractListProvider>();
            services.AddTransient<IBreakDownListProvider,BreakDownListProvider>();
            services.AddTransient<IDue_ExpiredWarrantyProvider, Due_ExpiredWarrantyProvider>();                  
            services.AddTransient<IImportProvider, ImportProvider>();
            services.AddTransient<IAMC_CMCExpiredContractProvider, AMC_CMCExpiredContractProvider>();
            services.AddTransient<IAllocationProvider, AllocationProvider>();
            services.AddTransient<IProductMasterProvider, ProductMasterProvider>();
            services.AddTransient<ISupplierMasterProvider, SupplierMasterProvider>();
            services.AddTransient<IInwardOutwardProvider, InwardOutwardProvider>();
            services.AddTransient<ILedgerProvider, LedgerProvider>();
        }
    }
}
