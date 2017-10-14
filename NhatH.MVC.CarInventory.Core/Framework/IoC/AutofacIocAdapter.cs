using Autofac;
using Autofac.Integration.Mvc;
using NhatH.MVC.CarInventory.Core.Service.Contract;
using NhatH.MVC.CarInventory.Core.Service.Impl;
using NhatH.MVC.CarInventory.DB;
using NhatH.MVC.CarInventory.DB.Miscellaneous;
using NhatH.MVC.CarInventory.DB.UoW;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace NhatH.MVC.CarInventory.Core.Framework.IoC
{
    public class AutofacIocAdapter
    {
        public static AutofacIocAdapter Instance = new AutofacIocAdapter();
        private ILifetimeScope _container;
        private AutofacIocAdapter()
        {
            this._container = IocResolver();
        }
        public IDependencyResolver GetResolver()
        {
            if (this._container == null)
            {
                this._container = IocResolver();
            }

            return new AutofacDependencyResolver(this._container);
        }
        public T GetService<T>()
        {
            return (T)this.GetResolver().GetService(typeof(T));
        }
        public object GetService(string type)
        {
            return this.GetResolver().GetService(Type.GetType(type));
        }
        public object GetService(Type type)
        {
            return this.GetResolver().GetService(type);
        }
        private static ILifetimeScope IocResolver()
        {
            var container = new ContainerBuilder();

            // Register HTTP abstraction here.
            // for more: https://code.google.com/p/autofac/wiki/MvcIntegration
            container.RegisterModule(new AutofacWebTypesModule());

            // Register controllers.

            // Read from assemblies.
            // find the delivered class from Icontroller, resolve ICachemanger by PerRequest.
            container.RegisterControllers(AppDomain.CurrentDomain.GetAssemblies());

            // Register local services.
            RegisterServices(container);

            return container.Build();
        }
        private static void RegisterJobs(ContainerBuilder container)
        {

        }
        private static void RegisterServices(ContainerBuilder container)
        {
            container.RegisterModule(new AutofacWebTypesModule());

            // Register services here.
            container.Register(
                c => new CarInventoryDBContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                .AsSelf()
                .InstancePerLifetimeScope();
            container.RegisterType<RepositoryFactories>().As<IRepositoryFactories>().InstancePerLifetimeScope();
            container.RegisterType<RepositoryProvider>().As<IRepositoryProvider>().InstancePerLifetimeScope();
            container.RegisterType<CarInventoryUoW>().As<ICarInventoryUoW>().InstancePerLifetimeScope();
            container.RegisterType<MemberService>().As<IMemberService>().InstancePerRequest();
            container.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerRequest();
            container.RegisterType<AuthorizationService>().As<IAuthorizationService>().InstancePerRequest();
            container.RegisterType<WebWorkContext>().As<IWebWorkContext>().InstancePerRequest();
            container.RegisterType<AppWorkContext>().As<IAppWorkContext>().InstancePerLifetimeScope();
            container.RegisterType<CarService>().As<ICarService>().InstancePerRequest();
            container.Register(
                (c, t) =>
                {
                    if (HttpContext.Current != null)
                    {
                        return (IWorkContext)c.Resolve<IWebWorkContext>();
                    }

                    return (IWorkContext)c.Resolve<IAppWorkContext>();
                }).As<IWorkContext>().InstancePerLifetimeScope();


            RegisterJobs(container);
        }
    }
}
