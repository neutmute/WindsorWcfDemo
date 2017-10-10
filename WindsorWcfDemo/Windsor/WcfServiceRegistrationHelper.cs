using Castle.Facilities.WcfIntegration;
using Castle.Facilities.WcfIntegration.Behaviors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.ServiceModel;
using WindsorWcfDemo.Windsor;

namespace Phoenix.Core.Infrastructure.Windsor
{
    public static class WcfServiceRegistrationHelper
    {
        public static IWcfServiceModel RegisterWcfService(this IWindsorContainer container, WcfTypePair wcfServiceTypePair)
         {
            var serviceModel = GetServiceModel(container, wcfServiceTypePair);


            var componentReg = Component
                .For(wcfServiceTypePair.Interface)
                .ImplementedBy(wcfServiceTypePair.Implementation)
                .Named(wcfServiceTypePair.Implementation.Name)
                //.Interceptors<LoggingInterceptor>()
                //.Interceptors<ExceptionHandlerInterceptor>()
                .LifeStyle.Transient
                .OnDestroy(service =>
                {
                    var channel = service as IServiceChannel;

                    if (channel?.State == CommunicationState.Faulted)
                    {
                        channel.Abort();
                    }
                })
                .AsWcfService(serviceModel);

            container.Register(componentReg);
  
            return serviceModel;
        }
               
        private static DefaultServiceModel GetServiceModel(IWindsorContainer container, WcfTypePair wcfServiceTypePair)
        {
            var baseAddress = $"https://{Environment.MachineName}:808/services/";
            var endpointAddress = baseAddress + wcfServiceTypePair.Implementation.Name;
            var address = new EndpointAddress(endpointAddress);
            
            var wsHttpBinding = BindingHelper.GetWsHttpBinding();
            var endpoint = WcfEndpoint.BoundTo(wsHttpBinding).At(address);

            var model = new DefaultServiceModel()
                            .OnFaulted(h => HandleProxy(container, h))
                            .AddEndpoints(endpoint);
            
            if (true)
            {
                string mexEndpointAddress = endpointAddress + "/mex/";
                model.AddExtensions(new WcfMetadataExtension().AtAddress(mexEndpointAddress));
            }

            //model.PublishMetadata(o => o.EnableHttpGet());
            
          //  model.AddExtensions(container.GetServiceCredentials());
   
            return model;
        }

        //private static ServiceCredentials GetServiceCredentials(this IWindsorContainer container)
        //{
        //    var serviceCredentials = new ServiceCredentials();
        //    var settings = container.Resolve<IConfigSetting>();
        //    var log = container.Resolve<ILogWriter>();

        //    // Configure service certificate
        //    serviceCredentials.ServiceCertificate.SetCertificate(
        //        StoreLocation.LocalMachine,
        //        StoreName.My,
        //        X509FindType.FindBySubjectName,
        //        settings.CertificateFriendlyName);

        //    // Add custom certificate validator
        //    serviceCredentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
        //    serviceCredentials.ClientCertificate.Authentication.CustomCertificateValidator = new PhoenixX509CertificateValidator(log, settings);
     
        //    return serviceCredentials;
        //}

        private static void HandleProxy(IWindsorContainer container, ICommunicationObject proxy)
        {
            if (proxy != null)
            {
                try
                {
                    if (proxy.State != CommunicationState.Faulted)
                    {
                        try
                        {
                            proxy.Close();
                        }
                        catch (CommunicationException e)
                        {
                            proxy.Abort();

                        }
                    }
                    else
                    {
                        proxy.Abort();
                    }
                }
                catch (Exception ex)
                {
                    proxy.Abort();
                }
                finally
                {
                    proxy = null;
                }
            }
        }
    }
}