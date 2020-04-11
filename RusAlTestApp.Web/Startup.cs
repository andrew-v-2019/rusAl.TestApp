using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RusAlTestApp.Data;
using RusAlTestApp.Services;

namespace RusAlTestApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            services.AddRazorPages()
                 .AddRazorRuntimeCompilation();

            services.AddControllersWithViews();
            services.AddTransient<IRegistrationsService, RegistrationsService>();
        }

        public async Task CreateUserAndRole(IServiceProvider services)
        {
            const string adminUserName = "admin";
            const string adminPassword = "admin";
            const string adminRoleName = "admin";

            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                var defaultUser = await CreateDefaultUser(scope, adminUserName, adminPassword);
                var defaultRole = await CreateDefaultRole(scope, adminRoleName);
                await AddUserToRole(scope, defaultRole, defaultUser);
            }
        }

        private static async Task<IdentityUser> CreateDefaultUser(IServiceScope serviceScope, string adminUserName,
            string adminPassword)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var defaultUser = await userManager.FindByNameAsync(adminUserName);

            if (defaultUser != null)
                return defaultUser;

            var user = new IdentityUser { UserName = adminUserName, Email = adminUserName };
            var identityResult = (await userManager.CreateAsync(user, adminPassword));
            defaultUser = await userManager.FindByNameAsync(adminUserName);

            return defaultUser;
        }

        private static async Task<IdentityRole> CreateDefaultRole(IServiceScope serviceScope, string adminRoleName)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var defaultRole = await roleManager.FindByNameAsync(adminRoleName);

            if (defaultRole != null)
                return defaultRole;
            var role = new IdentityRole
            {
                Name = adminRoleName,
                NormalizedName = adminRoleName
            };
            await roleManager.CreateAsync(role);
            defaultRole = await roleManager.FindByNameAsync(adminRoleName);
            return defaultRole;
        }

        private static async Task<IdentityRole> AddUserToRole(IServiceScope serviceScope, IdentityRole role, IdentityUser user)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            await userManager.AddToRoleAsync(user, role.Name);
            return role;
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CreateUserAndRole(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
