using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Facilities.TypedFactory;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Phoenix.Core.Infrastructure.Windsor;
using System.Text;
using WindsorWcfDemo.Services;

namespace WindsorWcfDemo
{
    public class WcfDemoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // It is important the WcfFacility is registered before the services
            container.AddFacility<WcfFacility>();

            var serviceModels = GetWcfServceTypes()
                                .Select(container.RegisterWcfService)
                                .ToList();
        }
        
        internal static List<WcfTypePair> GetWcfServceTypes()
        {
            var pairs = new List<WcfTypePair> {
                WcfTypePair.Factory<IMyService, MyService>()
            };
            return pairs;
        }
        
    }


}