namespace DiscountManager.App
{
    using DiscountManager.Data.Context;
    using DiscountManager.Models.Options;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfigurationSection mongoOptopnsSection = builder.Configuration.GetSection(MongoDbOptions.SectionName);
            builder.Services.AddOptions<MongoDbOptions>().Bind(mongoOptopnsSection);

            // Register services
            builder.Services.AddSingleton<MongoDbContext>();

            var app = builder.Build();

            app.MapGet("/", () => "Hello from 'DiscountManager' service!");

            app.Run();
        }
    }
}
