/*
 * Library for work with SQLite Database
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ProgramManagerVC
{
    class data
    {
        public static int SendQueryWithoutReturn(string query)
        {
            String dbFileName = "data.sqlite";
            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);

            try
            {
                SQLiteConnection m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                SQLiteCommand m_sqlCmd = new SQLiteCommand();
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;
                m_sqlCmd.CommandText = query;
                m_sqlCmd.ExecuteNonQuery();
                return 0;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                return 1;
            }
        }

        public static DataTable SendQueryWithReturn(string query)
        {
            DataTable dTable = new DataTable();
            String dbFileName = "data.sqlite";
            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);

            try
            {
                SQLiteConnection m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                SQLiteCommand m_sqlCmd = new SQLiteCommand();
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, m_dbConn);
                adapter.Fill(dTable);
                return dTable;
            }
            catch (SQLiteException ex)
            {
                 MessageBox.Show(ex.Message);
                return dTable;
            }
        }
    }
}
