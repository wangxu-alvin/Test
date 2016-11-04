using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using FanucSampling;
using FanucSampling.Configuration;
using com.isesol.ismes.platform.api.sdk;
using System.Diagnostics;
using FanucSampling.DataStructure;
using System.Threading;

namespace FanucForm
{
    public partial class MainForm : Form
    {
        
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<int, CNC> list;
        private System.Timers.Timer viewTimer;
        private System.Timers.Timer reportTimer;
        private WisSDK sdk;

        public MainForm()
        {
            InitializeComponent();
            list = new Dictionary<int, CNC>();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Conf.load();
            sdk = new WisSDK();
            reloadCNCs();
            log.Info("窗口已经启动");
            viewTimer = new System.Timers.Timer(Conf.freq.refreshView);
            viewTimer.Elapsed += new System.Timers.ElapsedEventHandler(refreshView);//到达时间的时候执行事件；
            viewTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            viewTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            reportTimer = new System.Timers.Timer(Conf.freq.report);
            reportTimer.Elapsed += new System.Timers.ElapsedEventHandler(report);//到达时间的时候执行事件；
            reportTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            reportTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；
        }

        bool isReporting = false;
        public void report(object source, System.Timers.ElapsedEventArgs e)
        {
            if (isReporting)
            {
                // 如果上次触发的采样未结束，本次采样将被放弃
                return;
            }
            isReporting = true;
            Stopwatch sw = new Stopwatch();
            foreach (var cnc in list)
            {
                if (cnc.Value._conf.run)
                {
                    sw.Start();
                    sdk.report(cnc.Value);
                    //sdk.reportDummy();
                    sw.Stop();
                    log.Debug("[" + cnc.Value._conf.index + "]上报耗时：" + sw.Elapsed);
                }
            }
            isReporting = false;
        }

        public void refreshView(object source, System.Timers.ElapsedEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string id = dataGridView1.Rows[i].Cells["ID"].Value.ToString();
                CNC cnc = list[int.Parse(id)];
                dataGridView1.Rows[i].Cells["lastMethod"].Value = cnc.getLastMethod();
                dataGridView1.Rows[i].Cells["lastResult"].Value = cnc.getLastResult();
                dataGridView1.Rows[i].Cells["lastTime"].Value = cnc.getLastTime();
                string connection;
                if (cnc.getConnection() == FanucSampling.DataStructure.Connection.CONNECTED)
                {
                    connection = "已连接";
                }
                else if (cnc.getConnection() == FanucSampling.DataStructure.Connection.CONNECTING)
                {
                    connection = "连接中";
                }
                else
                {
                    connection = "未连接";
                }
                dataGridView1.Rows[i].Cells["CONNECTION"].Value = connection;
            }
            this.timeLabel.Text = DateTime.Now.ToString();
        }

        private void reloadCNCs()
        {
            this.dataGridView1.Rows.Clear();
            releaseCNCs();
            foreach (CNCConf conf in Conf.cncs)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells["ID"].Value = conf.index;
                this.dataGridView1.Rows[index].Cells["SBNO"].Value = conf.sbNo;
                this.dataGridView1.Rows[index].Cells["IP"].Value = conf.ip;
                this.dataGridView1.Rows[index].Cells["PORT"].Value = conf.port;
                this.dataGridView1.Rows[index].Cells["RUNNABLE"].Value = conf.run ? "是":"否";
                CNC cnc = new CNC(conf);
                cnc.disconnectionHandler += sdk.reportDisconnect;
                list.Add(conf.index, cnc);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (viewTimer.Enabled || reportTimer.Enabled)
            {
                MessageBox.Show("请先停止“采样”、“上报”以及“刷新视图”");
                return;
            }
            Conf.load();
            reloadCNCs();
            MessageBox.Show("配置文件刷新成功");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (viewTimer.Enabled || reportTimer.Enabled || isSampling)
            {
                MessageBox.Show("请先停止“采样”、“上报”以及“刷新视图”");
                e.Cancel = true;
                return;
            }
            releaseCNCs();
        }

        private void releaseCNCs()
        {
            foreach (var cnc in list)
            {
                cnc.Value.release();
            }
            list.Clear();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (reportTimer.Enabled)
            {
                this.btnReport.Text = "开始上报";
            }
            else
            {
                this.btnReport.Text = "停止上报";
            }
            reportTimer.Enabled = !reportTimer.Enabled;
        }

        public static bool isSampling = false;
        private void btnSampling_Click(object sender, EventArgs e)
        {
            if (!isSampling)
            {
                this.btnSampling.Text = "停止采样";
                isSampling = true;
                Thread thread = new Thread(new ThreadStart(MainForm.sample));
                thread.Start();
            }
            else
            {
                this.btnSampling.Text = "开始采样";
                isSampling = false;
            }
        }

        public static void sample()
        {
            log.Debug("开始采样");
            Stopwatch sw = new Stopwatch();
            while (true)
            {
                if (!isSampling)
                {
                    log.Debug("停止采样");
                    return;
                }
                foreach (var cnc in list)
                {
                    if (!isSampling)
                    {
                        log.Debug("停止采样");
                        return;
                    }
                    if (cnc.Value._conf.run)
                    {
                        sw.Start();
                        cnc.Value.sample();
                        sw.Stop();
                        log.Debug("[" + cnc.Value._conf.index + "]采样耗时：" + sw.Elapsed);
                    }
                    if (!isSampling)
                    {
                        log.Debug("停止采样");
                        return;
                    }
                }
                Thread.Sleep(Conf.freq.sampling);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (viewTimer.Enabled)
            {
                this.btnRefreshView.Text = "自动刷新视图";
            }
            else
            {
                this.btnRefreshView.Text = "停止刷新视图";
            }
            viewTimer.Enabled = !viewTimer.Enabled;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = int.Parse(this.dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
            DetailForm df = new DetailForm(list[id], Conf.freq.sampling);
            df.ShowDialog();
        }
    }
}
