using Microsoft.Win32;
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
using System.IO;
using System.Reflection;

namespace GameShopProg
{
    /// <summary>
    /// Interaction logic for EditProfileWindow.xaml
    /// </summary>
    public partial class EditProfileWindow : Window
    {
        static List<FieldInfo> TbsInfo = typeof(EditProfileWindow)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(f => f.FieldType == typeof(TextBox))
            .ToList();
        string imgName;
        bool isImgChg = false;
        public EditProfileWindow()
        {
            InitializeComponent();

            AvatarImg.DataContext = UserInfo.User;
            EmailTB.Text = UserInfo.User.Email;
            LastNameTB.Text = UserInfo.User.LastName;
            NameTB.Text = UserInfo.User.FirstName;
            PatronymicTB.Text = UserInfo.User.Patronymic;
            BirthDay.SelectedDate = UserInfo.User.DateOfBirth;
        }

        private void LoadImgClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.jpg;*.png;*.jpeg";
            var result = dialog.ShowDialog();

            if(result.HasValue && result.Value)
            {
                var file = new FileInfo(dialog.FileName);
                var saveDir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName +
                    "\\Resources\\ProfileImgs\\" + file.Name;
                file.CopyTo(saveDir,true);
                AvatarImg.Source = new BitmapImage(new Uri(saveDir));
                imgName = file.Name;
                isImgChg = true;
            }
        }

        private void DelAccountClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите отменит редактирование?", "Отмена", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
                return;

            this.Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (!Validate.IsAllTextBoxesFill(TbsInfo, this))
            {
                HelperClass.ShowErrorMsg("Заполните все поля!");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordB.Password))
            {
                HelperClass.ShowErrorMsg("Заполните пароль!");
                return;
            }

            if (BirthDay.SelectedDate is null)
            {
                HelperClass.ShowErrorMsg("Выберете дату!");
                return;
            }

            if (!Validate.IsEmailValid(EmailTB.Text))
            {
                HelperClass.ShowErrorMsg("Неверный формат почты!");
                return;
            }

            if (PasswordB.Password.Length < 6)
            {
                HelperClass.ShowErrorMsg("Слишком короткий пароль!");
                return;
            }

            if (!Validate.IsPasswordContainsUppercaseAndLowercase(PasswordB.Password))
            {
                HelperClass.ShowErrorMsg("Пароль должен содержать строчные и прописные буквы");
                return;
            }

            if (!Validate.IsContainsSpecialSymbols(PasswordB.Password))
            {
                HelperClass.ShowErrorMsg("Пароль должен иметь спец. Символы");
                return;
            }

            UserInfo.User.Email = EmailTB.Text;
            UserInfo.User.Password = PasswordB.Password;
            UserInfo.User.LastName = LastNameTB.Text;
            UserInfo.User.FirstName = NameTB.Text;
            UserInfo.User.Patronymic = PatronymicTB.Text;
            UserInfo.User.DateOfBirth = BirthDay.SelectedDate;
            if (isImgChg)
                UserInfo.User.ProfileImg = imgName;

            DB.Context.SaveChanges();

            MessageBox.Show("Сохранение прошло успешно","Сохранение", MessageBoxButton.OK);
            this.Close();
        }
    }
}
