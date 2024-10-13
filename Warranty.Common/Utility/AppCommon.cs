using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Warranty.Common.Utility
{
    public class AppCommon
    {
        #region Variables
        public static string ApplicationName = "WarrantyManagement";
        public static string ApplicationLongTitle = "WarrantyManagement";
        public static string ApplicationTitle = "WarrantyManagement";
        public static string ErrorMessage = "Something Went Wrong. Please Contact Administrator!";
        public static string ErrorMessage2 = "No ledger entry with sufficient quantity found.";
        public static string ErrorMessage3 = "Corresponding product not found.";
        public static string ErrorMessage1 = " Please Contact Administrator!";
        public static string DefaultMenuName = "Dashboard";
        public static string NotFound = "Record not found. Please Contact Administrator!";
        public static string DateTimeFormat = "dd/MM/yyyy hh:mm tt";
        public static string TimeFormat = " hh:mm tt";
        public static string DateOnlyFormat = "dd/MM/yyyy";
       
        public static string StringLeadIdFormat = "AD00000";
        public static string Protection = "WarrantyManagement";
        public static string AppRootPath = "";
        public static string AppUrl = "";
        public static string WarrantyCode = "WR";
        public static string Fax_KEY = "";

        public static string Alpha_API = "https://api.alphaderalabs.com/";
        public static string ConnectionString = "";
        public static string FileNameSeperator = "__--__";
        public static string SessionName = "WarrantyManagement.Session";
        public static string SuccessMsg_Save = "Save Data";
        public static string SuccessMsg_Update = "Updated Data";
        public static string StringToNunberFormat = "00000";
        public static string CaliforniaPrefixStartDate = "11-MAY-2023";
        public static string BoxUnMatchedFolder = "";
        public static string GoogleDriveAPI = "";
        public static string OthDoc = "242150538077";
        public static string BoxAudioFolder = "242151808441";

        #region TempDataFiltersKeys
        public static string TMP_SearchText = "TMP_SearchText";
        public static string TMP_LeadsStatus = "TMP_LeadsStatus";
        public static string TMP_SellersStatus = "TMP_SellersStatus";
        public static string TMP_RecordStatus = "TMP_RecordStatus";
        public static string TMP_DateRange = "TMP_DateRange";
        public static string TMP_AllocatedTo = "TMP_AllocatedTo";
        public static string TMP_AllocatedDate = "TMP_AllocatedDate";
        public static string TMP_ReturnAndRefundStatus = "TMP_ReturnAndRefundStatus";
        public static string TMP_SupplierMaster = "TMP_SupplierMaster ";
        public static string TMP_ClientMaster = "TMP_ClientMaster ";
        #endregion


        #endregion

        #region Methods
        public static DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }



        public static void LogException(Exception ex, string source = "")
        {
            try
            {
                var TraceMsg = ex.StackTrace.ToString();
                var ErrorLineNo = TraceMsg.Substring(ex.StackTrace.Length - 7, 7);
                var ErrorMsg = ex.Message.ToString();
                var ErrorMsginDept = ex.InnerException;
                var Errortype = ex.GetType().ToString();
                var ErrorLocation = ex.Message.ToString();
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/LogFiles/");
                filePath = filePath.Replace("AlphaDeraDCC.API", "AlphaDeraDCC.Web").Replace("WebAPI\\", "");

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                filePath = filePath + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("---------------------------------------------------------------------------------------");
                    sw.WriteLine("Log date time     : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
                    sw.WriteLine("Source			: " + source);
                    sw.WriteLine("Error line number : " + ErrorLineNo);
                    sw.WriteLine("Error message     : " + ErrorMsg + ErrorLocation);
                    sw.WriteLine("Trace message     : " + TraceMsg);
                    sw.WriteLine("Inner Exception   : " + ErrorMsginDept);
                    sw.WriteLine("----------------------------------------------------------------------------------------");
                    sw.WriteLine("\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
        public static string RemoveExtra(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Replace("~", "").Replace("_", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "")
                .Replace("%", "").Replace("^", "").Replace("&", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "").Replace("+", "").Replace("{", "")
                .Replace("}", "").Replace("[", "").Replace("]", "").Replace("\\", "").Replace("|", "").Replace(":", "").Replace(";", "").Replace("\"", "").Replace("'", "")
                .Replace("<", "").Replace(",", "").Replace(".", "").Replace(">", "").Replace("?", "").Replace("/", "").Replace("*", "").Replace(" ", "");
            return value;
        }
        public static string GetFormatedPhoneNumber(string phone)
        {
            phone = RemoveExtra(phone);
            if (!string.IsNullOrEmpty(phone) && phone != "" && phone.Trim().Length == 10)
                return string.Format("({0}) {1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6));
            return phone;
        }
        public static string GetFormatedZipCode(string zipCode)
        {
            if (!string.IsNullOrEmpty(zipCode))
            {
                zipCode = RemoveExtra(zipCode);
                int len = zipCode.Trim().Length;
                if (len > 6)
                {
                    int pos = len - 6;
                    return string.Format("{0}-{1}", zipCode.Substring(0, 6), zipCode.Substring(6, pos));
                }
                else
                    return zipCode;
            }
            return "";
        }
        public static string FormatMoney(double money)
        {
            return String.Format("{0:C}", money, CultureInfo.CreateSpecificCulture("en-US"));
        }
        public static string FormatNumber(decimal value, short decimalPoints = 2)
        {
            return value.ToString("N" + decimalPoints);
        }
        public static string FormatPercentage(decimal percentage)
        {
            return String.Format("{0:P}", percentage, CultureInfo.CreateSpecificCulture("en-US"));
        }
        public static int ConvertToInt32(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static decimal ConvertToDecimal(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                return Convert.ToDecimal(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            var newColor = Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
            return "#" + newColor.R.ToString("X2") + newColor.G.ToString("X2") + newColor.B.ToString("X2");
        }
        public static DateTime? ConvertToDate(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return null;
                DateTime.TryParse(value.Replace(",", ""), out var date);
                return date;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string ConvertToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var cultureInfo = System.Globalization.CultureInfo.InvariantCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var camelCase = string.Join(" ", words.Select(word => textInfo.ToTitleCase(word.ToLower())));

            return camelCase;
        }
        public static string Trim(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim();
            return value;
        }

        public static string ConvertToUpper(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ToUpper();
            else
                return value;
        }
        public static string ConvertToLower(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ToLower();
            else
                return value;
        }

        public static List<string> SplitString(string value, string delimiter)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Split(delimiter).ToList();
            return new List<string>();
        }
        public static bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
        public static string EncodeString(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace("\n", "").Replace("\r", "").Replace(" ", "-SP-").Replace("~", "-TLD-").Replace("!", "-EX-").Replace("@", "-AT-").Replace("$", "-DL-").Replace("^", "-CRT-").Replace("_", "-UN-").Replace("/", "-SL-").Replace(".", "-DT-").Replace("*", "-ST-").Replace("#", "-HS-").Replace("%", "-PR-").Replace("&", "-AD-").Replace("(", "-OB-").Replace(")", "-CB-").Replace("+", "-PL-").Replace(":", "-CLN-").Replace(",", "-CMA-").Replace("?", "-QM-").Replace("<", "-LT-").Replace(">", "-GT-").Replace("[", "-BBO-").Replace("]", "-BBC-").Replace("{", "-CBO-").Replace("}", "-CBC-").Replace("'", "-QT-").Replace("\"", "-DQT-");
        }
        public static string DecodeString(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace("-SP-", " ").Replace("-TLD-", "~").Replace("-EX-", "!").Replace("-AT-", "@").Replace("-DL-", "$").Replace("-CRT-", "^").Replace("-UN-", "_").Replace("-SL-", "/").Replace("-DT-", ".").Replace("-ST-", "*").Replace("-HS-", "#").Replace("-PR-", "%").Replace("-AD-", "&").Replace("-OB-", "(").Replace("-CB-", ")").Replace("-PL-", "+").Replace("-CLN-", ":").Replace("-CMA-", ",").Replace("-QM-", "?").Replace("-LT-", "<").Replace("-GT-", ">").Replace("-BBO-", "[").Replace("-BBC-", "]").Replace("-CBO-", "{").Replace("-CBC-", "}").Replace("-QT-", "'").Replace("-DQT-", "\"");
        }


        public static string GetFullAddress(string address1, string address2, string city, string state, string zip)
        {
            var list = new List<string>() { address1, address2, city, state }.Where(x => !string.IsNullOrEmpty(x)).ToList();
            return string.Join(", ", list) + " " + GetFormatedZipCode(zip);
        }

        public static int GetOnlyNumber(string value)
        {
            string resultString = "0";
            try
            {
                resultString = Regex.Match(value, @"\d+").Value;
            }
            catch (Exception)
            {

                throw;
            }
            return Int32.Parse(resultString);
        }
        public static bool IsFaxNumberValid(string faxNumber)
        {
            // Check if the fax number has a length of 10 and contains only digits
            return faxNumber.Length == 10 && faxNumber.All(char.IsDigit);
        }

        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1: return "Jan";
                case 2: return "Feb";
                case 3: return "Mar";
                case 4: return "Apr";
                case 5: return "May";
                case 6: return "Jun";
                case 7: return "Jul";
                case 8: return "Aug";
                case 9: return "Sep";
                case 10: return "Oct";
                case 11: return "Nov";
                case 12: return "Dec";
                default: return "";
            }
        }

        #endregion
    }
}
