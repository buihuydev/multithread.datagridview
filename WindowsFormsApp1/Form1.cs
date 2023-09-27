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
            for(int i = 0; i < 10; i++)
            {
                dataGridView1.Rows.Add($"thread {i}");
            }
        }
        public class ThreadInfo
        {
            public int RowIndex { get; set; }
            public bool IsStopped { get; set; } = false;
        }

        List<ThreadInfo> runningThreads = new List<ThreadInfo>();
        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Không có luồng nào được chọn!");
                return;
            }
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
                    Class1.upload(row,threadInfo);
                });
                thread.Start();
            }
            MessageBox.Show(runningThreads.Count.ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Không có luồng nào được chọn!");
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int threadCount = row.Index;
                ThreadInfo threadInfo = runningThreads.Find(t => t.RowIndex == threadCount);
                if (threadInfo != null)
                {
                    threadInfo.IsStopped = true;
                    runningThreads.Remove(threadInfo);
                }
            }
            MessageBox.Show(runningThreads.Count.ToString());
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
