using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Administration.ViewModel;
using Auction.Model;

namespace Auction.ViewModel
{
    public class NewAuctionWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private Auction.Model.Auction currentAuction;

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
        #endregion

        #region Constructor
        public NewAuctionWindowViewModel(Mediator mediator) 
        {
            this.mediator = mediator;
            SaveCommand = new RelayCommand(SaveExecute, CanSave);
            CurrentAuction = new Auction.Model.Auction();
        }
        #endregion

        private ICommand saveCommand;

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if(saveCommand == value)
                {
                    return;
                }
                saveCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaveCommand"));
            }
        }

        void SaveExecute(object obj)
        {
            if(CurrentAuction != null && !CurrentAuction.HasErrors)
            {
                CurrentAuction.Insert();
                OnDone(new DoneEventArgs("Auction saved"));
                mediator.Notify("AuctionChange", CurrentAuction);
            }
            else
            {
                OnDone(new DoneEventArgs("Check your input"));
            }
        }

        bool CanSave(object obj)
        {
            return true;
        }

        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            private string message;

            public string Message
            {
                get { return message; }
                set
                {
                    if (message == value)
                    {
                        return;
                    }
                    message = value;
                }
            }

            public DoneEventArgs(string message)
            {
                this.message = message;
            }
        }

        public event DoneEventHandler Done;

        public void OnDone(DoneEventArgs e)
        {
            if (Done != null)
            {
                Done(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
