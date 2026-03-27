using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Auction.Model
{
    public class Auction : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Fields

        private int _id;
        private string _auctionName;
        private int _cena;
        private string _pobednik; 
        private int _vreme;
        private DateTime _VremePoslednjeAukcije;
        private int trenutna_cena;
        private string _trenutni;
        private string korisnik;
        private TimeSpan _preostaloVreme;

        private DispatcherTimer tajmer = new DispatcherTimer();

        #endregion

        public Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }
        public string AuctionName
        {
            get { return _auctionName; }
            set
            {
                if (_auctionName == value)
                {
                    return;
                }
                _auctionName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AuctionName"));
            }
        }
        public int Cena
        {
            get { return _cena; }
            set
            {
                if(_cena == value)
                {
                    return;
                }
                _cena = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cena"));
            }
        }
        public string Pobednik
        {
            get { return _pobednik; }
            set
            {
                if(_pobednik == value)
                {
                    return;
                }
                _pobednik = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Pobednik"));
            }
        }
        public int Vreme
        {
            get { return _vreme; }
            set 
            {
                if( _vreme == value)
                {
                    return;
                }
                _vreme = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Vreme"));
            }
        }

        public string Trenutni
        {
            get
            {
                return _trenutni;
            }
            set
            {
                if(_trenutni == value)
                {
                    return;
                }
                _trenutni = value;
                OnPropertyChanged(new PropertyChangedEventArgs("trenutni"));
            }
        }

        public DateTime VremePoslednjeAukcije
        {
            get
            {
                return _VremePoslednjeAukcije;
            }
            set
            {
                _VremePoslednjeAukcije = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastBidAt"));
            }
        }

        public TimeSpan PreostaloVreme
        {
            get => _preostaloVreme;
            set
            {
                if (_preostaloVreme != value)
                {
                    _preostaloVreme = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PreostaloVreme"));
                }
            }
        }

        public bool HasErrors
        {
            get
            {
                return (errors.Count > 0);
            }
        }

        #endregion

        #region Constructor
        public Auction(string AuctionName, int Cena, DateTime VremePoslednjeAukcije)
        {
            this.AuctionName = AuctionName;
            this.Cena = Cena;
            this.VremePoslednjeAukcije = VremePoslednjeAukcije;
        }
        public Auction(int Id, string AuctionName, int Cena, DateTime VremePoslednjeAukcije)
        {
            this.Id = Id;
            this.AuctionName = AuctionName;
            this.Cena = Cena;
            this.VremePoslednjeAukcije = VremePoslednjeAukcije;
        }

        public Auction(int Id, string AuctionName, int Cena, string Pobednik, DateTime VremePoslednjeAukcije, string Trenutni)
        {
            this.Id = Id;
            this.AuctionName = AuctionName;
            this.Cena = Cena;
            this.Pobednik = Pobednik;
            this.VremePoslednjeAukcije = VremePoslednjeAukcije;
            this.Trenutni = Trenutni;
        }
        public Auction()
        {

        }

        public Auction(string trenutni)
        {
            /*korisnik = trenutni;*/
        }




        #endregion

        #region DataAces

        public static Auction GetAuctionFromResultSet(SqlDataReader reader)
        {
            Auction auction = new Auction((int)reader["id"], (string)reader["auction_name"], (int)reader["cena"],(string)reader["pobednik"], (DateTime)reader["VremePoslednjeAukcije"], (string)reader["trenutni"]);
            return auction;
        }

        public void DeleteAuction()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE Auction04 SET is_deleted=1 WHERE id=@id", conn);

                SqlParameter myParam = new SqlParameter("@id", SqlDbType.Int, 11);
                myParam.Value = Id;

                command.Parameters.Add(myParam);

                int rows = command.ExecuteNonQuery();
            }
        }
        public void Insert()
        {
            using(SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                Trenutni = "";
                Pobednik = "";
                VremePoslednjeAukcije = DateTime.Now;

                SqlCommand command = new SqlCommand("INSERT INTO Auction04(auction_name, cena, pobednik, VremePoslednjeAukcije, trenutni) VALUES(@AuctionName, @Cena, @Pobednik, @Vreme, @Trenutni); SELECT SCOPE_IDENTITY();", conn);

                SqlParameter auctionNameParam = new SqlParameter("@AuctionName", SqlDbType.NVarChar);
                auctionNameParam.Value = this.AuctionName;

                SqlParameter cenaParam = new SqlParameter("@Cena", SqlDbType.Int);
                cenaParam.Value = this.Cena;

                SqlParameter pobednikParam = new SqlParameter("@Pobednik", SqlDbType.NVarChar);
                pobednikParam.Value = this.Pobednik;

                SqlParameter vremeParam = new SqlParameter("@Vreme", SqlDbType.DateTime);
                vremeParam.Value = this.VremePoslednjeAukcije;

                SqlParameter trenutniParam = new SqlParameter("@Trenutni", SqlDbType.NVarChar);
                trenutniParam.Value = this.Trenutni;

                command.Parameters.Add(auctionNameParam);
                command.Parameters.Add(cenaParam);
                command.Parameters.Add(vremeParam);
                command.Parameters.Add(pobednikParam);
                command.Parameters.Add(trenutniParam);

                var id = command.ExecuteScalar();

                if (id != null)
                {
                    this.Id = Convert.ToInt32(id);
                }
            }
        }

        public void UpdateTime()
        {
            int trajanje = 120;
            if(VremePoslednjeAukcije == DateTime.MinValue)
            {
                PreostaloVreme = TimeSpan.Zero;
                Vreme = 0;
                return;
            }


            TimeSpan ts = DateTime.Now - VremePoslednjeAukcije;
            int preostalo = trajanje - (int)ts.TotalSeconds;

            if (preostalo <= 0)
            {
                PreostaloVreme = TimeSpan.Zero;
                Vreme = 0;

                this.Pobednik = this.Trenutni;

                 

                using (SqlConnection conn = new SqlConnection())
                {
                
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                    conn.Open();



                    SqlCommand command = new SqlCommand("UPDATE Auction04 SET vreme=@Vreme, pobednik=@Pobednik WHERE id=@Id", conn);

                    SqlParameter pobednikParam = new SqlParameter("@Pobednik", SqlDbType.NVarChar);
                    pobednikParam.Value = this.Pobednik;

                    SqlParameter vremeParam = new SqlParameter("@Vreme", SqlDbType.Int, 11);
                    vremeParam.Value = this.Vreme;

                    SqlParameter myParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                    myParam.Value = this.Id;

                    command.Parameters.Add(myParam);
                    command.Parameters.Add(vremeParam);
                    command.Parameters.Add(pobednikParam);

                    int rows = command.ExecuteNonQuery();
                    
                }
            }
            else
            {
                PreostaloVreme = TimeSpan.FromSeconds(preostalo);
                Vreme = preostalo;
            }
        }

        public void UpdateTimeAndPrice(string TrenutniKorisnik)
        {
            
           
            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                    conn.Open();

                    Trenutni = TrenutniKorisnik;

                    Cena += 1;

                    VremePoslednjeAukcije = (DateTime.Now);

                    SqlCommand command = new SqlCommand("UPDATE Auction04 SET VremePoslednjeAukcije=@VremePoslednjeAukcije, cena=@cena, trenutni=@Trenutni WHERE id=@Id", conn);

                    SqlParameter vremePoslednjeAukcije = new SqlParameter("@VremePoslednjeAukcije", SqlDbType.DateTime);
                    vremePoslednjeAukcije.Value = this.VremePoslednjeAukcije;

                    SqlParameter cenaParam = new SqlParameter("@cena", SqlDbType.Int);
                    cenaParam.Value = this.Cena;

                    SqlParameter pobednikParam = new SqlParameter("@Trenutni", SqlDbType.NVarChar);
                    pobednikParam.Value = this.Trenutni;

                    SqlParameter myParam = new SqlParameter("@Id", SqlDbType.Int);
                    myParam.Value = this.Id;

                    command.Parameters.Add(myParam);
                    command.Parameters.Add(vremePoslednjeAukcije);
                    command.Parameters.Add(cenaParam);
                    command.Parameters.Add(pobednikParam);

                    int rows = command.ExecuteNonQuery();

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void Save()
        {
            if (Id == 0)
            {
                Insert();
            }
            
        }
        #endregion

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            // Clear any errors that already exist for this property.
            errors.Remove(propertyName);
            // Add the list collection for the specified property.
            errors.Add(propertyName, propertyErrors);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            // Remove the error list for this property.
            errors.Remove(propertyName);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Provide all the error collections.
                return (errors.Values);
            }
            else
            {
                // Provice the error collection for the requested property
                // (if it has errors).
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }

        public Auction Clone()
        {
            Auction clonedAuction = new Auction();
            clonedAuction.AuctionName = this.AuctionName;
            clonedAuction.Cena = this.Cena;
            clonedAuction.Pobednik = this.Pobednik;
            clonedAuction.Vreme = this.Vreme;
            clonedAuction.Trenutni = this.Trenutni;
            clonedAuction.Id = Id;

            return clonedAuction;
            
        }

        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Auction objAuction = (Auction)obj;

            if(objAuction.Id == this.Id) return true;

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
