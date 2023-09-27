using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static WindowsFormsApp1.Form1;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Class1
    {
        public static  void upload(DataGridViewRow row, ThreadInfo threadInfo)
        {

            while (threadInfo.IsStopped == false)
            {
                row.Cells[1].Value = DateTime.Now;
                Thread.Sleep(1000);
            }
            row.Cells[1].Value = "stopped";
        }
    }
}
