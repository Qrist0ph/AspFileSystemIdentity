# AspFileSystemIdentity
A plain and simple JSON File storage for ASP.NET Core Identity

# Features
ASP.NET Core Identity by default makes use of the Entity Framework and a database for storing Identity data. When using this library all user data is stored in plain json file.

# Usage
**First** modify method **ConfigureServices** in your **Startup.cs** like this
Replace this
```
  // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
```

by this
                
```

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
    ...
```

**Second** you will have generate the Identity scaffolding like described [here](https://stackoverflow.com/questions/50802781/where-are-the-login-and-register-pages-in-an-aspnet-core-scaffolded-app)

**Third** you need to uncomment the code in the generated file **IdentityHostingStartup.cs**
