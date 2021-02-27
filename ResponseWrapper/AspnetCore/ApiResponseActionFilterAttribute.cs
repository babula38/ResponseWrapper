using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;

namespace ResponseWrapper.AspnetCore
{
    public class ApiResponseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var resultBuilder = context.HttpContext.RequestServices.GetRequiredService<ResultBuilder>();

            var result = context.Result as ObjectResult;

            if (result == null) return;

            var resultTypes = result.Value.GetType();

            object wrappedResult = resultBuilder.Build(result.Value, resultTypes);

            context.Result = new ObjectResult(wrappedResult);
        }


    }

    public class ResultBuilder
    {
        private static Delegate compiledExpression;

        public object Build(object value, Type resultTypes)
        {
            if (compiledExpression == null)
            {
                Type anonType = typeof(ApiResponse<>).MakeGenericType(resultTypes);

                var input = Expression.Parameter(resultTypes, "input");

                var exp = Expression.New(anonType.GetConstructor(new[] { resultTypes }),
                                         input);

                var lambda = Expression.Lambda(exp, input);

                compiledExpression = lambda.Compile();
            }

            object wrappedResult = compiledExpression!.DynamicInvoke(value);

            return wrappedResult;
        }
    }
}
