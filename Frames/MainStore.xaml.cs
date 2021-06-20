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
using GameShopProg.EF;

namespace GameShopProg
{
    /// <summary>
    /// Interaction logic for MainStore.xaml
    /// </summary>
    public partial class MainStore : Page
    {
        public MainStore()
        {
            InitializeComponent();
            foreach (var category in DB.Context.Category.OrderBy(r => r.Name).ToList())
            {
                var cb = new CheckBox()
                {
                    Content = category.Name,
                    IsChecked = true
                };
                cb.Checked += UpdGames;
                cb.Unchecked += UpdGames;
                categories.Add(cb);
                CategoriesWarpPanel.Children.Add(cb);
            }


            UpdateGames();
        }

        List<CheckBox> categories = new List<CheckBox>();
        List<Game> games;
        bool isGridOpen = false;
        Grid openGrid = null;


        private void UpdateGames()
        {
            games = DB.Context.Game.ToList();

            if (!string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                var input = SearchTB.Text.Trim().ToLower();

                games = games.Where(r => r.Name.ToLower().Contains(input))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(MinPriceTextBox.Text))
            {
                double minPrice = 0;
                if (!double.TryParse(MinPriceTextBox.Text.Replace('.', ','), out minPrice))
                {
                    HelperClass.ShowErrorMsg("Неверный формат минимальной цены");
                    return;
                }
                games = games.Where(r => r.Price > (decimal)minPrice).ToList();
            }

            if (!string.IsNullOrWhiteSpace(MaxPriceTextBox.Text))
            {
                double maxPrice = 0;
                if (!double.TryParse(MaxPriceTextBox.Text.Replace('.', ','), out maxPrice))
                {
                    HelperClass.ShowErrorMsg("Неверный формат максимальной цены");
                    return;
                }
                games = games.Where(r => r.Price < (decimal)maxPrice).ToList();
            }

            var ctgNames = categories
                .Where(cb => cb.IsChecked.HasValue && cb.IsChecked.Value)
                .Select(cb => (string)cb.Content)
                .ToHashSet();

            games = games
                .Where(r => r.Category.Count > 0 &&
                r.Category.Select(c => ctgNames.Contains(c.Name)).Aggregate((b1, b2) => b1 || b2))
                .ToList();

            GamesListView.ItemsSource = games;
        }

        private void SearchGotMouseCapture(object sender, MouseEventArgs e)
        {
            SecondRowDefinition.Height = new GridLength(0.3, GridUnitType.Star);
            PriceDiapasonStackPanel.Visibility = Visibility.Visible;
            CategoriesWarpPanel.Visibility = Visibility.Visible;
            ClearBtn.Visibility = Visibility.Visible;
            HideBtn.Visibility = Visibility.Visible;
        }

        private void SearchButtonClick(object sender, MouseButtonEventArgs e)
        {
            UpdateGames();
        }


        private void UpdGames(object sender, EventArgs e)
        {
            UpdateGames();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var cb in categories)
            {
                cb.IsChecked = false;
            }
            SearchTB.Clear();
            MinPriceTextBox.Clear();
            MaxPriceTextBox.Clear();
        }

        private void HideBtn_Click(object sender, RoutedEventArgs e)
        {
            SecondRowDefinition.Height = new GridLength(0.2, GridUnitType.Star);
            PriceDiapasonStackPanel.Visibility = Visibility.Collapsed;
            CategoriesWarpPanel.Visibility = Visibility.Collapsed;
            ClearBtn.Visibility = Visibility.Collapsed;
            HideBtn.Visibility = Visibility.Collapsed;
        }

        private void SearchTBTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGames();
        }

        private void GameClick(object sender, MouseButtonEventArgs e)
        {
            if (isGridOpen && sender.Equals(openGrid))
            {
                CloseGameDetail(openGrid);
                return;
            }

            if (isGridOpen)
                CloseGameDetail(openGrid);

            OpenGameDetail((Grid)sender);
        }

        private void OpenGameDetail(Grid grid)
        {
            grid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
            grid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions[3].Height = new GridLength(0.5, GridUnitType.Star);
            openGrid = grid;
            isGridOpen = true;
        }

        private void CloseGameDetail(Grid grid)
        {
            grid.RowDefinitions[0].Height = new GridLength(0.8, GridUnitType.Star);
            grid.RowDefinitions[1].Height = new GridLength(0.2, GridUnitType.Star);
            grid.RowDefinitions[2].Height = new GridLength(0);
            grid.RowDefinitions[3].Height = new GridLength(0);
            openGrid = null;
            isGridOpen = false;
        }

        private void AddToCartClick(object sender, RoutedEventArgs e)
        {
            var selected = (Game)GamesListView.SelectedItem;
            UserInfo.User.Cart.Add(selected);
            DB.Context.SaveChanges();
            UpdateGames();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            var selected = (Game)GamesListView.SelectedItem;
            UserInfo.User.Cart.Remove(selected);
            DB.Context.SaveChanges();
            UpdateGames();
        }
    }
}
