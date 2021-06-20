using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for RegistrationWidows.xaml
    /// </summary>
    public partial class RegistrationWidows : Window
    {
        static List<FieldInfo> textBoxesInfo = typeof(RegistrationWidows)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(f => f.FieldType == typeof(TextBox))
            .ToList();

        public RegistrationWidows()
        {
            InitializeComponent();
        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            if(!Validate.IsAllTextBoxesFill(textBoxesInfo,this))
            {
                HelperClass.ShowErrorMsg("Заполните все поля!");
                return;
            }

            if(string.IsNullOrWhiteSpace(PasswordPasBox.Password))
            {
                HelperClass.ShowErrorMsg("Заполните пароль!");
                return;
            }

            if(BirthdayDatePicker.SelectedDate is null)
            {
                HelperClass.ShowErrorMsg("Выберете дату!");
                return;
            }

            if(!Validate.IsEmailValid(EmailTextBox.Text))
            {
                HelperClass.ShowErrorMsg("Неверный формат почты!");
                return;
            }

            if(PasswordPasBox.Password.Length < 6)
            {
                HelperClass.ShowErrorMsg("Слишком короткий пароль!");
                return;
            }

            if(!Validate.IsPasswordContainsUppercaseAndLowercase(PasswordPasBox.Password))
            {
                HelperClass.ShowErrorMsg("Пароль должен содержать строчные и прописные буквы");
                return;
            }

            if(!Validate.IsContainsSpecialSymbols(PasswordPasBox.Password))
            {
                HelperClass.ShowErrorMsg("Пароль должен иметь спец. Символы");
                return;
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите отменить?", "Отмена", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                return;

            this.Close();
        }
    }
}
