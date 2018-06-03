using AutoMapper;
using eDraw.api.Core;
using eDraw.api.Core.Models;
using eDraw.api.Core.Models.AppSettings;
using eDraw.api.Persistance;
using eDraw.api.ServiceClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace eDraw.api
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
            services.Configure<PhotoAppSettings>(Configuration.GetSection("PhotoSettings"));
            services.Configure<AwsAppSettings>(Configuration.GetSection("Aws"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddDbContext<EDrawDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAwsMailClient, AwsMailClient>();
            services.AddScoped<IJobBudgetRepository, JobBudgetRepository>();
            services.AddScoped<IAwsServiceClient, AwsServiceClient>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IDashBoardRepository, DashBoardRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IEmailServiceClient, EmailServiceClient>();
            services.AddScoped<IBankReportRepository, BankReportRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJobBudgetRepository, JobBudgetRepository>();
            services.AddScoped<IJobCategoriesRepository, JobCategoriesRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();

            services.AddAutoMapper();

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 4;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<EDrawDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<EDrawDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
     .AddAuthentication(options =>
     {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

     })
     .AddJwtBearer(cfg =>
     {
         cfg.RequireHttpsMetadata = false;
         cfg.SaveToken = true;
         cfg.TokenValidationParameters = new TokenValidationParameters
         {
             ValidIssuer = Configuration["JwtIssuer"],
             ValidAudience = Configuration["JwtIssuer"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
             ClockSkew = TimeSpan.Zero // remove delay of token when expire
         };

     });
            services.AddCors(options =>
            {
                options.AddPolicy("Example",
            builder => builder.WithOrigins("http://edrawapi.thriftmatch.com")
                                .AllowAnyHeader()
                                .AllowAnyMethod());
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());
            });
            services.AddMvc().AddJsonOptions(option =>
            {
                option.SerializerSettings.ContractResolver =
                    new DefaultContractResolver();

                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }); ;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            app.UseAuthentication();
            loggerFactory.AddLog4Net();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
            });
            DbInitializer.SeedRoles(serviceProvider).Wait();
        }

    }
}