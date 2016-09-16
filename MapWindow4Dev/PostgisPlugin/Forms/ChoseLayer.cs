using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Database.Forms
{
    public partial class ChoseLayer : Form
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Connect2Database frmConnection;

        private string tempPath;

        public ChoseLayer(Connect2Database frmConnect, string tmpPath)
        {
            InitializeComponent();

            log.Debug("ChoseLayer dialog constructing....");
            this.frmConnection = frmConnect;
            this.tempPath = tmpPath;
        }

        private void ChoseLayer_Load(object sender, EventArgs e)
        {

            string strSql;

            switch (frmConnection.DbType)
            {
                
                case "MySQL 5.0":

                    strSql = "select featurename from features features";
                    IDbDataAdapter daMySql = new MySql.Data.MySqlClient.MySqlDataAdapter(strSql, (MySqlConnection)frmConnection.SqlConnection);
                    DataSet dsMySql = new DataSet();
                    daMySql.Fill(dsMySql);
                    DataTable dtMySql = dsMySql.Tables[0];
                    for (int i = 0; i < dtMySql.Rows.Count; i++)
                    {
                        lstAvailbeLayers.Items.Add(dtMySql.Rows[i][0]);
                    }
                    break;

                //case "Oracle 10g":
                //    strSql = "select \"featurename\" from OPENHYDRO.\"features\"";
                //    IDbDataAdapter daOracle = new OracleDataAdapter(strSql, (OracleConnection)frmConnection.SqlConnection);
                //    DataSet dsOracle = new DataSet();
                //    daOracle.Fill(dsOracle);
                //    DataTable dtOracle = dsOracle.Tables[0];
                //    for (int i = 0; i < dtOracle.Rows.Count; i++)
                //    {
                //        lstAvailbeLayers.Items.Add(dtOracle.Rows[i][0]);
                //    }
                //    break;

                case "PostGIS":
                    // strSql = "select f_table_name from geometry_columns";
                    strSql = "select f_table_schema || '.' || f_table_name as f_table_name from geometry_columns";
                    IDbDataAdapter daPgsql = new Npgsql.NpgsqlDataAdapter(strSql, (Npgsql.NpgsqlConnection)frmConnection.SqlConnection);
                    DataSet dsPg = new DataSet();
                    daPgsql.Fill(dsPg);
                    DataTable dtPg = dsPg.Tables[0];
                    for (int i = 0; i < dtPg.Rows.Count; i++)
                    {
                        lstAvailbeLayers.Items.Add(dtPg.Rows[i][0]);
                    }
                    break;
                    default:
                    break;
            }

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            object item = lstAvailbeLayers.SelectedItem;
            if (item == null)
                return;
            lstToAddLayers.Items.Add(item);
            lstAvailbeLayers.Items.RemoveAt(lstAvailbeLayers.SelectedIndex);

        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            int count = lstToAddLayers.Items.Count;
            for (int i = 0; i < count; i++)
            {
                object item = lstToAddLayers.Items[0];
                lstAvailbeLayers.Items.Add(item);
                lstToAddLayers.Items.RemoveAt(0);
            }

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            int count = lstAvailbeLayers.Items.Count;
            for (int i = 0; i < count; i++)
            {
                object item = lstAvailbeLayers.Items[0];
                lstToAddLayers.Items.Add(item);
                lstAvailbeLayers.Items.RemoveAt(0);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            object item = lstToAddLayers.SelectedItem;
            if (item == null)
                return;
            lstAvailbeLayers.Items.Add(item);
            lstToAddLayers.Items.RemoveAt(lstToAddLayers.SelectedIndex);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == tempPath)
                {
                    FolderBrowserDialog dlgFolder = new FolderBrowserDialog();
                    if (dlgFolder.ShowDialog() == DialogResult.OK)
                    {
                        tempPath = dlgFolder.SelectedPath + "\\";
                    }
                    else //No output folder chosen, exit 
                    {
                        return;
                    }
                }
                int count = lstToAddLayers.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
                    string featureName = lstToAddLayers.Items[i].ToString();
                    string strSql = null;

                    switch (frmConnection.DbType)
                    {
                        case "MySQL 5.0":
                            strSql = "select Shape from " + featureName;
                            MySql.Data.MySqlClient.MySqlDataAdapter daMySql = new MySql.Data.MySqlClient.MySqlDataAdapter(strSql, (MySqlConnection)frmConnection.SqlConnection);
                            DataSet dsMySql = new DataSet();
                            daMySql.Fill(dsMySql);
                            DataTable dtMySql = dsMySql.Tables[0];

                            int rowCountMySql = dtMySql.Rows.Count;

                            if (rowCountMySql > 0)
                            {

                                MapWinGIS.Shape shp = new MapWinGIS.Shape();
                                shp.CreateFromString((string)dtMySql.Rows[i][0]);
                                sf.CreateNew(tempPath + featureName + ".shp", shp.ShapeType);
                                sf.StartEditingShapes(true, null);
                                int stupidx = 0;
                                sf.EditInsertShape(shp, ref stupidx);

                                for (int j = 1; j < rowCountMySql; j++)
                                {
                                    MapWinGIS.Shape newShp = new MapWinGIS.Shape();
                                    newShp.CreateFromString((string)dtMySql.Rows[j][0]);
                                    int veryStupidRefForTheFunEditInsertShape = sf.NumShapes;
                                    sf.EditInsertShape(newShp, ref veryStupidRefForTheFunEditInsertShape);
                                }
                                sf.StopEditingShapes(true, true, null);
                            }
                            break;
                        //case "Oracle 9i":

                        //    break;
                        //case "Oracle 10g":
                        //    strSql = "select \"Shape\" from OPENHYDRO." + "\"" + featureName + "\"";
                        //    OracleDataAdapter daOra = new OracleDataAdapter(strSql, (OracleConnection)frmConnection.SqlConnection);
                        //    DataSet dsOra = new DataSet();
                        //    daOra.Fill(dsOra);
                        //    DataTable dtOra = dsOra.Tables[0];

                        //    int rowCountOra = dtOra.Rows.Count;

                        //    if (rowCountOra > 0)
                        //    {

                        //        MapWinGIS.Shape shp = new MapWinGIS.Shape();
                        //        shp.CreateFromString((string)dtOra.Rows[i][0]);
                        //        sf.CreateNew(tempPath + featureName + ".shp", shp.ShapeType);
                        //        sf.StartEditingShapes(true, null);
                        //        int stupidx = 0;
                        //        sf.EditInsertShape(shp, ref stupidx);

                        //        for (int j = 1; j < rowCountOra; j++)
                        //        {
                        //            MapWinGIS.Shape newShp = new MapWinGIS.Shape();
                        //            newShp.CreateFromString((string)dtOra.Rows[j][0]);
                        //            int veryStupidRefForTheFunEditInsertShape = sf.NumShapes;
                        //            sf.EditInsertShape(newShp, ref veryStupidRefForTheFunEditInsertShape);
                        //        }
                        //        sf.StopEditingShapes(true, true, null);
                        //    }

                        //    break;
                        case "PostGIS":

                            // strSql = "select f_table_name from geometry_columns";
                            strSql = "select f_table_schema || '.' || f_table_name as f_table_name from geometry_columns";
                            IDbDataAdapter daPgsql = new Npgsql.NpgsqlDataAdapter(strSql, (Npgsql.NpgsqlConnection)frmConnection.SqlConnection);
                            DataSet dsPg = new DataSet();
                            daPgsql.Fill(dsPg);
                            DataTable dtPg = dsPg.Tables[0];

                            //store user name and password information for future usage:
                            string username = frmConnection.Username;
                            string password = frmConnection.Password;
                            string dbname = frmConnection.DbName;
                            string hostname = frmConnection.HostName;
                            string portnumber = frmConnection.PortNumber;

                            for (int x = 0; x < dtPg.Rows.Count; x++)
                            {
                                //Export all the features into shapefiles and then open them in MapWindow:
                                string fName = (string)dtPg.Rows[x][0];
                                string strCmd = " -h " + hostname + " -p " + portnumber + " -u " + username + " -P " + password + " -f " + tempPath + fName + ".shp" + " " + dbname + " " + fName;
                                MessageBox.Show(strCmd);
                                if (lstToAddLayers.Items.Contains(fName))
                                {
                                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                                    info.FileName = "pgsql2shp.exe ";
                                    info.Arguments = strCmd;
                                    info.CreateNoWindow = true;
                                    info.UseShellExecute = false;
                                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                                    proc.StartInfo = info;
                                    proc.Start();
                                    proc.WaitForExit();
                                }

                            }

                            break;

                        default:
                            break;

                    }

                }
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}