using _20210928.Areas.Identity.Data;
using _20210928.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _20210928
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
            services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddDbContext<StoreContext>(options => options.UseSqlite(
                Configuration.GetConnectionString("StoreContextConnection")
               ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            IServiceProvider serviceProvider)
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
                endpoints.MapRazorPages();
            });

            AddRolesAndUsers(serviceProvider);
        }

        private async void AddRolesAndUsers(IServiceProvider serviceProvider)
        {
            UserManager<StoreUser> userManager = 
                serviceProvider.GetRequiredService<UserManager<StoreUser>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Staff" };

            foreach(var role in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string generatedAdminUserName = "generated_admin@example.com";
            string generatedAdminPassword = "testPassword1234!";

            if (await userManager.FindByEmailAsync(generatedAdminUserName) == null)
            {
                var generatedAdmin = new StoreUser { 
                    UserName = generatedAdminUserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(
                    generatedAdmin, 
                    generatedAdminPassword);
                foreach (var role in roles)
                {
                    await userManager.AddToRoleAsync(generatedAdmin, role);
                }
            }
        }
    }
}
