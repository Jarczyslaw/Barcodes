using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Utils
{
    public static class RandomTexts
    {
        private static Random random = new Random();
        private static List<string> texts = new List<string>
        {
            "Informatyka dla biznesu - Polsoft Engineering Sp. z o.o.",
             "Dorota kradnie mleko!",
             "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
             "Jeśli historia mogłaby nas czegokolwiek nauczyć, to niewątpliwie tego, że żaden naród nie stworzył wyższej cywilizacji bez poszanowania prawa do posiadania własności prywatnej. ~Ludwig von Mises",
             "Pytasz się, a słyszysz. ~D. Legęza",
             "Public static, public static, public static, public static, public static",
             "https://github.com/Jarczyslaw/",
             "Wszystkie nietrywialne zera funkcji dzeta mają część rzeczywistą równą jedna druga",
             "W dowolnie bliskim otoczeniu każdego stanu równowagi układu termodynamicznego istnieją stany nieosiągalne na drodze adiabatycznej",
        };

        public static string Get()
        {
            int next = random.Next(texts.Count);
            return texts[next];
        }
    }
}
