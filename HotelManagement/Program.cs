using HotelManagement.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add Authentication using cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Access/Login";  // Path to login page
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);  // Session expiry
                    options.SlidingExpiration = true;  // Optional: Makes the session expire after a period of inactivity
                });

            // Add DBContext for HotelDbContext with connection string
            builder.Services.AddDbContext<HotelDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"))
            );

            // Build the app
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();  // Serve static files (CSS, JS, etc.)

            app.UseRouting();  // Enable routing

            app.UseAuthentication();  // Add authentication middleware
            app.UseAuthorization();   // Add authorization middleware

            // Configure default controller route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Access}/{action=Login}/{id?}");

            // Run the app
            app.Run();
        }
    }
}
