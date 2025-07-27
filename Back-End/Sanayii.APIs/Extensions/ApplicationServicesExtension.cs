using Microsoft.AspNetCore.Identity;
using Sanayii.APIs.Services;
using Sanayii.Core;
using Sanayii.Core.Entities;
using Sanayii.Core.Interfaces;
using Sanayii.Core.Repositories;
using Sanayii.Repository;
using Sanayii.Repository.Repository;
using Sanayii.Service.Chat;
using Sanayii.Service.Mappers;
using Sanayii.Services;

namespace Sanayii.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            // Email Sender Service
            services.AddTransient<EmailSenderService>();

            services.AddScoped<IChatService, ChatService>();

            // SMS Sender Service
            services.AddTransient<SMSSenderService>();

            // Register the chat service
            services.AddScoped<IChatService, ChatService>();

            services.AddScoped<CommentService>();

            // Apply AutoMapper
            services.AddAutoMapper(typeof(CustomerMapper));

            // Apply Generic Repository Dependency Injection
            services.AddScoped<UnitOFWork>();

            return services;
        }
    }
}
