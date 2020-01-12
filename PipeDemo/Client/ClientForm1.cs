using System;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientForm1 : Form
    {
        private readonly RichTextBox _richTextBox;

        public ClientForm1()
        {
            InitializeComponent();
            Button button = new Button
            {
                Location = new Point(10, 20),
                Size = new Size(100, 24),
                Text = @"连接服务"
            };
            button.MouseClick += ButtonOnMouseClick;
            Controls.Add(button);

            _richTextBox = new RichTextBox
            {
                Location = new Point(button.Bounds.Left, button.Bottom + 10),
                Size = new Size(300, 200)
            };
            Controls.Add(_richTextBox);
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(400, 300);
        }

        private void ButtonOnMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            _richTextBox.AppendText("开始连接服务 " + DateTime.Now);
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "myPipe", PipeDirection.InOut))
            {
                _richTextBox.AppendText("\n开始连接 myPipe " + DateTime.Now);
                pipeClient.Connect();
                Thread.Sleep(2000);
                try
                {
                    using (StreamReader streamReader = new StreamReader(pipeClient))
                    {
                        _richTextBox.AppendText("\n开始读取服务端信息 " + DateTime.Now);
                        while (true)
                        {
                            string text = streamReader.ReadLine();
                            _richTextBox.AppendText("\n" + text);
                            if (string.IsNullOrEmpty(text)) { break;}
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}