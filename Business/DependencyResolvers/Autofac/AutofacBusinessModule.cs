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

            builder.RegisterType<AuthorizationManager>().As<IAuthorizationService>().SingleInstance();
            builder.RegisterType<EfAuthorizationDal>().As<IAuthorizationDal>().SingleInstance();

            builder.RegisterType<ChannelManager>().As<IChannelService>().SingleInstance();
            builder.RegisterType<EfChannelDal>().As<IChannelDal>().SingleInstance();

            builder.RegisterType<CountryShippingMultiplierManager>().As<ICountryShippingMultiplierService>().SingleInstance();
            builder.RegisterType<EfCountryShippingMultiplierDal>().As<ICountryShippingMultiplierDal>().SingleInstance();

            builder.RegisterType<CsNoDeliveryDateHistoryManager>().As<ICsNoDeliveryDateHistoryService>().SingleInstance();
            builder.RegisterType<EfCsNoDeliveryDateHistoryDal>().As<ICsNoDeliveryDateHistoryDal>().SingleInstance();

            builder.RegisterType<CsNoDeliveryDateManager>().As<ICsNoDeliveryDateService>().SingleInstance();
            builder.RegisterType<EfCsNoDeliveryDateDal>().As<ICsNoDeliveryDateDal>().SingleInstance();

            builder.RegisterType<CustomerManager>().As<ICustomerService>().SingleInstance();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>().SingleInstance();

            builder.RegisterType<CurrentTypeManager>().As<ICurrentTypeService>().SingleInstance();
            builder.RegisterType<EfCurrentTypeDal>().As<ICurrentTypeDal>().SingleInstance();

            builder.RegisterType<DepartmentManager>().As<IDepartmentService>().SingleInstance();
            builder.RegisterType<EfDepartmentDal>().As<IDepartmentDal>().SingleInstance();

            builder.RegisterType<DefinitionManager>().As<IDefinitionService>().SingleInstance();
            //builder.RegisterType<DefinitionManager>().As<IDefinitionService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<EfDefinitionDal>().As<IDefinitionDal>().SingleInstance();

            builder.RegisterType<DefinitionTitleManager>().As<IDefinitionTitleService>().SingleInstance();
            builder.RegisterType<EfDefinitionTitleDal>().As<IDefinitionTitleDal>().SingleInstance();

            builder.RegisterType<EmailManager>().As<IEmailService>().SingleInstance();
            builder.RegisterType<EfEmailDal>().As<IEmailDal>().SingleInstance();

            //builder.RegisterModule(new AutofacBusinessModule());

            builder.RegisterType<HierarchyManager>().As<IHierarchyService>().SingleInstance();
            //builder.RegisterType<HierarchyManager>().As<IHierarchyService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<EfHierarchyDal>().As<IHierarchyDal>().SingleInstance();

            builder.RegisterType<LabelManager>().As<ILabelService>().SingleInstance();
            builder.RegisterType<EfLabelDal>().As<ILabelDal>().SingleInstance();

            builder.RegisterType<ModelSeasonRowNumberManager>().As<IModelSeasonRowNumberService>().SingleInstance();
            builder.RegisterType<EfModelSeasonRowNumberDal>().As<IModelSeasonRowNumberDal>().SingleInstance();

            builder.RegisterType<PaymentMethodShareManager>().As<IPaymentMethodShareService>().SingleInstance();
            builder.RegisterType<EfPaymentMethodShareDal>().As<IPaymentMethodShareDal>().SingleInstance();

            builder.RegisterType<PhoneManager>().As<IPhoneService>().SingleInstance();
            builder.RegisterType<EfPhoneDal>().As<IPhoneDal>().SingleInstance();

            builder.RegisterType<SeasonCurrencyManager>().As<ISeasonCurrencyService>().SingleInstance();
            builder.RegisterType<EfSeasonCurrencyDal>().As<ISeasonCurrencyDal>().SingleInstance();

            builder.RegisterType<SeasonPlaningManager>().As<ISeasonPlaningService>().SingleInstance();
            builder.RegisterType<EfSeasonPlaningDal>().As<ISeasonPlaningDal>().SingleInstance();

            builder.RegisterType<SeasonManager>().As<ISeasonService>().SingleInstance();
            builder.RegisterType<EfSeasonDal>().As<ISeasonDal>().SingleInstance();

            builder.RegisterType<StaffAuthorizationManager>().As<IStaffAuthorizationService>().SingleInstance();
            builder.RegisterType<EfStaffAuthorizationDal>().As<IStaffAuthorizationDal>().SingleInstance();

            builder.RegisterType<StaffEmailManager>().As<IStaffEmailService>().SingleInstance();
            builder.RegisterType<EfStaffEmailDal>().As<IStaffEmailDal>().SingleInstance();

            builder.RegisterType<StaffPhoneManager>().As<IStaffPhoneService>().SingleInstance();
            builder.RegisterType<EfStaffPhoneDal>().As<IStaffPhoneDal>().SingleInstance();

            builder.RegisterType<StaffManager>().As<IStaffService>().SingleInstance();
            builder.RegisterType<EfStaffDal>().As<IStaffDal>().SingleInstance();

            builder.RegisterType<TariffNoManager>().As<ITariffNoService>().SingleInstance();
            builder.RegisterType<EfTariffNoDal>().As<ITariffNoDal>().SingleInstance();

            builder.RegisterType<TariffNoDetailManager>().As<ITariffNoDetailService>().SingleInstance();
            builder.RegisterType<EfTariffNoDetailDal>().As<ITariffNoDetailDal>().SingleInstance();

            builder.RegisterType<TestManager>().As<ITestService>().SingleInstance();
            builder.RegisterType<EfTestDal>().As<ITestDal>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
