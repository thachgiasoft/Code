using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using log4net;
using FX.Core;
using EIS.Core;
using FX.Context;
namespace EIS.FEW
{
    public class Bootstrapper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));
        private static IWindsorContainer container;
        public static void InitializeContainer()
        {
            try
            {
                // Initialize Windsor
                container = new WindsorContainer(new XmlInterpreter());
                IoC.Initialize(container);

                // Add ICuyahogaContext to the container.
                container.Register(Component.For<FX.Context.IFXContext>()
                    .ImplementedBy<EISContext>()
                    .Named("FX.context")
                    .LifeStyle.PerWebRequest
                );
            }
            catch (Exception ex)
            {
                log.Error("Error initializing application.", ex);
                throw;
            }
        }
    }
}