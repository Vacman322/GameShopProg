using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameShopProg
{
    /// <summary>
    /// Interaction logic for PayWindow.xaml
    /// </summary>
    public partial class PayWindow : Window
    {

        static List<FieldInfo> textBoxesInfo = typeof(PayWindow)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(f => f.FieldType == typeof(TextBox))
            .ToList();

        public PayWindow()
        {
            InitializeComponent();
        }

        private void PayBtnClick(object sender, RoutedEventArgs e)
        {
            if (!IsAllTextBoxesFill())
            {
                HelperClass.ShowErrorMsg("Заполните все поля!");
                return;
            }

            if(!IsAllTextBoxesFillWithDigits())
            {
                HelperClass.ShowErrorMsg("Все поля в этом окне должны содержать только неотрицательные цифры");
                return;
            }

            if (CardNumberTB.Text.Length != 16)
            {
                HelperClass.ShowErrorMsg("Неправильное количество цифр в номере карты!");
                return;
            }

            int year = int.Parse(YearTB.Text);
            int month = int.Parse(MonthTB.Text);
            if(month > 12 || month == 0 || year > DateTime.Now.Year + 100 || year <= 0)
            {
                HelperClass.ShowErrorMsg("Неверная дата");
                return;
            }

            if(ScvTB.Text.Length != 3)
            {
                HelperClass.ShowErrorMsg("SCV код должен содержать 3 числа");
                return;
            }

            if(new DateTime(year, month,1) < DateTime.Now)
            {
                HelperClass.ShowErrorMsg("У карты закончился срок");
                return;
            }

            int sum = int.Parse(SumTB.Text);
            if(sum > 5000)
            {
                HelperClass.ShowErrorMsg("Нельзя пополнять счёт больше чем на 5000 руб");
                return;
            }

            UserInfo.User.Money += sum;
            DB.Context.SaveChanges();
            MessageBox.Show("Оплата прошла успешно!","Оплата",MessageBoxButton.OK);
            this.Close();
        }

        private bool IsAllTextBoxesFill()
        {
            foreach (var tbInfo in textBoxesInfo)
            {
                var tb = (TextBox)tbInfo.GetValue(this);
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAllTextBoxesFillWithDigits()
        {
            foreach (var tbInfo in textBoxesInfo)
            {
                var tb = (TextBox)tbInfo.GetValue(this);
                if (tb.Text.Any(c => !char.IsDigit(c)))
                {
                    return false;
                }
            }
            return true;
        }

        private void CancelBtnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
