using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using System.Net;

namespace UMB.Reminder.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost
                //.ConfigureAppConfiguration((c,b)=>b.AddJsonFile(""))
                .CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    // Run callbacks on the transport thread
                    options.ApplicationSchedulingMode = SchedulingMode.ThreadPool;

                    options.Listen(IPAddress.Any, 5900, listenOptions =>
                    {
                        // Uncomment the following to enable Nagle's algorithm for this endpoint.
                        //listenOptions.NoDelay = false;

                        //listenOptions.UseConnectionLogging();
                    });                    
                })                
                .UseStartup<Startup>()
                .Build();
    }
}
