namespace PequiBank.Presentation.ExceptionHandlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PequiBank.Domain.Exceptions;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public sealed class InvalidFinancialTransactionTypeExceptionHandler : IExceptionHandler
{
    private const int UNPROCESSABLE_ENTITY = (int)HttpStatusCode.UnprocessableEntity;

    private static readonly ProblemDetails problemDetails = new()
    {
        Type = nameof(InvalidFinancialTransactionTypeException),
        Title = "Invalid financial transaction type"
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidFinancialTransactionTypeException)
        {
            return false;
        }

        httpContext.Response.StatusCode = UNPROCESSABLE_ENTITY;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}