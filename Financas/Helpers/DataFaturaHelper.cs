namespace Financas.Helpers
{
    public static class DataFaturaHelper
    {
        public static DateTime DataFatura(DateTime dataOperacao, int diaVencimento)
        {
            var dia = dataOperacao.Day;
            if (dia <= diaVencimento)
            {
                return dataOperacao;
            }
            else
            {
                return dataOperacao.AddMonths(1);
            }
        }
    }
}
