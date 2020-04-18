using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using RecipesBookBll.Exceptions;

namespace RecipesBookWebApi.Filters
{
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            int statusCode = 500;

            string message = null;

            var exceptionType = context.Exception.GetType();
        
            (message, statusCode) = context.Exception switch
            {
                EntityException _ when context.Exception is EntityException => ($"Entity exception: {context.Exception.Message}", 400),
                EntityDoesNotExistException _ when context.Exception is EntityDoesNotExistException => ($"Entity exception: {context.Exception.Message}", 404),  
                _ => ($"Unexpected exception with message: {context.Exception.Message}", 500)
            };
            
            context.Result = new ContentResult
            {
                Content = message
            };
            context.HttpContext.Response.StatusCode = statusCode;
            context.ExceptionHandled = true;
        }
    }
}