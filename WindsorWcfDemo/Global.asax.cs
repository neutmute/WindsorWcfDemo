using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WindsorWcfDemo
{
    public class Global : System.Web.HttpApplication
    {
        IWindsorContainer _container;

        protected void Application_Start(object sender, EventArgs e)
        {
            var iocInstallers = new IWindsorInstaller[] { new WcfDemoInstaller() };
            _container = new WindsorContainer();
            _container.Install(iocInstallers);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            _container.Dispose();
        }
    }
}