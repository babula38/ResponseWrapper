using ResponseWrapper.AspnetCore;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExceptionWrappperMiddlewareExtension
    {
        public static IApplicationBuilder UseErrorWrapper(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionWrappperMiddleware>();

            return builder;
        }
    }
}
