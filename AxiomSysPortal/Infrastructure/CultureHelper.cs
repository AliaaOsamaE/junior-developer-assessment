using System.Globalization;

namespace AxiomSysPortal.Infrastructure;

public static class CultureHelper
{
    public const string English = "en";
    public const string Arabic = "ar";

    public static bool IsArabic
    {
        get { return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == Arabic; }
    }

    public static string CurrentCulture
    {
        get { return IsArabic ? Arabic : English; }
    }

    public static List<CultureInfo> GetSupportedCultures()
    {
        var english = new CultureInfo(English);

        var arabic = new CultureInfo(Arabic);

        foreach (var calendar in arabic.OptionalCalendars)
        {
            if (calendar is GregorianCalendar)
            {
                arabic.DateTimeFormat.Calendar = calendar;
                break;
            }
        }

        return new List<CultureInfo> { english, arabic };
    }
}
