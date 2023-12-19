using System.Text.RegularExpressions;

namespace Financas.ObjectValues
{
    public class Email
    {
        public Email(string enderecoEmail)
        {
            VerifyIsValid(enderecoEmail);
            EnderecoEmail = enderecoEmail;
        }

        public string EnderecoEmail { get; private set; }

        private void VerifyIsValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (email == null)
                throw new Exception("O e-mail não pode ser nulo");
            if (email.Length < 4)
                throw new Exception("Informe um e-mail maior");
            if (!Regex.IsMatch(email, pattern))
                throw new Exception("Informe um e-mail valido");
        }
    }
}
