using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
            context.Response.OnStarting(state => AddProcessingTimeHeader(context, timer), context);

            // Before the headers are sent back tot he client the OnStarting method gets called
            await next(context);
        }

        private Task AddProcessingTimeHeader(object state, Stopwatch timer)
        {
            var httpContext = (HttpContext)state;

            var currentHeader = httpContext.Response.Headers.FirstOrDefault(h => h.Key == "X-ProcessingTime");
            if (currentHeader.Key == null)
            {
                httpContext.Response.Headers["X-ProcessingTime"] = timer.ElapsedMilliseconds.ToString() + " ms";
            }
            else
            {
                httpContext.Response.Headers.Add("X-ProcessingTime", new[] { timer.ElapsedMilliseconds.ToString() + " ms" });
            }
            

            return Task.FromResult(0);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ProcessingTimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseProcessingTimeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProcessingTimeMiddleware>();
        }
    }
}
