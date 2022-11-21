using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Redis
            var redisUrl = builder.Configuration["CacheOptions:Url"];
            builder.Services.AddSingleton<RedisService>(serviceProvider =>
            {
                return new RedisService(redisUrl);
            });

            // IDatabase
            builder.Services.AddSingleton<IDatabase>(serviceProvider =>
            {
                var redisService = serviceProvider.GetRequiredService<RedisService>();
                return redisService.GetDatabase();
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}