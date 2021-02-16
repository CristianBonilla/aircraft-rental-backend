using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Rental.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Rental.API
{
    public class Startup
    {
        public const string Bearer = nameof(Bearer);

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

            IConfigurationSection jwtSettingsSection = Configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(jwtSettingsSection);
            JwtSettings jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            services.AddSingleton(jwtSettings);

            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false; // Development only
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UsersPolicy", policy =>
                {
                    policy.AddRequirements(new RolPermissionsPolicyRequirement(Permissions.USERS));
                });
                options.AddPolicy("RolesPolicy", policy =>
                {
                    policy.AddRequirements(new RolPermissionsPolicyRequirement(Permissions.ROLES));
                });
                options.AddPolicy("PassengersPolicy", policy =>
                {
                    policy.AddRequirements(new RolPermissionsPolicyRequirement(Permissions.PASSENGERS));
                });
                options.AddPolicy("RentalsPolicy", policy =>
                {
                    policy.AddRequirements(new RolPermissionsPolicyRequirement(Permissions.RENTALS));
                });
                options.AddPolicy("AircraftsPolicy", policy =>
                {
                    policy.AddRequirements(new RolPermissionsPolicyRequirement(Permissions.AIRCRAFTS));
                });
            });
            services.AddSingleton<IAuthorizationHandler, RolPermissionsPolicyHandler>();

            MapperConfiguration mapperConfiguration = MapperStart.Start();  
            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            string connectionString = Configuration.GetConnectionString("AircraftRentalConnection");
            DataDirectoryConfig.SetDataDirectoryPath(ref connectionString);

            services.AddDbContextPool<RentalContext>(options =>
            {
                options.UseSqlServer(connectionString);
                // options.EnableSensitiveDataLogging();
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Aircraft Rental API",
                    Version = "v1",
                    Description = "Carry out aircraft rental processes safely"
                });
                options.AddSecurityDefinition(Bearer, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                OpenApiSecurityScheme apiSecurity = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = Bearer,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { apiSecurity, new List<string>() } });
            });
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
            // DbMigrationStart<RentalContext>(app);
        }

        private void JsonSerializer(MvcNewtonsoftJsonOptions options)
        {
            JsonSerializerSettings settings = options.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.None;
        }

        //private static void DbMigrationStart<TContext>(IApplicationBuilder app) where TContext : DbContext
        //{
        //    IServiceScopeFactory scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
        //    using IServiceScope serviceScope = scopeFactory.CreateScope();
        //    TContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TContext>();
        //    dbContext.Database.Migrate();

        //    // IRelationalDatabaseCreator databaseCreator = serviceScope.ServiceProvider.GetService<IRelationalDatabaseCreator>();
        //    // databaseCreator.Exists();
        //}
    }
}
