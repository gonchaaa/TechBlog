using System.Text.RegularExpressions;

namespace Web.Helpers
{
    public static class SeoHelper
    {
        public static string SeourlCreater(string url)
        {
            url = url.ToLower();
            url.Replace("ə","e").Replace("ü","u").Replace("ö","o").Replace("ı","i").Replace("ğ","g");

            string result= Regex.Replace(url,"[^a-z0-9]","-");//her sozden sonra "-" atacaq

            return result;
        }

    }
}
