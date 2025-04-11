using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebHotelRezervation.Models;

namespace WebHotelRezervation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var value = builder.Services.AddMvc().AddRazorRuntimeCompilation();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("MyDbConst")!;
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            builder.Services.AddDbContext<HotelDbContext>(options=>options.UseMySql(connectionString,serverVersion));

            var app = builder.Build();


            // Configure the HTTP request pipeline. bwaaaaaaaaaaaaaaaa
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
