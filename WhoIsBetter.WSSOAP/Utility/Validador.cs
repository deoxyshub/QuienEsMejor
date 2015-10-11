using System.Text.RegularExpressions;

namespace WhoIsBetter.WSSOAP
{
    public class Validador
    {
        public static bool EsCorreo(string correo)
        {
            var regex = new Regex(@".+\@.+\..+");
            return regex.Match(correo).Success;
        }

        public static bool ValidaNumero(string texto)
        {
            var regex = new Regex(@"^[1-9]\\d*$");
            return regex.Match(texto).Success;
        }

        public static bool EsPotenciaDe2(int x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }
    }
}
