namespace PequiBank.Presentation.ExceptionHandlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PequiBank.Domain.Exceptions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public sealed class InvalidValueExceptionHandler : IExceptionHandler
{
    private const int UNPROCESSABLE_ENTITY = (int)HttpStatusCode.UnprocessableEntity;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidValueException invalidValue)
        {
            return false;
        }

        httpContext.Response.StatusCode = UNPROCESSABLE_ENTITY;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Type = nameof(InvalidValueException),
            Title = invalidValue.Message
        }, cancellationToken);

        return true;
    }
}