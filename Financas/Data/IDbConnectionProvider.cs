using System.Data;

namespace Financas.Data
{
    public interface IDbConnectionProvider : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }
}
