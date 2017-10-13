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
                .AsWcfService(serviceModel);

            container.Register(componentReg);
  
            return serviceModel;
        }
               
        private static DefaultServiceModel GetServiceModel(IWindsorContainer container, WcfTypePair wcfServiceTypePair)
        {
            var baseAddress = $"https://localhost:443/Services/";
            var endpointAddress = baseAddress + wcfServiceTypePair.Implementation.Name;
            var address = new EndpointAddress(endpointAddress);
            
            var wsHttpBinding = BindingHelper.GetWsHttpBinding();
            var endpoint = WcfEndpoint.BoundTo(wsHttpBinding).At(address);

            var model = new DefaultServiceModel()
                            .AddEndpoints(endpoint);
            
            if (false)
            {
                var mexEndpointAddress = endpointAddress + "/mex/";
                model.AddExtensions(new WcfMetadataExtension().AtAddress(mexEndpointAddress));
            }
               
            return model;
        }
    }
}