using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TeammateOnlineApi.Middleware
{
    public class ProcessingTimeMiddleware
    {
        private readonly RequestDelegate next;

        public ProcessingTimeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var timer = Stopwatch.StartNew();

            // Add Processing time header AFTER everything else is finished
            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("X-ProcessingTime", new[] { timer.ElapsedMilliseconds.ToString() + " ms" });

                return Task.FromResult(0);
            }, context);

            // Before the headers are sent back tot he client the OnStarting method gets called
            await next(context);
        }
    }
}
