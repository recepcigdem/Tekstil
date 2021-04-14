using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AgeGroupManager>().As<IAgeGroupService>().SingleInstance();
            builder.RegisterType<EfAgeGroupDal>().As<IAgeGroupDal>().SingleInstance();

            builder.RegisterType<ArmManager>().As<IArmService>().SingleInstance();
            builder.RegisterType<EfArmDal>().As<IArmDal>().SingleInstance();

            builder.RegisterType<AuthorizationManager>().As<IAuthorizationService>().SingleInstance();
            builder.RegisterType<EfAuthorizationDal>().As<IAuthorizationDal>().SingleInstance();
            
            builder.RegisterType<BeltManager>().As<IBeltService>().SingleInstance();
            builder.RegisterType<EfBeltDal>().As<IBeltDal>().SingleInstance();
            
            builder.RegisterType<BrandManager>().As<IBrandService>().SingleInstance();
            builder.RegisterType<EfBrandDal>().As<IBrandDal>().SingleInstance();

            builder.RegisterType<ButtoningManager>().As<IButtoningService>().SingleInstance();
            builder.RegisterType<EfButtoningDal>().As<IButtoningDal>().SingleInstance();

            builder.RegisterType<BuyingMethodManager>().As<IBuyingMethodService>().SingleInstance();
            builder.RegisterType<EfBuyingMethodDal>().As<IBuyingMethodDal>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<ChannelManager>().As<IChannelService>().SingleInstance();
            builder.RegisterType<EfChannelDal>().As<IChannelDal>().SingleInstance();

            builder.RegisterType<CollectionManager>().As<ICollectionService>().SingleInstance();
            builder.RegisterType<EfCollectionDal>().As<ICollectionDal>().SingleInstance();

            builder.RegisterType<CountryManager>().As<ICountryService>().SingleInstance();
            builder.RegisterType<EfCountryDal>().As<ICountryDal>().SingleInstance();

            builder.RegisterType<CountryShippingMultiplierManager>().As<ICountryShippingMultiplierService>().SingleInstance();
            builder.RegisterType<EfCountryShippingMultiplierDal>().As<ICountryShippingMultiplierDal>().SingleInstance();

            builder.RegisterType<CsNoDeliveryDateHistoryManager>().As<ICsNoDeliveryDateHistoryService>().SingleInstance();
            builder.RegisterType<EfCsNoDeliveryDateHistoryDal>().As<ICsNoDeliveryDateHistoryDal>().SingleInstance();

            builder.RegisterType<CsNoDeliveryDateManager>().As<ICsNoDeliveryDateService>().SingleInstance();
            builder.RegisterType<EfCsNoDeliveryDateDal>().As<ICsNoDeliveryDateDal>().SingleInstance();

            builder.RegisterType<CustomerManager>().As<ICustomerService>().SingleInstance();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>().SingleInstance();

            builder.RegisterType<DepartmentManager>().As<IDepartmentService>().SingleInstance();
            builder.RegisterType<EfDepartmentDal>().As<IDepartmentDal>().SingleInstance();

            builder.RegisterType<DetailManager>().As<IDetailService>().SingleInstance();
            builder.RegisterType<EfDetailDal>().As<IDetailDal>().SingleInstance();

            builder.RegisterType<EmailManager>().As<IEmailService>().SingleInstance();
            builder.RegisterType<EfEmailDal>().As<IEmailDal>().SingleInstance();

            builder.RegisterType<FitManager>().As<IFitService>().SingleInstance();
            builder.RegisterType<EfFitDal>().As<IFitDal>().SingleInstance();

            builder.RegisterType<FoldedProductManager>().As<IFoldedProductService>().SingleInstance();
            builder.RegisterType<EfFoldedProductDal>().As<IFoldedProductDal>().SingleInstance();

            builder.RegisterType<FormManager>().As<IFormService>().SingleInstance();
            builder.RegisterType<EfFormDal>().As<IFormDal>().SingleInstance();

            builder.RegisterType<FurManager>().As<IFurService>().SingleInstance();
            builder.RegisterType<EfFurDal>().As<IFurDal>().SingleInstance();

            builder.RegisterType<GenderManager>().As<IGenderService>().SingleInstance();
            builder.RegisterType<EfGenderDal>().As<IGenderDal>().SingleInstance();

            builder.RegisterType<HandiworkManager>().As<IHandiworkService>().SingleInstance();
            builder.RegisterType<EfHandiworkDal>().As<IHandiworkDal>().SingleInstance();

            builder.RegisterType<HierarchyManager>().As<IHierarchyService>().SingleInstance();
            builder.RegisterType<EfHierarchyDal>().As<IHierarchyDal>().SingleInstance();

            builder.RegisterType<HoodManager>().As<IHoodService>().SingleInstance();
            builder.RegisterType<EfHoodDal>().As<IHoodDal>().SingleInstance();

            builder.RegisterType<ImportedLocalManager>().As<IImportedLocalService>().SingleInstance();
            builder.RegisterType<EfImportedLocalDal>().As<IImportedLocalDal>().SingleInstance();

            builder.RegisterType<LabelManager>().As<ILabelService>().SingleInstance();
            builder.RegisterType<EfLabelDal>().As<ILabelDal>().SingleInstance();

            builder.RegisterType<LinerManager>().As<ILinerService>().SingleInstance();
            builder.RegisterType<EfLinerDal>().As<ILinerDal>().SingleInstance();

            builder.RegisterType<ManufacturingHeadersManager>().As<IManufacturingHeadersService>().SingleInstance();
            builder.RegisterType<EfManufacturingHeadersDal>().As<IManufacturingHeadersDal>().SingleInstance();

            builder.RegisterType<MaterialContentManager>().As<IMaterialContentService>().SingleInstance();
            builder.RegisterType<EfMaterialContentDal>().As<IMaterialContentDal>().SingleInstance();

            builder.RegisterType<ModelSeasonRowNumberManager>().As<IModelSeasonRowNumberService>().SingleInstance();
            builder.RegisterType<EfModelSeasonRowNumberDal>().As<IModelSeasonRowNumberDal>().SingleInstance();

            builder.RegisterType<NeckManager>().As<INeckService>().SingleInstance();
            builder.RegisterType<EfNeckDal>().As<INeckDal>().SingleInstance();

            builder.RegisterType<OriginManager>().As<IOriginService>().SingleInstance();
            builder.RegisterType<EfOriginDal>().As<IOriginDal>().SingleInstance();

            builder.RegisterType<OutletSeasonManager>().As<IOutletSeasonService>().SingleInstance();
            builder.RegisterType<EfOutletSeasonDal>().As<IOutletSeasonDal>().SingleInstance();

            builder.RegisterType<PatternManager>().As<IPatternService>().SingleInstance();
            builder.RegisterType<EfPatternDal>().As<IPatternDal>().SingleInstance();

            builder.RegisterType<PaymentMethodManager>().As<IPaymentMethodService>().SingleInstance();
            builder.RegisterType<EfPaymentMethodDal>().As<IPaymentMethodDal>().SingleInstance();

            builder.RegisterType<PaymentMethodShareManager>().As<IPaymentMethodShareService>().SingleInstance();
            builder.RegisterType<EfPaymentMethodShareDal>().As<IPaymentMethodShareDal>().SingleInstance();

            builder.RegisterType<PhoneManager>().As<IPhoneService>().SingleInstance();
            builder.RegisterType<EfPhoneDal>().As<IPhoneDal>().SingleInstance();

            builder.RegisterType<ProductGroupManager>().As<IProductGroupService>().SingleInstance();
            builder.RegisterType<EfProductGroupDal>().As<IProductGroupDal>().SingleInstance();

            builder.RegisterType<SeasonCurrencyManager>().As<ISeasonCurrencyService>().SingleInstance();
            builder.RegisterType<EfSeasonCurrencyDal>().As<ISeasonCurrencyDal>().SingleInstance();

            builder.RegisterType<SeasonPlaningManager>().As<ISeasonPlaningService>().SingleInstance();
            builder.RegisterType<EfSeasonPlaningDal>().As<ISeasonPlaningDal>().SingleInstance();

            builder.RegisterType<SeasonManager>().As<ISeasonService>().SingleInstance();
            builder.RegisterType<EfSeasonDal>().As<ISeasonDal>().SingleInstance();

            builder.RegisterType<ShipmentMethodManager>().As<IShipmentMethodService>().SingleInstance();
            builder.RegisterType<EfShipmentMethodDal>().As<IShipmentMethodDal>().SingleInstance();

            builder.RegisterType<ShippingMethodManager>().As<IShippingMethodService>().SingleInstance();
            builder.RegisterType<EfShippingMethodDal>().As<IShippingMethodDal>().SingleInstance();

            builder.RegisterType<SizeManager>().As<ISizeService>().SingleInstance();
            builder.RegisterType<EfSizeDal>().As<ISizeDal>().SingleInstance();

            builder.RegisterType<StaffAuthorizationManager>().As<IStaffAuthorizationService>().SingleInstance();
            builder.RegisterType<EfStaffAuthorizationDal>().As<IStaffAuthorizationDal>().SingleInstance();

            builder.RegisterType<StaffEmailManager>().As<IStaffEmailService>().SingleInstance();
            builder.RegisterType<EfStaffEmailDal>().As<IStaffEmailDal>().SingleInstance();

            builder.RegisterType<StaffPhoneManager>().As<IStaffPhoneService>().SingleInstance();
            builder.RegisterType<EfStaffPhoneDal>().As<IStaffPhoneDal>().SingleInstance();

            builder.RegisterType<StaffManager>().As<IStaffService>().SingleInstance();
            builder.RegisterType<EfStaffDal>().As<IStaffDal>().SingleInstance();

            builder.RegisterType<StyleManager>().As<IStyleService>().SingleInstance();
            builder.RegisterType<EfStyleDal>().As<IStyleDal>().SingleInstance();

            builder.RegisterType<SubBrandManager>().As<ISubBrandService>().SingleInstance();
            builder.RegisterType<EfSubBrandDal>().As<ISubBrandDal>().SingleInstance();

            builder.RegisterType<SubDetailGroupManager>().As<ISubDetailGroupService>().SingleInstance();
            builder.RegisterType<EfSubDetailGroupDal>().As<ISubDetailGroupDal>().SingleInstance();

            builder.RegisterType<SubGroupManager>().As<ISubGroupService>().SingleInstance();
            builder.RegisterType<EfSubGroupDal>().As<ISubGroupDal>().SingleInstance();

            builder.RegisterType<SupplierMethodManager>().As<ISupplierMethodService>().SingleInstance();
            builder.RegisterType<EfSupplierMethodDal>().As<ISupplierMethodDal>().SingleInstance();

            builder.RegisterType<SupplierManager>().As<ISupplierService>().SingleInstance();
            builder.RegisterType<EfSupplierDal>().As<ISupplierDal>().SingleInstance();

            builder.RegisterType<TariffNoManager>().As<ITariffNoService>().SingleInstance();
            builder.RegisterType<EfTariffNoDal>().As<ITariffNoDal>().SingleInstance();

            builder.RegisterType<TestManager>().As<ITestService>().SingleInstance();
            builder.RegisterType<EfTestDal>().As<ITestDal>().SingleInstance();

            builder.RegisterType<WaistManager>().As<IWaistService>().SingleInstance();
            builder.RegisterType<EfWaistDal>().As<IWaistDal>().SingleInstance();

            builder.RegisterType<WidthManager>().As<IWidthService>().SingleInstance();
            builder.RegisterType<EfWidthDal>().As<IWidthDal>().SingleInstance();
            
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
