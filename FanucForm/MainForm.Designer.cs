namespace FanucForm
{
    partial class MainForm
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
            this.btnSampling = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RUNNABLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CONNECTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnRefreshView = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSampling
            // 
            this.btnSampling.Location = new System.Drawing.Point(28, 30);
            this.btnSampling.Name = "btnSampling";
            this.btnSampling.Size = new System.Drawing.Size(75, 22);
            this.btnSampling.TabIndex = 1;
            this.btnSampling.Text = "开始采样";
            this.btnSampling.UseVisualStyleBackColor = true;
            this.btnSampling.Click += new System.EventHandler(this.btnSampling_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.SBNO,
            this.IP,
            this.PORT,
            this.RUNNABLE,
            this.CONNECTION,
            this.lastMethod,
            this.lastResult,
            this.lastTime});
            this.dataGridView1.Location = new System.Drawing.Point(28, 70);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(831, 471);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // ID
            // 
            this.ID.FillWeight = 50F;
            this.ID.Frozen = true;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Visible = false;
            this.ID.Width = 50;
            // 
            // SBNO
            // 
            this.SBNO.HeaderText = "设备编号";
            this.SBNO.Name = "SBNO";
            this.SBNO.ReadOnly = true;
            // 
            // IP
            // 
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            this.IP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // PORT
            // 
            this.PORT.HeaderText = "端口";
            this.PORT.Name = "PORT";
            this.PORT.ReadOnly = true;
            this.PORT.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // RUNNABLE
            // 
            this.RUNNABLE.HeaderText = "是否启用";
            this.RUNNABLE.Name = "RUNNABLE";
            this.RUNNABLE.ReadOnly = true;
            this.RUNNABLE.Width = 80;
            // 
            // CONNECTION
            // 
            this.CONNECTION.HeaderText = "连接状态";
            this.CONNECTION.Name = "CONNECTION";
            this.CONNECTION.ReadOnly = true;
            this.CONNECTION.Width = 80;
            // 
            // lastMethod
            // 
            this.lastMethod.HeaderText = "最近请求方法";
            this.lastMethod.Name = "lastMethod";
            this.lastMethod.ReadOnly = true;
            // 
            // lastResult
            // 
            this.lastResult.HeaderText = "最近请求结果";
            this.lastResult.Name = "lastResult";
            this.lastResult.ReadOnly = true;
            // 
            // lastTime
            // 
            this.lastTime.HeaderText = "最近操作时间";
            this.lastTime.Name = "lastTime";
            this.lastTime.ReadOnly = true;
            this.lastTime.Width = 160;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(109, 30);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 22);
            this.btnReport.TabIndex = 3;
            this.btnReport.Text = "开始上报";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(707, 31);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 22);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "刷新配置文件";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnRefreshView
            // 
            this.btnRefreshView.Location = new System.Drawing.Point(190, 30);
            this.btnRefreshView.Name = "btnRefreshView";
            this.btnRefreshView.Size = new System.Drawing.Size(113, 23);
            this.btnRefreshView.TabIndex = 4;
            this.btnRefreshView.Text = "自动刷新视图";
            this.btnRefreshView.UseVisualStyleBackColor = true;
            this.btnRefreshView.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(309, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "视图数据时间：";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(395, 35);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 12);
            this.timeLabel.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 567);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefreshView);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSampling);
            this.Controls.Add(this.btnRefresh);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSampling;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnRefreshView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn PORT;
        private System.Windows.Forms.DataGridViewTextBoxColumn RUNNABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CONNECTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastTime;
    }
}

