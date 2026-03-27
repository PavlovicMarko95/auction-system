using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Auction.Model
{
    public class Person : INotifyPropertyChanged
    {
        #region Fields

        private int _id;
        private string _firstName;
        private string _lastName;
        private bool _isAdmin;

        #endregion

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
                if(_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                {
                    return;
                }
                _firstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if(_lastName == value)
                {
                    return;
                }
                _lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
            }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
            }
        }

        #endregion

        #region Constructor

        public Person(string FirstName, string LastName, bool IsAdmin)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.IsAdmin = IsAdmin;
        }

        public Person(int Id, string FirstName, string LastName, bool IsAdmin)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.IsAdmin = IsAdmin;
            this.Id = Id;
        }

        public Person()
        {
            FirstName = "";
            LastName = "";
            IsAdmin = false;
        }

        #endregion

        #region DataAces

       /* public static List<Person> GetAllPersons()
        {
            List<Person> persons = new List<Person>();
            Person person = null;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT id, first_name, last_name, is_admin FROM Person", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person = new Person((int)reader["id"], (string)reader["first_name"], (string)reader["last_name"], (bool)reader["is_admin"]);
                        persons.Add(person);
                    }
                }
            }
            return persons;
        }*/

        public void UpdatePerson()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE Users SET first_name=@FirstName, last_name=@LastName, is_admin=@IsAdmin WHERE id=@Id", conn);

                SqlParameter firstNameParam = new SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar);
                firstNameParam.Value = this.FirstName;

                SqlParameter lastNameParam = new SqlParameter("@LastName", System.Data.SqlDbType.NVarChar);
                lastNameParam.Value = this.LastName;

                SqlParameter isAdminParam = new SqlParameter("@IsAdmin", System.Data.SqlDbType.Bit);
                isAdminParam.Value = this.IsAdmin;

                SqlParameter myParam = new SqlParameter("@Id", System.Data.SqlDbType.Int, 11);
                myParam.Value = this.Id;

                command.Parameters.Add(firstNameParam);
                command.Parameters.Add(lastNameParam);
                command.Parameters.Add(isAdminParam);
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

                SqlCommand command = new SqlCommand("INSERT INTO Person(first_name, last_name, is_admin) VALUES(@FirstName, @LastName, 0); SELECT IDENT_CURRENT('Person';)", conn);

                SqlParameter firstNameParam = new SqlParameter("@FirstName", SqlDbType.NVarChar);
                firstNameParam.Value = this.FirstName;

                SqlParameter lastNameParam = new SqlParameter("@LastName", SqlDbType.NVarChar);
                lastNameParam.Value = this.LastName;

                command.Parameters.Add(firstNameParam);
                command.Parameters.Add(lastNameParam);

                var id = command.ExecuteScalar();

                if(id != null)
                {
                    this.Id = Convert.ToInt32(id);
                }
            }
        }

        #endregion
    }


}
