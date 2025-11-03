using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion
{
    internal static class Utils
    {
        /// <summary>
        /// Uppercases the first letter of each word in the 
        /// string and leaves the rest in lowercase
        /// </summary>
        /// <param name="str">the raw string</param>
        /// <returns>the capitalized string</returns>
        public static string CapitalizeString(string str)
        {
            string capitalized = "";
            var words = str.ToLower().Split(' ');
            foreach (var word in words)
            {
                capitalized += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
            }
            return capitalized.Trim();
        }

        /// <summary>
        /// From any text that may have letters with accents in Spanish, 
        /// get the same text but without punctuation
        /// </summary>
        /// <param name="str">Spanish text with accents</param>
        /// <returns>Text without accents</returns>
        public static string RemoveSpanishAccentsString(string str)
        {
            return new String(
                str.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray()
            )
            .Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// translates the string value of SwimStroke
        /// from Spanish to English so we can save them in DB
        /// </summary>
        public static string SwimStrokeTranslatorEspToEng(string swimStrokeESP)
        {
            string swimStrokeENG = "";
            switch (swimStrokeESP.ToLower())
            {
                case "braza": swimStrokeENG = "BREAST";
                    break;
                case "espalda":
                    swimStrokeENG = "BACK";
                    break;
                case "mariposa":
                    swimStrokeENG = "FLY";
                    break;
                case "libres":
                    swimStrokeENG = "FREE";
                    break;
                case "estilos":
                    swimStrokeENG = "MEDLEY";
                    break;

                default: swimStrokeENG = "NO INFO";
                    break;
            }
            return swimStrokeENG;
        }
    }
}
