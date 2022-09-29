using AspNetCore.Identity.FileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AuthentificationTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Just scaffolding code for an email sender
        /// </summary>
        class EmailSender : IEmailSender
        {
            public async Task SendEmailAsync(string email, string subject, string message)
            {
                var htmlContent = message;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IUserStore<IdentityUser>>(provider => new FsUserStore<IdentityUser>(@"c:\tmp\"));
            services.AddTransient<IRoleStore<FsRole>, FsRoleStore>();
            services.AddIdentity<IdentityUser, FsRole>
                    //set password restrictions
                    (
                        //Uncomment if you want to confirm the email
                        //options => options.SignIn.RequireConfirmedAccount = true
                        )
                .AddDefaultTokenProviders();

            var emailSender = new EmailSender();
            services.AddSingleton<IEmailSender, EmailSender>(provider => emailSender);

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }

     
}
