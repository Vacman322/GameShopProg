using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameShopProg
{
    public class HelperClass
    {
        public static void ShowErrorMsg(string msg, string cpt = "Ошибка")
        {
            MessageBox.Show(msg, cpt, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
