using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.Form1;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows.Add(5);
        }
        public class ThreadInfo
        {
            public int RowIndex { get; set; }
            public bool IsStopped { get; set; } = false;
        }

        List<ThreadInfo> runningThreads = new List<ThreadInfo>();
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                Thread thread = new Thread(() =>
                {
                    int threadCount = row.Index;
                    if (row.Cells[1].Value != null)
                    {
                        if (row.Cells[1].Value.ToString() != "đã dừng")
                        {
                            ThreadInfo threadInfo1 = runningThreads.Find(t => t.RowIndex == threadCount);
                            if (threadInfo1 != null)
                            {
                                threadInfo1.IsStopped = true;
                                runningThreads.Remove(threadInfo1);
                                row.Cells[1].Value = "Đã kết thúc luồng cũ";
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    ThreadInfo threadInfo = new ThreadInfo { RowIndex = threadCount };
                    lock (runningThreads)
                    {
                        runningThreads.Add(threadInfo);
                    }
                    while (!threadInfo.IsStopped)
                    {
                        dataGridView1.Invoke((MethodInvoker)delegate
                        {
                            row.Cells[1].Value = DateTime.Now;
                        });
                        Thread.Sleep(1000);
                    }
                    lock (runningThreads)
                    {
                        runningThreads.Remove(threadInfo);
                    }
                });
                thread.Start();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Dừng luồng chỉ định
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int threadCount = row.Index;
                ThreadInfo threadInfo = runningThreads.Find(t => t.RowIndex == threadCount);
                if (threadInfo != null)
                {
                    threadInfo.IsStopped = true;
                    row.Cells[1].Value = "đã dừng";
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(runningThreads.Count > 0)
            {
                MessageBox.Show("Vui lòng đợi các luồng kết thúc công việc!", "Thông báo");
                foreach (ThreadInfo th in runningThreads)
                {
                    th.IsStopped = true;
                }
            }
           
        }
    }
}
