namespace Financas.Models
{
    public class CategoriaOp
    {
        public CategoriaOp()
        {
            
        }
        public CategoriaOp(string nome, string? descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public int CategoriaOpId { get; private set; }
        public string Nome { get; private set; }
        public string? Descricao { get; private set; }
    }
}
