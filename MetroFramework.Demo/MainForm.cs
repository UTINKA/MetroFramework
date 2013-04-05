using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Forms;

namespace MetroFramework.Demo
{

    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void metroTileSwitch_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager.Style = (MetroColorStyle)next;
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroStyleManager.Theme = metroStyleManager.Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
        }

        // I'm deliberately NOT using a Forms timer to have callbacks from the "wrong" thread
        private System.Threading.Timer timer;

        private void EnsureTimer()
        {
             if(timer != null ) return;
            timer = new System.Threading.Timer(Callback);
        }

        private void Callback(object state)
        {
            Random rng = new Random();
            
            MetroStyleManager.Default.Theme = rng.Next(2) > 0 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;

            Array values = Enum.GetValues(typeof(MetroColorStyle));
            object newStyle = values.GetValue(rng.Next(values.Length) + values.GetLowerBound(0));
            MetroStyleManager.Default.Style = (MetroColorStyle)newStyle;
        }


        private void metroToggle4_CheckedChanged(object sender, EventArgs e)
        {
            EnsureTimer();
            long interval = metroToggle4.Checked ? 5000 : -1;
            timer.Change(0, interval);
        }
    }
}
