using Dapper;
using Financas.Data;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;

namespace Financas.Repositories
{
    public class TituloRepository : ITituloRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public TituloRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Create(TituloViewModel model)
        {
            try
            {
                SQL = "";
                SQL = @"INSERT INTO Titulo (Descricao,Observacao,Parcela,NumeroParcelas,Valor,TipoTitulo,StatusTitulo,DataTitulo,ContaId)
                        VALUES (@Descricao,@Observacao,@Parcela,@NumeroParcelas,@Valor,@TipoTitulo,@StatusTitulo,@DataTitulo,@ContaId)";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, model);

                    return true;
                }
                else
                {
                    _connection.Connection.Execute(SQL, model, _connection.CurrentTransaction);

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
