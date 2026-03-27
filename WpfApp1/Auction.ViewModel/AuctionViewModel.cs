using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;
using Administration.ViewModel;

namespace Auction.ViewModel
{
    public class AuctionViewModel : INotifyPropertyChanged
    {
        #region Fields

        private Auction.Model.Auction currentAuction;
        private AuctionCollection auctionList;

        public string trenutniKorisnik;

        private Mediator mediator;

        
        #endregion

        #region Properties

        public Auction.Model.Auction CurrentAuction
        {
            get { return currentAuction; }
            set
            {
                if (currentAuction == value)
                {
                    return;
                }
                currentAuction = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentAuction"));
            }
        }
        public AuctionCollection AuctionList
        {
            get { return auctionList; }
            set
            {
                if (auctionList == value)
                {
                    return;
                }
                auctionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AuctionList"));
            }
        }

        public string TrenutniKorisnik
        {
            get { return trenutniKorisnik; }
            set
            {
                if (trenutniKorisnik == value)
                {
                    return;
                }
                trenutniKorisnik = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrenutniKorisnik"));
            }
        }
        #endregion

        #region Constructors
        public AuctionViewModel(Mediator mediator) {

            this.mediator = mediator;

            BidCommand = new RelayCommand(BidExecute, CanBid);

            AuctionList = AuctionCollection.GetAllAuctions();

            CurrentAuction = new Model.Auction();

            /*if (AuctionList.Count > 0) 
            { 
                CurrentAuction = AuctionList[0]; 
            }*/

            mediator.Register("AuctionChange", AuctionChanged);

        }
        


        #endregion
        

        private void AuctionChanged(object obj)
        {
            Auction.Model.Auction auction = (Auction.Model.Auction)obj;

            int index = AuctionList.IndexOf(auction);

            if (index != -1) 
            { 
                AuctionList.RemoveAt(index);
                AuctionList.Insert(index, auction);
            }
            else
            {
                AuctionList.Add(auction);
            }
        }

        private ICommand bidCommand;

        public ICommand BidCommand
        {
            get { return bidCommand; }
            set
            {
                if(bidCommand == value)
                {
                    return;
                }
                bidCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BidCommand"));
            }
        }

        void BidExecute(object obj)
        {
            CurrentAuction.UpdateTimeAndPrice(TrenutniKorisnik.ToString());
        }

        bool CanBid(object obj)
        {
            if (CurrentAuction == null) return false;

            foreach (var auction in AuctionList)
            {
                if(CurrentAuction.Vreme == 0)
                {
                    return false;
                }
            }
            return true;
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
