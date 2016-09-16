using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Database.Forms
{
    public partial class Connect2Database : Form
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string username = null;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string pwd = null;

        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }

        private string dbname = null;
        public string DbName
        {
            get { return dbname; }
            set { dbname = value; }
        }

        private string portnumber = null;
        public string PortNumber
        {
            get { return portnumber; }
            set { portnumber = value; }
        }

        private string hostname = null;
        public string HostName
        {
            get { return hostname; }
            set { hostname = value; }
        }
        
        private string dbType = null;

        public string DbType
        {
            get { return dbType; }
        }
        private bool isConnected = false;

        public bool IsConnected
        {
            get { return isConnected; }
        }
        private IDbConnection sqlConnection;

        public IDbConnection SqlConnection
        {
            get { return sqlConnection; }
        }
  

        public Connect2Database()
        {
            InitializeComponent();
        }

        private void Connect2Database_Load(object sender, EventArgs e)
        {
            cbDbType.SelectedIndex = 0;
            port.Text = "5432";
            hostName.Text = "emipc183-fwr.rivm.nl";
            userName.Text = "boassonw";
            password.Text = "";
            database.Text = "gdb";
        }

        private void cbDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbDbType.SelectedItem.ToString())
            {
                case "MySQL 5.0":
                    port.Text = "3306";
                    break;
                case "Oracle 9i":
                    port.Text = "1521";
                    break;
                case "Oracle 10g":
                    port.Text = "1521";
                    break;
                case "PostGIS":
                    port.Text = "5432";
                    database.Text = "postgis";
                    userName.Text = "";
                    password.Text = "";
                    break;

                default:
                    break;
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            MySqlConnection mysqlConnection = null;
            //OracleConnection oraConnection = null;
            Npgsql.NpgsqlConnection npgConnection = null;
            try
            {
                switch (cbDbType.SelectedItem.ToString())
                {
                    case "MySQL 5.0":
                        mysqlConnection = new MySqlConnection("Server=" + hostName.Text + ";UID=" + userName.Text + ";PWD=" + password.Text + ";Database=" + database.Text + ";Port=" + port.Text + ";");
                        mysqlConnection.Open();
                        break;
                    //case "Oracle 9i":
                    //    port.Text = "1521";
                    //    break;
                    //case "Oracle 10g":
                    //    oraConnection = new OracleConnection();
                    //    oraConnection.ConnectionString = "user id=" + this.userName.Text + ";password=" + this.password.Text + ";data source=//" + this.hostName.Text + ":" + this.port.Text + "/orcl";
                    //    oraConnection.Open();
                    //    break;
                    case "PostGIS":
                        npgConnection = new NpgsqlConnection("Server=" + hostName.Text + ";UID=" + userName.Text + ";PWD=" + password.Text + ";Database=" + database.Text + ";Port=" + port.Text + ";");
                        npgConnection.Open();
                        break;
                    default:
                        break;
                }
                MessageBox.Show("Connection is established successfullly");
             }
            catch (Exception ex)
            {
                MessageBox.Show("A error occurred when trying to connect to the database, please refer to the log file for more detials");
                log.Error("An error happened when trying to connect to the database:" + cbDbType.SelectedItem.ToString() + " " + ex.ToString());
            }
            finally //clean up
            {
                if (mysqlConnection != null)
                    mysqlConnection.Close();
                //if (oraConnection != null)
                //    oraConnection.Close();
                if (npgConnection != null)
                    npgConnection.Close();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            MySqlConnection mysqlConnection = null;
            //OracleConnection oraConnection = null;
            Npgsql.NpgsqlConnection npgConnection = null;
            this.Username = userName.Text;
            this.Password = password.Text;
            this.DbName = database.Text;

            try
            {
                switch (cbDbType.SelectedItem.ToString())
                {
                    case "MySQL 5.0":
                        mysqlConnection = new MySqlConnection("Server=" + hostName.Text + ";UID=" + userName.Text + ";PWD=" + password.Text + ";Database=" + database.Text + ";Port=" + port.Text + ";");
                        mysqlConnection.Open();
                        this.dbType = "MySQL 5.0";
                        this.isConnected = true;
                        this.sqlConnection = mysqlConnection;
                        break;
                    //case "Oracle 9i":
                    //    port.Text = "1521";
                    //    break;
                    //case "Oracle 10g":
                    //    oraConnection = new OracleConnection();
                    //    oraConnection.ConnectionString = "user id=" + this.userName.Text + ";password=" + this.password.Text + ";data source=//" + this.hostName.Text + ":" + this.port.Text + "/orcl";
                    //    oraConnection.Open();
                    //    this.dbType = "Oracle 10g";
                    //    this.isConnected = true;
                    //    this.sqlConnection = oraConnection;
                    //    break;
                    case "PostGIS":
                        npgConnection = new NpgsqlConnection("Server=" + hostName.Text + ";UID=" + userName.Text + ";PWD=" + password.Text + ";Database=" + database.Text + ";Port=" + port.Text + ";");
                        npgConnection.Open();
                        this.DbName = database.Text;
                        this.portnumber = port.Text;
                        this.hostname = hostName.Text;
                        this.dbType = "PostGIS";
                        this.isConnected = true;
                        this.sqlConnection = npgConnection;
                        break;
                    default:
                        break;
                }
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A error occurred when trying to connect to the database, please refer to the log file for more detials");
                log.Error("An error happened when trying to connect to the database:" + cbDbType.SelectedItem.ToString() + " " + ex.ToString());
            }
        }

    }
}