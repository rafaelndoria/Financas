using System.Data;

namespace Financas.Data
{
    public interface IDbConnectionProvider : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction CurrentTransaction { get; set; }
        IDbTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void SetTransaction(IDbTransaction transaction);
    }
}
