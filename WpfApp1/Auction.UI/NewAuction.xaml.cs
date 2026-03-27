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
using Auction.ViewModel;

namespace Auction.UI
{
    /// <summary>
    /// Interaction logic for NewAuction.xaml
    /// </summary>
    public partial class NewAuction : Window
    {
        public NewAuction()
        {
            InitializeComponent();
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            NewAuctionWindowViewModel viewModel = (NewAuctionWindowViewModel)DataContext;
            viewModel.Done += ViewModel_Done;
        }

        private void ViewModel_Done(object sender, NewAuctionWindowViewModel.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
