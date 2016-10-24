using Autofac;

namespace Common.Ioc
{
    public interface IRegister
    {
        void Register(ContainerBuilder containerBuilder);
    }
}
