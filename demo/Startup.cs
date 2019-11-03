using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using demo.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace demo
{
    public class Startup
    {

        public string ContentRootPath { get; set; }
        void log(string s)
        {
            try
            {
                //Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (string.IsNullOrWhiteSpace(s)) return;
                var fn = $@"{ContentRootPath}\logs\log.txt";
                string v = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {s}\r\n";
                File.AppendAllText(fn, v);
            }
            catch (Exception)
            {
                //throw;
            }
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //https://www.webwiz.net/clientarea/support-view-ticket.htm?TicketID=F94-43761-8619444444-B96F


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2 );
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@".\logs\"));

            // register our custom middleware since we use the IMiddleware factory approach
            services.AddTransient<WebSocketMiddleware>();

            // register the background process to periodically send a timestamp to clients
            services.AddHostedService<BroadcastTimestamp>();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // enable websocket support
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            });

            // add our custom middleware to the pipeline
            app.UseMiddleware<WebSocketMiddleware>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //        app.UseHsts();
        //    }

        //    app.UseHttpsRedirection();
        //    //app.UseMvc();
        //    app.UseStaticFiles();


        //    //WS
        //    ContentRootPath = env.ContentRootPath; //save path to wwwroot
        //    log("start v2");
        //    log($"contentRoot: {ContentRootPath}");

        //    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-2.2

        //    //app.UseWebSockets();//ws
        //    var webSocketOptions = new WebSocketOptions()
        //    {
        //        KeepAliveInterval = TimeSpan.FromSeconds(120),
        //        ReceiveBufferSize = 4 * 1024
        //    };

        //    app.UseWebSockets(webSocketOptions);

        //    //Accept WebSocket requests
        //    app.Use(async (context, next) =>
        //    {
        //        try
        //        {
        //            log("---" + context.Request.Path);
        //            if (context.Request.Path == "/ws")
        //            {
        //                log("WS " + context.Request.Path);
        //                if (context.WebSockets.IsWebSocketRequest)
        //                {
        //                    log("ok IsWebSocketRequest");
        //                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        //                    log("ok we have websocket");
        //                    await Echo(context, webSocket);
        //                }
        //                else
        //                {
        //                    log("NOT IsWebSocketRequest");
        //                    context.Response.StatusCode = 400;
        //                }
        //            }
        //            else
        //            {
        //                await next();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log(ex.Message);
        //            log(ex.StackTrace);
        //            if (ex.InnerException != null)
        //            {
        //                log("INNER: " + ex.InnerException.Message);
        //                log("INNER: " + ex.InnerException.StackTrace);
        //                if (ex.InnerException.InnerException != null)
        //                {
        //                    log("INNER2: " + ex.InnerException.InnerException.Message);
        //                    log("INNER2: " + ex.InnerException.InnerException.StackTrace);
        //                }
        //            }

        //        }

        //    });


        //}



        //private async Task Echo(HttpContext context, WebSocket webSocket)
        //{
        //    var buffer = new byte[1024 * 4];
        //    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //    while (!result.CloseStatus.HasValue)
        //    {
        //        await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

        //        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //    }
        //    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        //}


    }
}

