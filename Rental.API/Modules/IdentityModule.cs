using Autofac;

namespace Rental.API
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityService>()
                .As<IIdentityService>();
        }
    }
}
