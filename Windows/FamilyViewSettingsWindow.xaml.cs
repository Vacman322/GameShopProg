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
    /// Interaction logic for FamilyViewSettingsWindow.xaml
    /// </summary>
    public partial class FamilyViewSettingsWindow : Window
    {
        public FamilyViewSettingsWindow()
        {
            InitializeComponent();
            FamilyViewUsers.ItemsSource = UserInfo.User.Kids.ToList();
        }

        private void AddClick(object sender, MouseButtonEventArgs e)
        {
            new AddKidWindow().ShowDialog();
            FamilyViewUsers.ItemsSource = UserInfo.User.Kids.ToList();
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            var selected = (FamilyView)((Button)sender).DataContext;
            new AddKidWindow(selected).ShowDialog();
            FamilyViewUsers.ItemsSource = UserInfo.User.Kids.ToList();
        }

        private void BanClick(object sender, RoutedEventArgs e)
        {
            var selected = (FamilyView)((Button)sender).DataContext;
            if(!selected.IsAccepted)
            {
                HelperClass.ShowErrorMsg("Пользователь не подтверждён");
                return;
            }
            new BlockAccWindow(selected).ShowDialog();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
