namespace UniqueStudio.ExtraTools
{
    partial class DatabaseExport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnStr = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ckbColumns = new System.Windows.Forms.CheckedListBox();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnToClipboard = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接字符串：";
            // 
            // txtConnStr
            // 
            this.txtConnStr.Location = new System.Drawing.Point(98, 10);
            this.txtConnStr.Name = "txtConnStr";
            this.txtConnStr.Size = new System.Drawing.Size(599, 20);
            this.txtConnStr.TabIndex = 1;
            this.txtConnStr.Text = "server=(local)\\sqlexpress;database=UniqueStudioCMS;User ID=sa;Password=P@ssw0rd";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(271, 41);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(78, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "表：";
            // 
            // ckbColumns
            // 
            this.ckbColumns.CheckOnClick = true;
            this.ckbColumns.FormattingEnabled = true;
            this.ckbColumns.Location = new System.Drawing.Point(16, 74);
            this.ckbColumns.Name = "ckbColumns";
            this.ckbColumns.Size = new System.Drawing.Size(231, 259);
            this.ckbColumns.TabIndex = 6;
            // 
            // cmbTables
            // 
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new System.Drawing.Point(50, 43);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(197, 21);
            this.cmbTables.TabIndex = 7;
            this.cmbTables.SelectedIndexChanged += new System.EventHandler(this.cmbTables_SelectedIndexChanged);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(253, 74);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(484, 438);
            this.txtOutput.TabIndex = 8;
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(368, 41);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 23);
            this.btnBuild.TabIndex = 9;
            this.btnBuild.Text = "生成";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnToClipboard
            // 
            this.btnToClipboard.Location = new System.Drawing.Point(465, 41);
            this.btnToClipboard.Name = "btnToClipboard";
            this.btnToClipboard.Size = new System.Drawing.Size(108, 23);
            this.btnToClipboard.TabIndex = 10;
            this.btnToClipboard.Text = "复制到剪贴板";
            this.btnToClipboard.UseVisualStyleBackColor = true;
            this.btnToClipboard.Click += new System.EventHandler(this.btnToClipboard_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column,
            this.Type,
            this.Value});
            this.dataGridView1.Location = new System.Drawing.Point(16, 361);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(231, 143);
            this.dataGridView1.TabIndex = 11;
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Checked = true;
            this.chkAutoSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoSave.Location = new System.Drawing.Point(594, 45);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(74, 17);
            this.chkAutoSave.TabIndex = 12;
            this.chkAutoSave.Text = "自动保存";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 345);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "附加字段：";
            // 
            // Column
            // 
            this.Column.HeaderText = "字段";
            this.Column.Name = "Column";
            // 
            // Type
            // 
            this.Type.HeaderText = "类型";
            this.Type.Items.AddRange(new object[] {
            "int",
            "smallint",
            "bigint",
            "char",
            "varchar",
            "nvarchar",
            "ntext",
            "uniqueidentifier"});
            this.Type.Name = "Type";
            // 
            // Value
            // 
            this.Value.HeaderText = "值";
            this.Value.Name = "Value";
            // 
            // DatabaseExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 516);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkAutoSave);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnToClipboard);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.cmbTables);
            this.Controls.Add(this.ckbColumns);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtConnStr);
            this.Controls.Add(this.label1);
            this.Name = "DatabaseExport";
            this.Text = "数据库数据导出工具";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnStr;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox ckbColumns;
        private System.Windows.Forms.ComboBox cmbTables;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnToClipboard;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkAutoSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Label label2;
    }
}