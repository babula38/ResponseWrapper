using Microsoft.AspNetCore.Http;
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
            if (context.Exception != null)
                //context.ExceptionHandled = true;
            return;

            if (context.HttpContext.Response.StatusCode != StatusCodes.Status200OK)
                return;

            var resultBuilder = context.HttpContext.RequestServices.GetRequiredService<ResultBuilder>();

            var result = context.Result as ObjectResult;

            if (result == null) return;

            var resultTypes = result.Value.GetType();

            object wrappedResult = resultBuilder.Build(result.Value, resultTypes, StatusCodes.Status200OK);

            context.Result = new ObjectResult(wrappedResult);
        }


    }

    public class ResultBuilder
    {
        private static Delegate compiledExpression;

        public object Build(object value, Type resultTypes, int status)
        {
            if (compiledExpression == null)
            {
                Type anonType = typeof(ApiResponse<>).MakeGenericType(resultTypes);

                var input = Expression.Parameter(resultTypes, "input");
                var inputStatus = Expression.Parameter(typeof(OperationStatus), "status");

                var exp = Expression.New(anonType.GetConstructor(new[] { resultTypes, typeof(OperationStatus) }),
                                         input, inputStatus);
                //var exp = typeof(ApiResponse<>).GetMethod("Success", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                //var methodCallExpression = Expression.Call(exp, input);


                var lambda = Expression.Lambda(exp, input, inputStatus);
                //var lambda = Expression.Lambda(methodCallExpression, input);

                compiledExpression = lambda.Compile();
            }

            var operationStatus = OperationStatus.Success;

            if (status == 200) operationStatus = OperationStatus.Success;

            object wrappedResult = compiledExpression!.DynamicInvoke(value, operationStatus);

            return wrappedResult;
        }
    }
}
