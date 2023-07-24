using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyFirstAzureWebApp.Utility
{
    public static class Utility
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static string DMYtoYMD(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return "";
            }
            DateTime oDate = Convert.ToDateTime(DateTime.ParseExact(src, "dd/MM/yyyy", new CultureInfo("en-US")));
            return oDate.ToString("yyyyMMdd");
        }

        public static string getCurrentDtm(string format, string timeZone)
        {
            if (!string.IsNullOrEmpty(format))
            {
                //return DateTime.Now.ToString(format, new CultureInfo("th-TH"));
                return DateTime.Now.AddHours(Int32.Parse(timeZone)).ToString(format, new CultureInfo("th-TH"));
            }
            return null;
        }

        public static string ConvertTimeFrontToBackFormat(string dateTime)
        {
            if (String.IsNullOrEmpty(dateTime))
            {
                return dateTime;
            }
            //YYYYMMDD
            string tmp = dateTime.Substring(6, 2) + "/" + dateTime.Substring(4, 2) + "/" + (Int32.Parse(dateTime.Substring(0, 4)) + 543);
            return tmp;

            /*string tmp = dateTime.Split("T")[0];
            string[] tmpArr = tmp.Split("-");
            tmp = tmpArr[2] + "/" + tmpArr[1] + "/" + (Int32.Parse(tmpArr[0]) + 543);
            return tmp;*/

        }
    }
}
