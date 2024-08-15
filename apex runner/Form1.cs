using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsKey;
using static InputMethod;
using static AltShift;
using System.Diagnostics;
using System.Threading;
using System.IO;
using apex_runner.Properties;
using System.Security.Policy;


namespace apex_runner
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }
        //让窗口可以拖拽
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //获取当前输入法状态并且标识
            if(InputMethod.CurrentMethod() == InputMethodType.Chinese)
            {
                radioButton_chinese.Checked = true;
            }
            else
            {
                radioButton_eng.Checked = true;
            }
            //获取设置中的 uu 加速器路径\语音软件\ Steam 位置
            textBox1.Text = Settings.Default.uupath;
            textBox2.Text = Settings.Default.oopzpath;
            textBox3.Text = Settings.Default.steampath;

        }


        //禁用 Windows 键功能
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                WindowsKey.Disable();
            }
            else
            {
                WindowsKey.Enable();
            }
        }

        //切换英文输入法功能
        private void radioButton_eng_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_eng.Checked == true)
            {
                InputMethod.ChangeToEnglish();
            }
            else
            {
                InputMethod.ChangeToChinese();
            }
        }

        //切换 alt shift 开关功能
        private void radioButtonaltshiftOff_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonaltshiftOff.Checked == true)
            {
                AltShift.Disable();
            }
            else
            {
                AltShift.Enable();
            }
        }

        //手抖多点出来的,懒得删了
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        //一键优化 开启游戏模式
        private void button1_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
            radioButton_eng.Checked = true;
            radioButtonaltshiftOff.Checked = true;
        }

        //一键恢复 开启正常电脑模式
        private void button2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            radioButton_chinese.Checked = true;
            radioButtonaltshiftOn.Checked = true;
        }

        //textbox1 在修改时自动保存加速器路径
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.uupath = textBox1.Text;
            Settings.Default.Save();
        }
        //textbox2 在修改时自动保存路径
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.oopzpath = textBox2.Text;
            Settings.Default.Save();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.steampath = textBox3.Text;
            Settings.Default.Save();
        }
        //右下关于按钮
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show($"版本号: v1.5:黑色模式" +
                $"这个程序是免费的\n如果你感觉这个程序有帮助的话\n" +
                $"我希望你们可以去 github 帮我点星星\n---\n" +
                $"这个程序其实主要都是Chat-GPT 写的\n" +
                $"有时候我得承认\n" +
                $"他写的代码确实比我写的质量好多了\n" +
                $":P");
        }
        
        //右下角链接 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://github.com/cornradio");
        }

        //用于启动加速器的启动函数
        static void StartProgram(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("程序路径不存在或无法启动，请检查路径是否正确。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 创建一个新的进程启动信息
                ProcessStartInfo startInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // 启动程序
                Process process = Process.Start(startInfo);

                // 模拟双击的延迟
                Thread.Sleep(200); // 200毫秒的延迟

                // 再次启动程序
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法启动程序：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //用于开启网页链接的函数
        static void OpenUrl(string url)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法打开URL：" + ex.Message);
            }
        }

        //开启加速器 图片按钮
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            StartProgram(path);
        }
        //开启语音软件
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string path = textBox2.Text;
            StartProgram(path);
        }
        //开起 Steam
        private void pictureBox_steam_Click(object sender, EventArgs e)
        {
            string path = textBox3.Text;
            StartProgram(path);
        }


        //打开显示设置
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开Windows的显示设置
                Process.Start("ms-settings:display");
                Console.WriteLine("显示设置已打开。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法打开显示设置: " + ex.Message);
            }
        }
        //双击开启音频设置
        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            Process.Start("ms-settings:apps-volume");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        //关闭按钮
        private void exitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //点击 apex 大图 启动steam
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        bool IsFloded = true;
        private void button3_Click(object sender, EventArgs e)
        {
            if (IsFloded)
            {
                this.Height = 450;
                IsFloded = false;
                button3.Text= "收起路径设置";                                                         
            }
            else {
                this.Height = 310;
                IsFloded = true;
                button3.Text = "展开路径设置";

            }
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // 焦点聚集在缩小的时候 , 自动切换到 "优化"按钮
        // 以防止一些 bug
        private void button4_Enter(object sender, EventArgs e)
        {
            this.button1.Focus();
        }
    }
}
