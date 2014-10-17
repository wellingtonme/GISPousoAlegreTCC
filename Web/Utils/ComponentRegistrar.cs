using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Utils
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddApplicationServicesTo(container);

            //container.AddComponent("validator",
            //    typeof(IValidator), typeof(Validator));
        }

        private static void AddApplicationServicesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes.Pick()
                .FromAssemblyNamed("ApplicationService")
                .WithService.FirstInterface());

            container.Register(
                AllTypes.Pick()
                .FromAssemblyNamed("Data")
                .WithService.FirstInterface());

            container.Register(
                AllTypes.Pick()
                .FromAssemblyNamed("Common")
                .WithService.FirstInterface());
        }
    }
}