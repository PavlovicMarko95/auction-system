using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Auction.Model
{
    public class AuctionCollection : ObservableCollection<Auction>
    {
        public static AuctionCollection GetAllAuctions() 
        {


            AuctionCollection auctions = new AuctionCollection();
            Auction auction = null;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT id, auction_name, cena, pobednik, VremePoslednjeAukcije, trenutni FROM Auction04 WHERE DATEDIFF(SECOND, VremePoslednjeAukcije, GETDATE()) <= 120", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        auction = Auction.GetAuctionFromResultSet(reader);
                        auctions.Add(auction);
                    }
                }
            }
            return auctions;
        }
    }
}
