using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using UniqueStudio.Common.DatabaseHelper;

namespace UniqueStudio.ExtraTools
{
    public partial class DatabaseExport : Form
    {
        public DatabaseExport()
        {
            InitializeComponent();
        }

        private List<ColumnInfo> columns = new List<ColumnInfo>();
        private string sqlSelect;

        private delegate void OnBuildCallBack(string text);

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string cmdText = "Select Name FROM SysObjects Where XType='U' ORDER BY Name";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(txtConnStr.Text.Trim(),
                                                                            CommandType.Text, cmdText, null))
                {
                    cmbTables.Items.Clear();
                    while (reader.Read())
                    {
                        cmbTables.Items.Add(reader["Name"].ToString());
                    }
                }
                if (cmbTables.Items.Count != 0)
                {
                    cmbTables.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmdText = string.Format("SELECT syscolumns.name ColumnName,systypes.name ColumnType"
                                                            + " FROM syscolumns, systypes"
                                                            + " WHERE syscolumns.xusertype = systypes.xusertype"
                                                            + " AND syscolumns.id = object_id('{0}')", cmbTables.SelectedItem.ToString());
            using (SqlDataReader reader = SqlHelper.ExecuteReader(txtConnStr.Text.Trim(), CommandType.Text, cmdText, null))
            {
                ckbColumns.Items.Clear();
                columns.Clear();
                while (reader.Read())
                {
                    ColumnInfo column = new ColumnInfo();
                    column.ColumnName = reader["ColumnName"].ToString();
                    column.ColumnType = reader["ColumnType"].ToString();
                    columns.Add(column);
                    ckbColumns.Items.Add(column.ColumnName, true);
                }
            }

            cmdText = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.columns"
                            + " WHERE TABLE_NAME='{0}' "
                            + " AND COLUMNPROPERTY(OBJECT_ID('{0}'),COLUMN_NAME,'IsIdentity')=1", cmbTables.SelectedItem.ToString());
            using (SqlDataReader reader = SqlHelper.ExecuteReader(txtConnStr.Text.Trim(), CommandType.Text, cmdText, null))
            {
                string identityColumn;
                int index;
                while (reader.Read())
                {
                    identityColumn = reader["COLUMN_NAME"].ToString();
                    index = ckbColumns.Items.IndexOf(identityColumn);
                    if (index >= 0)
                    {
                        ckbColumns.SetItemChecked(index, false);
                    }
                }
            }
        }

        private class ColumnInfo
        {
            public ColumnInfo()
            {
            }

            public string ColumnName;
            public string ColumnType;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            int j = 0;
            for (int i = 0; i < ckbColumns.Items.Count; i++)
            {
                if (ckbColumns.GetItemChecked(i))
                {
                    sbColumns.Append(string.Format("[{0}],", ckbColumns.Items[i].ToString()));
                    if ("int;smalint;bigint;money;".IndexOf(columns[i].ColumnType) >= 0)
                    {
                        sbValues.Append("{" + j + "},");
                    }
                    else
                    {
                        sbValues.Append("'{" + j + "}',");
                    }
                    j++;
                }
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    sbColumns.Append(string.Format("[{0}],", dataGridView1.Rows[i].Cells[0].Value));
                    if ("int;smalint;bigint;money;".IndexOf((string)dataGridView1.Rows[i].Cells[1].Value) >= 0)
                    {
                        sbValues.Append(dataGridView1.Rows[i].Cells[2].Value + ",");
                    }
                    else
                    {
                        sbValues.Append("'" + dataGridView1.Rows[i].Cells[2].Value + "',");
                    }
                }
            }

            if (sbColumns.Length != 0)
            {
                sbColumns.Remove(sbColumns.Length - 1, 1);
                sbValues.Remove(sbValues.Length - 1, 1);

                sqlSelect = string.Format("SELECT {0} FROM [{1}]", sbColumns.ToString(), cmbTables.SelectedItem.ToString());
                string sqlFormat = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})",
                                                                cmbTables.SelectedItem.ToString(),
                                                                sbColumns.ToString(), sbValues.ToString());
                txtOutput.Text = "正在生成，请稍候...";
                ThreadPool.QueueUserWorkItem(new WaitCallback(Build), sqlFormat);
            }
        }

        private void BuildCallBack(string text)
        {
            if (txtOutput.InvokeRequired)
            {
                OnBuildCallBack d = new OnBuildCallBack(BuildCallBack);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                txtOutput.Text = text;
                try
                {
                    if (chkAutoSave.Checked)
                    {
                        string path = Path.Combine(Environment.CurrentDirectory, cmbTables.SelectedItem.ToString() + ".sql");
                        StreamWriter writer = new StreamWriter(path);
                        writer.Write(text);
                        writer.Flush();
                        writer.Close();
                        MessageBox.Show("已保存到：" + path);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Build(object sqlFormat)
        {
            StringBuilder sqlOutput = new StringBuilder();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(txtConnStr.Text.Trim(), CommandType.Text, sqlSelect, null))
            {
                while (reader.Read())
                {
                    object[] parms = new object[reader.VisibleFieldCount];
                    for (int i = 0; i < reader.VisibleFieldCount; i++)
                    {
                        parms[i] = reader[i].ToString().Replace("'", "''");
                    }
                    sqlOutput.Append(string.Format((string)sqlFormat, parms)).Append(Environment.NewLine);
                }
            }
            BuildCallBack(sqlOutput.ToString());
        }

        private void btnToClipboard_Click(object sender, EventArgs e)
        {
            if (txtOutput.Text.Length == 0)
            {
                MessageBox.Show("呃。。。空的耶!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Clipboard.SetDataObject(txtOutput.Text);
                MessageBox.Show("搞定！");
            }
        }
    }
}
