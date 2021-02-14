using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rental.Domain;
using Rental.Infrastructure;

namespace Rental.API
{
    public class AutofacProvider
    {
        public static AutofacServiceProvider Provider(IServiceCollection services)
        {
            IContainer container = Container(services);
            AutofacServiceProvider serviceProvider = new AutofacServiceProvider(container);

            return serviceProvider;
        }

        private static IContainer Container(IServiceCollection services)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterGeneric(typeof(RepositoryContext<>))
                .As(typeof(IRepositoryContext<>));
            containerBuilder.RegisterGeneric(typeof(Repository<,>))
                .As(typeof(IRepository<,>));
            containerBuilder.RegisterType<AuthService>()
                .As<IAuthService>();
            containerBuilder.RegisterType<AircraftService>()
                .As<IAircraftService>();
            containerBuilder.RegisterType<RentalService>()
                .As<IRentalService>();

            IContainer container = containerBuilder.Build();

            return container;
        }
    }
}
