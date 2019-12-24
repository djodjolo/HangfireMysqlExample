using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.Extensions.Configuration;

namespace WebApplication3
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
            services.AddHangfire(x => x.UseStorage(new MySqlStorage("server = 127.0.0.1; uid = root; pwd =; database =hangfire; Allow User Variables = True", new MySqlStorageOptions() { TablePrefix = "hangfire" })));

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireServer();//启动Hangfire服务
            app.UseHangfireDashboard();//启动hangfire面板
            RecurringJob.AddOrUpdate("testRecurringJob", () => Console.WriteLine("Simple!"), "1 * * * * ");
            var jobId = BackgroundJob.Enqueue(
    () => Console.WriteLine("Fire-and-forget!"));
            var jobIds = BackgroundJob.Schedule(
    () => Console.WriteLine("Delayed!"),
    TimeSpan.FromDays(7));
            BackgroundJob.ContinueWith(
    jobId,
    () => Console.WriteLine("Continuation!"));
            RecurringJob.AddOrUpdate(
    () => Console.WriteLine("Recurring!"),
    Cron.Minutely);

        }
    }
}




