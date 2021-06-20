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
    /// Interaction logic for BlockAccWindow.xaml
    /// </summary>
    public partial class BlockAccWindow : Window
    {
        FamilyView familyView;
        public BlockAccWindow(FamilyView familyView)
        {
            InitializeComponent();
            this.familyView = familyView;
            BlockDate.SelectedDate = familyView.BanEnd;
            FullBanCB.IsChecked = familyView.IsFullBan;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            familyView.IsFullBan = FullBanCB.IsChecked.Value;

            if (BlockDate.SelectedDate is null)
            {
                HelperClass.ShowErrorMsg("Выберите дату");
                return;
            }

            familyView.BanEnd = BlockDate.SelectedDate;
            DB.Context.SaveChanges();
            this.Close();
        }
    }
}
