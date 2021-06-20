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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameShopProg
{
    /// <summary>
    /// Interaction logic for CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            UpdateCart();
        }

        private void UpdateCart()
        {
            MiniList.ItemsSource = UserInfo.User.Cart.ToList();
            BalanceTBlock.Text = $"В кошельке: {UserInfo.User.Money}";
            CartListBox.ItemsSource = UserInfo.User.Cart.ToList();
            var sum = UserInfo.User.Cart.Select(r => r.RegionPrice).Sum();
            TotalTBlock.Text = $"Итог: {sum} руб";
            PayButton.IsEnabled = sum < UserInfo.User.Money;
        }
        private void AddMoney(object sender, MouseButtonEventArgs e)
        {
            new PayWindow().ShowDialog();
            UpdateCart();
        }

        private void PayClick(object sender, RoutedEventArgs e)
        {
            var games = UserInfo.User.Cart.ToList();
            var sum = UserInfo.User.Cart.Select(r => r.RegionPrice).Sum();
            UserInfo.User.Money -= sum;
            foreach (var game in games)
            {
                UserInfo.User.UserGame.Add(new UserGame 
                {
                    Game = game,
                    BuyPrice = game.RegionPrice,
                    BuyDate = DateTime.Now
                }
                );
                UserInfo.User.Cart.Remove(game);
            }
            DB.Context.SaveChanges();
            UpdateCart();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {

        }

        private void DelGameClick(object sender, MouseButtonEventArgs e)
        {
            var btn = (Label)sender;
            var game = (Game)btn.DataContext;
            UserInfo.User.Cart.Remove(game);
            DB.Context.SaveChanges();
            UpdateCart();
        }

    }
}
