using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Circa.Actors.Application;
using Circa.Actors.Domain;
using Circa.Actors.Domain.Operations;
using Circa.Actors.Infra;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Solari.Callisto;
using Solari.Callisto.Tracer;
using Solari.Deimos;
using Solari.Sol;
using Solari.Titan.DependencyInjection;
using Solari.Vanth.DependencyInjection;

namespace Circa.Actors.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // IConveyBuilder builder = ConveyBuilder.Create(services);
            services.AddControllers();
            // builder.AddRabbitMq()
            //        .AddCommandHandlers()
            //        .AddQueryHandlers()
            //        .AddEventHandlers()
            //        .AddServiceBusCommandDispatcher()
            //        .AddServiceBusEventDispatcher();

            services.AddScoped<PersonOperations>();
            services.AddScoped<IPersonApplication, PersonApplication>();
            services.AddSol(Configuration)
                    .AddTitan()
                    // .AddDeimos(plugin => plugin.Register(new CallistoTracerPlugin()))
                    .AddVanth()
                    .AddCallisto(a => a.RegisterDefaultClassMaps()
                                       .RegisterDefaultConventionPack()
                                       .RegisterCollection<IPersonRepository, PersonRepository, Person>("person", ServiceLifetime.Scoped));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseConvey();
            app.UseSol();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}