using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sanayii.APIs.Extensions;
using Sanayii.APIs.Services;
using Sanayii.Core.Entities;
using Sanayii.Services;
using Sanayii.Repository.Data;
using System.Threading.Tasks;
using Sanayii.Core.Interfaces;
using Sanayii.Service.Chat;
using Sanayii.Core.DTOs.ChatDTOs;
using System.Text.Json.Nodes;
using Sanayii.Service.Hubs;
using Stripe;
using Sanayii.Repository;
using Sanayii.Core;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
namespace Sanayii
{
    public class Program
    {
        // Entry Point
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Configure Services
            // Add services to the container.
            builder.Services.AddSignalR();
            builder.Services.AddControllers(); // Apply API Configurations
            //builder.Services.AddRazorPages(); // Add if using Razor Pages

            //logging
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            // Configure Swagger
            builder.Services.AddSwaggerServices();

            // Database Connection
            builder.Services.AddDbContext<SanayiiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity Configuration
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<SanayiiContext>()
            .AddDefaultTokenProviders();

            // Identity Configuration
            builder.Services.AddIdentityServices(builder.Configuration);

            // Add Application Services
            builder.Services.AddApplicationServices();

            //Registeration for Api chat 
            // Register HttpClient with retry policy for resilience
            builder.Services.AddHttpClient<IChatService, ChatService>()
                .AddPolicyHandler(GetRetryPolicy());



            // Retry policy configuration
            static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    .WaitAndRetryAsync(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            }
            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200") // URL Angular app
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Allow cookies, headers, etc.
                    });
            });

            // Add SignalR services to the DI container
            builder.Services.AddSignalR();

            // Stripe Configuration
            var stripeSettings = builder.Configuration.GetSection("Stripe");
            StripeConfiguration.ApiKey = stripeSettings["SecretKey"];

            #endregion


            var app = builder.Build();


            #region Apply Migrations
            //using var scope = app.Services.CreateScope();

            //var services = scope.ServiceProvider;

            //var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //try
            //{
            //    var dbContext = services.GetRequiredService<SanayiiContext>(); // Ask Explicitly
            //    await dbContext.Database.MigrateAsync(); // Apply Migrations [Update Database]

            //}
            //catch (Exception ex)
            //{
            //    var logger = loggerFactory.CreateLogger<Program>();
            //    logger.LogError(ex, ex.Message);
            //}
            #endregion


            // Configure Middleware

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Add this line before endpoints!
            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            // Now map hubs and controllers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });

            app.Run();

        }
    }
}
