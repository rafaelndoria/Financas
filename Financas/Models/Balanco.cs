namespace Financas.Models
{
    public class Balanco
    {
        public Balanco()
        {
            
        }
        public Balanco(int mes, int ano, int usuarioId, Usuario usuario, double? despesa = 0, double? receita = 0)
        {
            Mes = mes;
            Ano = ano;
            UsuarioId = usuarioId;
            Usuario = usuario;
            Link = (Mes + Ano + UsuarioId).ToString();
            Despesa = despesa;
            Receita = receita;
            AtualizarTotalBalanco();
        }

        public int BalancoId { get; private set; }
        public int Mes { get; private set; }
        public int Ano { get; private set; }
        public string Link { get; private set; }
        public double? Despesa { get; private set; }
        public double? Receita { get; private set; }
        public double Total { get; private set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public void AdicionarReceita(double valor)
        {
            Receita += valor;
            AtualizarTotalBalanco();
        }

        public void AdicionarDespesa(double valor)
        {
            Despesa += valor;
            AtualizarTotalBalanco();
        }

        private void AtualizarTotalBalanco()
        {
            Total = (double)(Receita - Despesa);
        }
    }
}
