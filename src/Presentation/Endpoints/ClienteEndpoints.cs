namespace PequiBank.Presentation.Endpoints;

using PequiBank.Application.Features.CreateFinancialTransaction;
using PequiBank.Application.Features.GetBankStatement;
using PequiBank.Domain.Exceptions;

public static class ClienteEndpoints
{
    public static WebApplication MapClienteEndpoints(this WebApplication app)
    {
        var builder = app.MapGroup("/clientes");
        builder.MapPost("/{id}/transacoes", CreateFinancialTransactionHandler);
        builder.MapGet("/{id}/extrato", GetFinancialTransactionsHandler);

        return app;
    }

    private static async Task<IResult> CreateFinancialTransactionHandler
        (
            int id,
            CriarTransacao body,
            ICreateFinancialTransaction createFinancialTransaction,
            CancellationToken cancellationToken
        )
    {
        if (int.TryParse(body.Valor.ToString(), out var value) is false)
        {
            throw new InvalidValueException("Value is not valid.");
        }

        var request = new RequestCreateFinancialTransaction(id, value, body.Tipo, body.Descricao);
        var response = await createFinancialTransaction.ExecuteAsync(request, cancellationToken);
        return TypedResults.Ok<RespostaCriarTransacao>(new(response.CreditLimit, response.Balance));
    }

    private static async ValueTask<IResult> GetFinancialTransactionsHandler
        (
            int id,
            IGetBankStatement getBankStatement,
            CancellationToken cancellationToken
        )
    {
        var response = await getBankStatement.ExecuteAsync(id, cancellationToken);

        return TypedResults.Ok<RespostaTransacoesFinanceira>(new
        (
            Saldo: new(response.Customer.Balance, DateTime.UtcNow, response.Customer.CreditLimit),
            Ultimas_transacoes: response.FinancialTransactions.ConvertAll(ft => new TransacaoFinanceira(ft.Value, ft.Type, ft.Description, ft.ExecutedOn.DateTime))
        ));
    }

    public record CriarTransacao(object Valor, string Tipo, string Descricao);
    public record RespostaCriarTransacao(int Limite, int Saldo);
    public record SaldoTransacoesFinanceira(int Total, DateTime Data_extrato, int Limite);
    public record TransacaoFinanceira(int Valor, char Tipo, string Descricao, DateTime Realizada_em);
    public record RespostaTransacoesFinanceira(SaldoTransacoesFinanceira Saldo, List<TransacaoFinanceira> Ultimas_transacoes);
}