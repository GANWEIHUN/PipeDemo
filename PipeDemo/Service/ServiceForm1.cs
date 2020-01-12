using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Windows.Forms;

namespace Service
{
    public partial class ServiceForm1 : Form
    {
        private readonly RichTextBox _richTextBox;

        public ServiceForm1()
        {
            InitializeComponent();
            Button button = new Button
            {
                Location = new Point(10, 20),
                Size = new Size(100, 24),
                Text = @"启动服务"
            };
            button.MouseClick += ButtonOnMouseClick;
            Controls.Add(button);

            _richTextBox = new RichTextBox
            {
                Location = new Point(button.Bounds.Left, button.Bottom + 10),
                Size = new Size(300, 200)
            };
            Controls.Add(_richTextBox);
            _richTextBox.AppendText("我是服务端");
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(400, 300);
        }

        private void ButtonOnMouseClick(object sender, MouseEventArgs e)
        {
            _richTextBox.AppendText("\n启动管道服务 myPipe " + DateTime.Now);
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("myPipe", PipeDirection.InOut))
            {
                pipeServer.WaitForConnection();
                _richTextBox.AppendText("\n客户端连接成功 " + DateTime.Now);
                Trace.WriteLine("客户端连接成功");
                //Thread.Sleep(1000);
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(pipeServer))
                    {
                        Trace.WriteLine("CanWrite" + pipeServer.CanWrite);
                        streamWriter.AutoFlush = true;
                        Trace.WriteLine("开始发送消息 ");
                        streamWriter.Write("我是服务端 myPipe");
                        streamWriter.WriteLine();
                        streamWriter.Write(Process.GetCurrentProcess());
                        streamWriter.WriteLine();
                        streamWriter.Write("最后一行");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}