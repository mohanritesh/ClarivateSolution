using ClarivateApp.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ClarivateApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        // Set the JSON serializer options
            //        options.JsonSerializerOptions.PropertyNamingPolicy = null; // To preserve the original property names
            //        options.JsonSerializerOptions.IgnoreNullValues = true; // Ignore null values when serializing
            //        options.JsonSerializerOptions.WriteIndented = true; // Write JSON with indentation for better readability
            //        options.JsonSerializerOptions.MaxDepth = 64; // Set maximum serialization depth to avoid potential stack overflow exceptions
            //    });

            builder.Services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(BasicAuthenticationDefaults.AuthenticationScheme,
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = BasicAuthenticationDefaults.AuthenticationScheme,
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Basic authorization Header"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = BasicAuthenticationDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { "Basic "}
                    }
                });
            });
            builder.Services.Configure<BasicAuthenticationOptions>(builder.Configuration.GetSection("BasicAuthentication"));
            builder.Services.AddAuthentication()
         .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, null);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policyBuilder =>
                    {
                        policyBuilder
                        .AllowAnyOrigin() // Add your frontend URL here
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                    });
            });

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}