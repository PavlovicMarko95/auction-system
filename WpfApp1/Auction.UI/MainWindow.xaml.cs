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
using System.Windows.Threading;
using Administration.ViewModel;
using Auction.ViewModel;

namespace Auction.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

     
        public MainWindow(string ime)
        {
            InitializeComponent();

            AuctionViewModel auctionViewModel = new AuctionViewModel(Mediator.Instance);
            auctionViewModel.TrenutniKorisnik = ime;
            this.DataContext = auctionViewModel;


            DispatcherTimer tajmer = new DispatcherTimer();
            tajmer.Interval = TimeSpan.FromSeconds(1);
            tajmer.Tick += Timer_Tick;
            tajmer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            AuctionViewModel viewModel = (AuctionViewModel)DataContext;

            foreach (var auction in viewModel.AuctionList) 
            {
                auction.UpdateTime();
            }

            if (UserName.Content.ToString() == "")
            {
                bid.IsEnabled = false;
                newbtn.IsEnabled = false;
            }
            else if (UserName.Content.ToString() == "Paka")
            {
                bid.IsEnabled = true;
                newbtn.IsEnabled = true;
            }
            else
            {
                bid.IsEnabled = true;
                newbtn.IsEnabled = false;
            }
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
           MainWindow mainWindow = new MainWindow("");
           mainWindow.ShowDialog();

           this.Hide();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewAuction newAuction = new NewAuction();
            newAuction.DataContext= new NewAuctionWindowViewModel(Mediator.Instance);
            newAuction.ShowDialog();
        }
    }
}
