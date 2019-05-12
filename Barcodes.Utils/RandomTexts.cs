using System;
using System.Collections.Generic;

namespace Barcodes.Utils
{
    public static class RandomTexts
    {
        private static readonly Random random = new Random();
        private static readonly List<int> textsIndexes = new List<int>();

        private static readonly List<string> texts = new List<string>
        {
             "D. kradnie mleko!",
             "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
             "Jeśli historia mogłaby nas czegokolwiek nauczyć, to niewątpliwie tego, że żaden naród nie stworzył wyższej cywilizacji bez poszanowania prawa do posiadania własności prywatnej. ~Ludwig von Mises",
             "Pytasz się, a słyszysz",
             "Public static, public static, public static, public static, public static",
             "https://github.com/Jarczyslaw/",
             "Wszystkie nietrywialne zera funkcji dzeta mają część rzeczywistą równą jedna druga",
             "W dowolnie bliskim otoczeniu każdego stanu równowagi układu termodynamicznego istnieją stany nieosiągalne na drodze adiabatycznej",
        };

        static RandomTexts()
        {
            RefreshIndexesList();
        }

        public static string Get()
        {
            var index = textsIndexes[random.Next(textsIndexes.Count)];
            textsIndexes.Remove(index);
            if (textsIndexes.Count == 0)
                RefreshIndexesList(index);
            return texts[index];
        }

        private static void RefreshIndexesList(int exclude = -1)
        {
            textsIndexes.Clear();
            for (int i = 0; i < texts.Count; i++)
                textsIndexes.Add(i);
            if (exclude != -1)
                textsIndexes.Remove(exclude);
        }
    }
}