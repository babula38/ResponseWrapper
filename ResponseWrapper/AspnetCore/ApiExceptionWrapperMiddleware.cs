using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseWrapper.AspnetCore
{
    public class ApiExceptionWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionWrapperMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                IError? error = ex as IError;

                if (error != null)
                {
                    var problem = error.GetProblemDetails(ex);

                    context.Response.Clear();
                    context.Response.StatusCode = problem.Code;

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(problem);
                }
            }
        }

    }
}
