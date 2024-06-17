namespace DiscountManager.App
{
    using DiscountManager.Data.Context;
    using DiscountManager.Data.Repositories;
    using DiscountManager.Models.Options;
    using DiscountManager.Services;
    using DiscountManagerController.Grpc;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfigurationSection mongoOptopnsSection = builder.Configuration.GetSection(MongoDbOptions.SectionName);
            builder.Services.AddOptions<MongoDbOptions>().Bind(mongoOptopnsSection);

            IConfigurationSection serviceSettingsOptopnsSection = builder.Configuration.GetSection(ServiceSettings.SectionName);
            builder.Services.AddOptions<ServiceSettings>().Bind(serviceSettingsOptopnsSection);

            builder.Services.AddGrpc(opt =>
            {
                opt.EnableDetailedErrors = true;
            });

            // Register services
            builder.Services.AddSingleton<MongoDbContext>();
            builder.Services.AddTransient<IDiscountCodeRepository, DiscountCodeRepository>();
            builder.Services.AddTransient<IDiscountCodeGeneratorService, DiscountCodeGeneratorService>();

            var app = builder.Build();

            app.MapGrpcService<DiscountManagerControllerService>();

            app.MapGet("/", () => "Hello from 'DiscountManager' service!");

            app.Run();
        }
    }
}
