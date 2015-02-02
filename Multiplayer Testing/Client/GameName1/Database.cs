using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace GameName1
{
//    class Database
//    {
//        private MySqlConnection myConnection = new MySqlConnection("Server=benadlington.biz;Port=3306;Database=test;Uid=Ben;Pwd=Adlington1337");
//        private string newUserQuery;
//        public Point getPlayerPos()
//        {
//            myConnection.Open();
//            newUserQuery = "SELECT X FROM multiplayer WHERE name != ?name";
//            MySqlCommand newUserCmd = new MySqlCommand(newUserQuery, myConnection);
//            newUserCmd.Parameters.AddWithValue("?name", "ben");
//            int x = (int)newUserCmd.ExecuteScalar();
//            newUserQuery = "SELECT Y FROM multiplayer WHERE name != ?name";
//            newUserCmd = new MySqlCommand(newUserQuery, myConnection);
//            newUserCmd.Parameters.AddWithValue("?name", "ben");
//            int y = (int)newUserCmd.ExecuteScalar();

//            Point n = new Point(x, y);
//            return n;
//        }
//        public void updatePlayerPosition(Point location, string name)
//        {
//            myConnection.Open();
//            newUserQuery = "UPDATE multiplayer SET x = ?x, y = ?y WHERE Name = ?name";
//            MySqlCommand newUserCmd = new MySqlCommand(newUserQuery, myConnection);
//            newUserCmd.Parameters.AddWithValue("?x", location.X);
//            newUserCmd.Parameters.AddWithValue("?y", location.Y);
//            newUserCmd.Parameters.AddWithValue("?name", name);
//            newUserCmd.ExecuteNonQuery();
//        }
//    }
}
