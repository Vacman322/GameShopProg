using GameShopProg.EF;
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
using System.Windows.Shapes;

namespace GameShopProg
{
    /// <summary>
    /// Interaction logic for AddKidWindow.xaml
    /// </summary>
    public partial class AddKidWindow : Window
    {
        FamilyView family;
        bool isAdd = false;
        public AddKidWindow(FamilyView familyView = null)
        {
            family = familyView;
            if (familyView is null)
            {
                isAdd = true;
                family = new FamilyView();
            }

            InitializeComponent();
            if (isAdd)
            {
                var logins = DB.Context.User
                .ToList();
                logins.Remove(UserInfo.User);
                logins = logins.Except(UserInfo.User.Kids.Select(r => r.Kid)).ToList();
                LoginsCB.ItemsSource = logins;
                LoginsCB.DisplayMemberPath = "Login";
            }            
            else
            {
                LoginsCB.ItemsSource = new List<User>() { family.Kid };
                LoginsCB.SelectedIndex = 0;
                LoginsCB.DisplayMemberPath = "Login";
                LoginsCB.IsEnabled = false;
                DelBlock.Visibility = Visibility.Visible;
                TimeLimitTB.Text = family.DailyLimitInMinutes.ToString();
                accessCB.IsChecked = family.AccessFamilyLibrary;
                AddButton.Content = "Сохранить";
            }

        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (LoginsCB.SelectedItem is null)
            {
                HelperClass.ShowErrorMsg("Выберите пользователя");
                return;
            }

            if (string.IsNullOrWhiteSpace(TimeLimitTB.Text))
            {
                HelperClass.ShowErrorMsg("Заполните все поля");
                return;
            }

            if (!Validate.IsPositiveNumber(TimeLimitTB.Text))
            {
                HelperClass.ShowErrorMsg("Не верный формат ограничения по времени");
                return;
            }

            var user = (User)LoginsCB.SelectedItem;
            var limit = int.Parse(TimeLimitTB.Text);
            var access = accessCB.IsChecked.Value;

            family.Kid = user;
            family.DailyLimitInMinutes = limit;
            family.AccessFamilyLibrary = access;

            if(isAdd)
                UserInfo.User.Kids.Add(family);

            DB.Context.SaveChanges();

            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DelClick(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите удалить?", "Удалить", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                return;

            UserInfo.User.Kids.Remove(family);
            DB.Context.SaveChanges();
            this.Close();
        }
    }
}
