using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp1
{
    internal class DB
    {
        
        MySqlConnection _mySqlConnection = new MySqlConnection($@"server=127.0.0.1;port=3306;username='{Form1.getUser()}';password='{Form1.getPassword()}';database=gitar_shop");


        public void openConnection()
        {
            
            if(_mySqlConnection.State == System.Data.ConnectionState.Closed)
                _mySqlConnection.Open();
        }
        public void closeConnection() 
        {
        if (_mySqlConnection.State == System.Data.ConnectionState.Open)
                _mySqlConnection.Close();
        }
        public MySqlConnection GetConnection()
        {
            return _mySqlConnection;

        }
    }
}
