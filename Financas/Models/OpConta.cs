namespace Financas.Models
{
    public class OpConta
    {
        public OpConta(string descricao, double valor, DateTime dataOp, int contaId, Conta conta, int categoriaOpId, CategoriaOp categoriaOp, int tipoOpId, TipoOp tipoOp)
        {
            Descricao = descricao;
            Valor = valor;
            DataOp = dataOp;
            ContaId = contaId;
            Conta = conta;
            CategoriaOpId = categoriaOpId;
            CategoriaOp = categoriaOp;
            TipoOpId = tipoOpId;
            TipoOp = tipoOp;
        }

        public int OpContaId { get; private set; }
        public string Descricao { get; private set; }
        public double Valor { get; private set; }
        public DateTime DataOp { get; private set; }
        public int ContaId { get; private set; }
        public Conta Conta { get; private set; }
        public int CategoriaOpId { get; private set; }
        public CategoriaOp CategoriaOp { get; private set; }
        public int TipoOpId { get; private set; }
        public TipoOp TipoOp { get; private set; }
    }
}
