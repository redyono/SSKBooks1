using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Services;
using SSKBooks1.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure the database context
        builder.Services.AddDbContext<SSKBooksDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
            ));

        // Configure Identity with custom options
        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SSKBooksDbContext>();

        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IBookService, BookService>();

        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Home/AccessDenied";
        });
      


        var app = builder.Build();

        // Seed admin user
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await DbInitializer.SeedAdminUserAsync(services);
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error500");
            app.UseStatusCodePagesWithReExecute("/Home/Error{0}");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Enable Identity UI Razor Pages
        app.MapRazorPages();

        await app.RunAsync();
    }
}
