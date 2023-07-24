using Azure.Storage.Blobs;
using Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyFirstAzureWebApp.Authentication;
using MyFirstAzureWebApp.Business;
using MyFirstAzureWebApp.Business.People;
using System;
using System.Text;

namespace MyFirstAzureWebApp
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
            // Setup SetEnvironmentVariable
            var env = Configuration["ASPNETCORE_ENVIRONMENT"]??"Production";
            if (env != "Development")
            {
                Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", Configuration["dev-uat-database-saleonmob"]);
                Environment.SetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING", Configuration["dev-uat-storage-saleonmob"]);
                Environment.SetEnvironmentVariable("AZURE_SENDGRID_CONNECTION_STRING", Configuration["dev-uat-sendgrid-saleonmob"]);
            }
            else
            {
                Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", Configuration["DatabaseConnectionString"]);
                Environment.SetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING", Configuration["BlobConnectionString"]);
                Environment.SetEnvironmentVariable("AZURE_SENDGRID_CONNECTION_STRING", Configuration["SendgridConnectionString"]);
            }

            services.AddControllers();

            // AddSwaggerGen
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyFirstAzureWebApp", Version = "v1" });
            });

            // Setup Database connection string
            //var DatabaseConnectionString = Configuration.GetConnectionString("DATABASE_CONNECTION_STRING");
            var DatabaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            services.AddDbContext<MyAppContext>(options =>
                options.UseSqlServer(DatabaseConnectionString)
                );

            // Azue BlobService
            // see https://www.learmoreseekmore.com/2021/02/dotnet5-web-api-managing-files-using-azure-blob-storage.html
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            services.AddScoped(_ =>
            {
                return new BlobServiceClient(connectionString);
            });

            //Init interface
            services.AddScoped<IFileManagerLogic, FileManagerLogic>();
            services.AddScoped<IEmployees, Employees>();

            // JWT Authentication
            var tokenKey = Configuration.GetValue<string>("TokenKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(tokenKey));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Make sure UseSwagger only in dev
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyFirstAzureWebApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
