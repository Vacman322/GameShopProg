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
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            UpateProfile();
        }

        private void UpateProfile()
        {
            ProfileGrid.DataContext = UserInfo.User;
            BindingOperations.GetBindingExpression(FioTb, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(AvatarImg, Image.SourceProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(AgeTB, TextBlock.TextProperty).UpdateTarget();
            GamesListBox.ItemsSource = UserInfo.User.UserGame
                .Select(r => r.Game)
                .OrderByDescending(r => r.Hours)
                .ToList();
            FriendsListBox.ItemsSource = UserInfo.User.Friends.ToList();
        }
        private void EditProfileClick(object sender, MouseButtonEventArgs e)
        {
            new EditProfileWindow().ShowDialog();
            UpateProfile();
        }

        private void FamilyViewClick(object sender, MouseButtonEventArgs e)
        {
            new FamilyViewSettingsWindow().ShowDialog();
            UpateProfile();
        }
    }
}
