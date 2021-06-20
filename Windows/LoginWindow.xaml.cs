using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameShopProg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Captcha captcha = new Captcha();
        string login = "";
        int tryCount = 0;
        public LoginWindow()
        {
            InitializeComponent();
            CaptchaImg.DataContext = captcha;

            LoginTextBox.Text = "log";
            PasPasswordBox.Password = "pas";
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (login == LoginTextBox.Text)
                tryCount++;
            else
                tryCount = 0;

            login = LoginTextBox.Text;

            if (tryCount >= 5)
            {
                HelperClass.ShowErrorMsg("Авторизация заблокирована");
                return;
            }

            if(string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasPasswordBox.Password))
            {
                HelperClass.ShowErrorMsg("Заполните все поля");
                return;
            }

            if(!DB.Context.User.Any(r=> r.Login.Equals(LoginTextBox.Text)))
            {
                HelperClass.ShowErrorMsg("Неверный логин");
                return;
            }

            var usr = DB.Context.User.FirstOrDefault(r => r.Login.Equals(LoginTextBox.Text));
            if (!usr.Password.Equals(PasPasswordBox.Password))
            {
                HelperClass.ShowErrorMsg("Неверный пароль");
                return;
            }

            //if (CaptchaTextBox.Text != captcha.CaptchaText)
            //{
            //    HelperClass.ShowErrorMsg("Ошибка в капче");
            //    captcha.Regenerate();
            //    return;
            //}

            new MainWindow().Show();
            UserInfo.User = usr;
            UserInfo.RememberMe = RememberMeCB.IsChecked.HasValue && RememberMeCB.IsChecked.Value;
            this.Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshClick(object sender, MouseButtonEventArgs e)
        {
            captcha.Regenerate();
        }

        private void RegistrationClick(object sender, MouseButtonEventArgs e)
        {
            new RegistrationWidows().ShowDialog();
        }
    }
}
