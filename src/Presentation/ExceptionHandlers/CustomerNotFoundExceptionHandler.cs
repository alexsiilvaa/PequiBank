namespace PequiBank.Presentation.ExceptionHandlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PequiBank.Domain.Exceptions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public sealed class CustomerNotFoundExceptionHandler : IExceptionHandler
{
    private const int NOT_FOUND = (int)HttpStatusCode.NotFound;

    private static readonly ProblemDetails problemDetails = new()
    {
        Type = nameof(CustomerNotFoundException),
        Title = "Customer not found"
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not CustomerNotFoundException)
        {
            return false;
        }

        httpContext.Response.StatusCode = NOT_FOUND;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}