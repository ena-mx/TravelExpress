namespace TravelExpress.WebApplication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TravelExpress.Domain.Customers.Model;
    using TravelExpress.Domain.Customers.Shared;
    using TravelExpress.Domain.Orders;
    using TravelExpress.Domain.Orders.Component;
    using TravelExpress.Domain.Products;
    using TravelExpress.Domain.Sql.Customers.Model;
    using TravelExpress.Domain.Sql.Customers.Shared;
    using TravelExpress.Domain.Sql.Orders.Component;
    using TravelExpress.Domain.Sql.Orders.Shared;
    using TravelExpress.Domain.Sql.Products;
    using TravelExpress.Queries.Customers;
    using TravelExpress.Queries.Products;
    using TravelExpress.Queries.Sql.Customers;
    using TravelExpress.Queries.Sql.Products;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AllowAnonymousToFolder("/Account");
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<CustomersQueryComponent, SqlCustomersQueryComponent>();
            services.AddSingleton<CustomerComponentUowFactory, SqlCustomerComponentUowFactory>();
            services.AddScoped<OrderFactory, SqlOrderFactory>();
            services.AddScoped<OrderHistorization, SqlOrderHistorization>();
            services.AddScoped<OrderStorage, SqlOrderStorage>();

            services.AddSingleton<IProductComponentFactory, SqlProductComponentFactory>();
            services.AddScoped<ProductQueryComponent, SqlProductQueryComponent>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
