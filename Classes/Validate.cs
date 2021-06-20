using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GameShopProg
{
    public class Validate
    {
        public static bool IsAllTextBoxesFill(List<FieldInfo> textBoxesInfo, object obj)
        {
            foreach (var tbInfo in textBoxesInfo)
            {
                var tb = (TextBox)tbInfo.GetValue(obj);
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    return false;
                }
            }
            return true;
        }


        public static bool IsEmailValid(string email)
        {
            try
            {
                var a = new MailAddress(email);
                if (a.Host.Last() == '.') return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsPasswordContainsUppercaseAndLowercase(string txt)
        {
            return txt.Any(s => char.IsUpper(s)) &&
                txt.Any(s => char.IsLower(s));
        }

        public static bool IsContainsSpecialSymbols(string txt)
        {
            return txt
                .Except(txt.Where(s => char.IsDigit(s)))
                .Except(txt.Where(s => char.IsLetter(s)))
                .Count() > 0;
        }

        public static bool IsPositiveNumber(string txt)
        {
            return txt.Select(c => char.IsDigit(c)).Aggregate((b1, b2) => b1 && b2);
        }
    }
}
