using Dapper;
using Financas.Data;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;

namespace Financas.Repositories
{
    public class PagamentoFaturaRepository : IPagamentoFaturaRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public PagamentoFaturaRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Create(PagamentoFaturaViewModel fatura)
        {
            SQL = "";
            SQL = "INSERT INTO PagamentoFatura (FaturaId, ValorPago) VALUES (@FaturaId,@ValorPago)";

            try
            {
                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, fatura);
                }
                else
                {
                    _connection.Connection.Execute(SQL, fatura, _connection.CurrentTransaction);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
