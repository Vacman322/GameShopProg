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
using GameShopProg.EF;

namespace GameShopProg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainStore());
        }

        private void ListClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new MainStore());
            NavigationTextBlock.Text = "Главная страница";
        }

        private void CartClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new CartPage());
            NavigationTextBlock.Text = "Корзина";
        }

        private void ProfileClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage());
            NavigationTextBlock.Text = "Профиль";
        }
    }
}
