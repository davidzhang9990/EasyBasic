using System.Linq;
using Autofac;
using Autofac.Core;
using log4net;

namespace Common.Log
{
    public class LogInjectionModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs preparingEventArgs)
        {
            var limitType = preparingEventArgs.Component.Activator.LimitType;
            preparingEventArgs.Parameters = preparingEventArgs.Parameters.Union(new[]
                                                  {
                                                      new ResolvedParameter((p, i) => p.ParameterType == typeof (ILog),
                                                                            (p, i) => LogManager.GetLogger(limitType))
                                                  });
        }
    }
}
