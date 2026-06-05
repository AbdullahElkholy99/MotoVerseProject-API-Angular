using MotoVerse.Entities.Models;
using System.Globalization;

namespace MotoVerse.Data.Commans;

public class GeneralLocalizableEntity : BaseEntity
{
    public string Localize(string textAr, string textEn)
    {
        CultureInfo culture = Thread.CurrentThread.CurrentCulture;

        if (culture == null) return "";

        var name = (culture.TwoLetterISOLanguageName.ToLower().Equals("ar")) ?
                 textAr : textEn;

        return name ?? "";
    }
}
