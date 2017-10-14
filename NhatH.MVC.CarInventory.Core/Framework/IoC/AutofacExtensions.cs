using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Framework.IoC
{
    public static class AutofacExtensions
    {
        public static object ResolveUnregistered(this AutofacDependencyResolver autofacDependencyResolver, Type type)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = autofacDependencyResolver.GetService(parameter.ParameterType);
                        if (service == null) throw new NullReferenceException("Unkown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {

                }
            }
            throw new NullReferenceException("No contructor was found that had all the dependencies satisfied.");
        }
    }
}
