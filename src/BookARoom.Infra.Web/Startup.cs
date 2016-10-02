using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra.MessageBus;
using BookARoom.Infra.ReadModel.Adapters;
using BookARoom.Infra.WriteModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookARoom.Infra.Web
{
    public class Startup
    {
        private IHostingEnvironment env;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            this.env = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Build the WRITE side (we use a bus for it) ----------
            var bus = new FakeBus();
            var bookingRepository = new BookingAndClientsRepository();

            var bookingHandler = CompositionRootHelper.BuildTheWriteModelHexagon(bookingRepository, bookingRepository, bus, bus);

            // TODO: register handlers for the query part coming from the bus?

            // Build the READ side ---------------------------------
            var hotelsAdapter = new HotelsAndRoomsAdapter($"{env.WebRootPath}/hotels/", bus);
            hotelsAdapter.LoadAllHotelsFiles();
            
            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter, null, bus);

            // Registers all services to the MVC IoC framework
            services.AddSingleton<ISendCommands>(bus);
            services.AddSingleton<IQueryBookingOptions>(readFacade);
            services.AddSingleton<IProvideHotel>(readFacade);
            services.AddSingleton<IProvideReservations>(readFacade);

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
