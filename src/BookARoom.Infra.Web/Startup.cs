using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
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
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var bus = new FakeBus();
            var bookingRepository = new BookingAndClientsRepository();

            var bookingHandler = CompositionRootHelper.BuildTheWriteModelHexagon(bookingRepository, bookingRepository, bus);

            // Register handlers (note: this should be packaged)
            bus.RegisterHandler<BookARoomCommand>(bookingHandler.Handle);

            // TODO: register handlers for the query part coming from the bus?

            services.AddSingleton<ISendCommands>(bus);

            var hotelsAdapter = new HotelAndRoomsAdapter(@"../../integration-files/", bus);
            var readFacade = new ReadModelFacade(hotelsAdapter, hotelsAdapter);

            services.AddSingleton<IQueryBookingProposals>(readFacade);
            services.AddSingleton<IProvideHotel>(readFacade);

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
