using Autofac;
using Rental.Domain;
using Rental.Infrastructure;

namespace Rental.API
{
    public class RentalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(RepositoryContext<>))
                .As(typeof(IRepositoryContext<>));
            builder.RegisterGeneric(typeof(Repository<,>))
                .As(typeof(IRepository<,>));
            builder.RegisterType<AuthService>()
                .As<IAuthService>();
            builder.RegisterType<AircraftService>()
                .As<IAircraftService>();
            builder.RegisterType<RentalService>()
                .As<IRentalService>();
        }
    }
}
