using DataAccess.DbAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ApiDemo
{
    public static class StartupExtentions
    {
        public static IServiceCollection RegisterServicesToDI(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<ICosmosDataAccess, CosmosDataAccess>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MediatREntryPoint).Assembly));
            services.AddRouting();
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }
    }
}
