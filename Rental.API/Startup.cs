using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rental.Domain;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;

namespace Rental.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(JsonSerializer);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rental.API", Version = "v1" });
            });

            IConfigurationSection jwtSettingsSection = Configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(jwtSettingsSection);
            JwtSettings jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            services.AddSingleton(jwtSettings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            MapperConfiguration mapperConfiguration = MapperStart.Start();
            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            string connectionString = Configuration.GetConnectionString("AircraftRentalConnection");
            DataDirectoryConfig.SetDataDirectoryPath(ref connectionString);

            services.AddDbContextPool<RentalContext>(options => options.UseSqlServer(connectionString));
        }

        // Register your own things directly with Autofac here.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<IdentityModule>();
            builder.RegisterModule<RentalModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental.API v1"));
            }

            app.UseRouting();

            // Global cors policy
            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            MigrationStart<RentalContext>(app);
        }

        private static void MigrationStart<TContext>(IApplicationBuilder app) where TContext : DbContext
        {
            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            using IServiceScope serviceScope = scopeFactory.CreateScope();
            TContext context = serviceScope.ServiceProvider.GetRequiredService<TContext>();
            context.Database.Migrate();

            // IRelationalDatabaseCreator databaseCreator = serviceScope.ServiceProvider.GetService<IRelationalDatabaseCreator>();
            // databaseCreator.Exists();
        }

        private void JsonSerializer(MvcNewtonsoftJsonOptions options)
        {
            JsonSerializerSettings settings = options.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.None;
        }
    }
}
