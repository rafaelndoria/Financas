namespace Financas.Models
{
    public class TipoOp
    {
        public TipoOp(string nome)
        {
            Nome = nome;
        }

        public int TipoOpId { get; private set; }
        public string Nome { get; private set; }
    }
}
