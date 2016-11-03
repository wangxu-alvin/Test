using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanucSampling;
using FanucSampling.DataStructure;

namespace FanucForm
{
    public partial class DetailForm : Form
    {
        private CNC _cnc;
        private System.Timers.Timer samplingTimer;
        private double _freq;

        public DetailForm(CNC cnc, double freq)
        {
            InitializeComponent();
            _cnc = cnc;
            _freq = freq;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Detail_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            sample(null, null);

            samplingTimer = new System.Timers.Timer(_freq);
            samplingTimer.Elapsed += new System.Timers.ElapsedEventHandler(sample);//到达时间的时候执行事件；
            samplingTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            samplingTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        public void sample(object source, System.Timers.ElapsedEventArgs e)
        {
            Acts acts = _cnc.getActs();
            Coordinate coor = _cnc.getCoordinate();
            Load load = _cnc.getLoad();
            Prog prog = _cnc.getProgram();
            StateInfo si = _cnc.getStateInfo();
            Count count = _cnc.getCount();
            string lastMethod = _cnc.getLastMethod();
            string lastResult = _cnc.getLastResult();
            string lastTime = _cnc.getLastTime();

            if (acts.accept)
            {
                this.labelActs.Text = acts.value + " | " + acts.time;
            }
            if (coor.accept)
            {
                this.labelCoor.Text = "X:" + coor.x + " Y:" + coor.y + " Z:" + coor.z + " | " + coor.time;
            }
            if (load.accept)
            {
                this.labelLoad.Text = load.value + " | " + load.time;
            }
            if (prog.accept)
            {
                this.labelProg.Text = count.value + "," + prog.time + " | " + prog.seq + "," + prog.name + prog.comment;
            }
            if (si.accept)
            {
                //this.labelState.Text = "alarm:" + si.alarm + " emergency:" + si.emergency + " run:" + si.run + " | " + si.time;
                this.labelState.Text = "alarm:" + si.alarm + " run:" + si.run + " | " + si.time;
            }
            this.lableLastMethod.Text = lastMethod;
            this.labelLastResult.Text = lastResult;
            this.labelLastTime.Text = lastTime;

        }

        private void DetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            samplingTimer.Enabled = false;
        }

      
    }
}
