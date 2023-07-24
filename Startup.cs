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
using MyFirstAzureWebApp.common;
using SendGrid.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
                // DEV & UAT
                /*
                // DEV
                //Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", Configuration["dev-uat-database-saleonmob"]);
                //Environment.SetEnvironmentVariable("SAP_INTERFACE_PASSWORD", Configuration["dev-saleonmobile-sappwd"]);
                //UAT
                //Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", Configuration["uat-dbcon-saleonmob"]);
                //Environment.SetEnvironmentVariable("SAP_INTERFACE_PASSWORD", Configuration["uat-saleonmobile-sappwd"]);

                //Environment.SetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING", Configuration["dev-uat-storage-saleonmob"]);
                //Environment.SetEnvironmentVariable("AZURE_SENDGRID_CONNECTION_STRING", Configuration["dev-uat-sendgrid-saleonmob"]);
                //Environment.SetEnvironmentVariable("SAP_INTERFACE_REQ_KEY", Configuration["dev-uat-saleonmobile-sap-reqkey"]);
                */
                // DEV & UAT

                //PROD
                
                Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", Configuration["prd-saleonmobile-condb"]);
                Environment.SetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING", Configuration["prd-saleonmobile-storage"]);
                Environment.SetEnvironmentVariable("AZURE_SENDGRID_CONNECTION_STRING", Configuration["prd-saleonmobile-sendgrid"]);
                Environment.SetEnvironmentVariable("SAP_INTERFACE_PASSWORD", Configuration["prd-saleonmobile-sappwd"]);
                Environment.SetEnvironmentVariable("SAP_INTERFACE_REQ_KEY", Configuration["prd-saleonmobile-sap-reqkey"]);
                
                //PROD
            }
            else
            {
                Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", Configuration["DatabaseConnectionString"]);
                Environment.SetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING", Configuration["BlobConnectionString"]);
                Environment.SetEnvironmentVariable("AZURE_SENDGRID_CONNECTION_STRING", Configuration["SendgridConnectionString"]);
                Environment.SetEnvironmentVariable("SAP_INTERFACE_PASSWORD", Configuration["SapInterfacePassword"]);
                Environment.SetEnvironmentVariable("SAP_INTERFACE_REQ_KEY", Configuration["SapInterfacePassReqKey"]);
            }

            services.AddControllers();

            // AddSwaggerGen
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyFirstAzureWebApp", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "MyFirstAzureWebApp.xml");
                c.IncludeXmlComments(filePath, includeControllerXmlComments: true);
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
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[CommonConstant.COOKIE_TOKEN_KEY];
                        return Task.CompletedTask;
                    },
                };
            });
            services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(tokenKey));
            // Add sendGrid
            Console.WriteLine("SendgridConnectionString:" + Environment.GetEnvironmentVariable("AZURE_SENDGRID_CONNECTION_STRING"));
            services.AddSendGrid(options =>
            {
                options.ApiKey = Environment.GetEnvironmentVariable("AZURE_SENDGRID_CONNECTION_STRING");
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(CommonConstant.ORIGINS)
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  .AllowCredentials();
                        builder.WithExposedHeaders("Content-Disposition");
                    });

            });
            /*
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => {
                    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    options.WithExposedHeaders("Content-Disposition");
                });
            });
            */

            // session

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
            });
            services.AddMvc();
            /*
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();*/

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

            app.UseCors(options => options.WithOrigins(CommonConstant.ORIGINS).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // global cors policy
            /*app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials*/

            // Session
            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

            // Add MVC to the request pipeline.
            /*app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

 
        }
    }
}
