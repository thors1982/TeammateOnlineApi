using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace TeammateOnlineApi.Middleware
{
    public class ThrowExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ThrowExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Query.ContainsKey("throw"))
            {
                switch(context.Request.Query["throw"].ToString().ToLower())
                {
                    case "unauthorized":
                    case "401":
                        context.Response.StatusCode = 401; //Unauthorized
                        return;
                    case "500":
                        context.Response.StatusCode = 500;
                        return;
                    case "exception":
                    default:
                        throw new Exception("Exception thrown due to throw query paramater");
                }                
            }

            await next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ThrowExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseThrowExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThrowExceptionMiddleware>();
        }
    }
}
