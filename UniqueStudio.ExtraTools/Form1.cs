﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.ExtraTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
                webBrowser1.Url = new Uri(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cmdText = "SELECT Config FROM [uniqueCMS_Sites] WHERE SiteID=@SiteID";
            SqlParameter parm = new SqlParameter("@SiteID", Convert.ToInt32(txtSiteId.Text));
            object o = SqlHelper.ExecuteScalar(txtConnnectionString.Text, CommandType.Text, cmdText, parm);
            if (o != null && o != DBNull.Value)
            {
                try
                {
                    XmlManager.SaveXml(Environment.CurrentDirectory + "/temp.xml", (string)o);
                    webBrowser1.Url = new Uri(Environment.CurrentDirectory + "/temp.xml");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int siteId = Convert.ToInt32(txtSiteId.Text);
            string content = Common.FileAccessHelper.FileAccess.ReadFile(txtFileName.Text);
            string cmdText = "UpdateWebSiteConfig";
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteId",siteId),
                                                    new SqlParameter("@Config",content)};
            if (SqlHelper.ExecuteNonQuery(txtConnnectionString.Text, CommandType.StoredProcedure, cmdText, parms) > 0)
            {
                MessageBox.Show("ok");
            }
        }
    }
}
