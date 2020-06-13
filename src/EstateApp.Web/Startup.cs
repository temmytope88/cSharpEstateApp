using System;
using System.Threading.Tasks;
using Estate.Data.Entities;
using Estateapp.Data.DBContext.ApplicationDBContext;
using Estateapp.Data.DBContext.AuthenticationDBContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EstateApp.Web
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
      services.AddDbContextPool<AuthenticationDBContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("AuthenticationConnection"),
       
       sqlServerOptions => { 
         sqlServerOptions.MigrationsAssembly("Estateapp.Data"); 
         }
      ));
      services.AddDbContextPool<ApplicationDBContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationConnection"),
      
      sqlServerOptions => { sqlServerOptions.MigrationsAssembly("Estateapp.Data"); }
      ));
     services.AddIdentity<ApplicationUser, IdentityRole>()
     .AddEntityFrameworkStores<AuthenticationDBContext>()
     .AddDefaultTokenProviders();

     services.Configure<IdentityOptions>(options =>
     {
       options.Password.RequireDigit = false;
       options.Password.RequiredLength = 6;
       options.Password.RequireUppercase = false;
       options.Password.RequireLowercase = false;
       options.Password.RequireNonAlphanumeric = false;
       options.SignIn.RequireConfirmedEmail = false;
       options.SignIn.RequireConfirmedPhoneNumber = false;
     }
       
     );
      services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
      });

      MigrateDatabasecontexts(svp);
      CreateDefaultRolesAndUsers(svp).GetAwaiter().GetResult();
    }

    public void MigrateDatabasecontexts(IServiceProvider svp)
    {
      var authenticationDBContext = svp.GetRequiredService<AuthenticationDBContext >();
      authenticationDBContext.Database.Migrate();

      var applicationDBContext = svp.GetRequiredService<ApplicationDBContext>();
      applicationDBContext.Database.Migrate();
    }
    public async Task CreateDefaultRolesAndUsers(IServiceProvider svp)
    {
      string[] roles = new string[] {"SystemAdministrator", "Agent", "User"};
      var userEmail = "admin@estateapp.com";
      var userPassword = "supersecretpassword@2020";
      var roleManager = svp.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in roles)
        
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
              await roleManager.CreateAsync(new IdentityRole{ Name = role });
            }
        }
      var userManager = svp.GetRequiredService<UserManager<ApplicationUser>>();
      var user = await userManager.FindByEmailAsync(userEmail);
      if (user is null)
      {
        user = new ApplicationUser
        {
          Email= userEmail,
          UserName = userEmail,
          EmailConfirmed = true,
          PhoneNumber= "+2348050846834",
          PhoneNumberConfirmed = true
        };
       
        await userManager.CreateAsync(user, userPassword);
        await userManager.AddToRolesAsync(user, roles);
      }
       

    }

  }
}
