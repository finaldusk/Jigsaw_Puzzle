using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Collections;

namespace pintu
{
    public partial class Main_Game : Form
    {

        public PictureBox[] PicBlock = new PictureBox[49];
        int GameSize;
        int MAP_WIDTH = 400;
        int FirstBlock, SecondBlock;
        bool isSwap;
        int[] Position = new int[49];
        Bitmap Source;
        string filename;


        private void SaveBmp()
        {
            Graphics g;
            g = Graphics.FromImage(Source);
            g.DrawImage(Image.FromFile(filename), 0, 0, MAP_WIDTH, MAP_WIDTH);
        }

        private void init(int n)
        {
            Random rdm;
            ArrayList a1 = new ArrayList();
            int t = 0;
            rdm = new Random();
            while (a1.Count < n * n)
            {
                t = rdm.Next(0, n * n);
                if ((!a1.Contains(t)))
                {
                    a1.Add(t);
                }
            }
            for (t = 0; t <= a1.Count - 1; t++)
            {
                Position[t] = Convert.ToInt16(a1[t]);
            }

        }

        private void swap(object sender,System.EventArgs e)
        {
            PictureBox bClick = (PictureBox)sender;
            int i = 0;
            Image temp;
            if (!isSwap)
            {
                isSwap = true;
                FirstBlock = Convert.ToInt16(bClick.Tag);
            }
            else
            {
                label1.Text = "";
                SecondBlock = Convert.ToInt16(bClick.Tag);
                temp = PicBlock[SecondBlock].Image;
                PicBlock[SecondBlock].Image = PicBlock[FirstBlock].Image;
                PicBlock[FirstBlock].Image = temp;

                isSwap = false;

                i = Position[SecondBlock];
                Position[SecondBlock] = Position[FirstBlock];
                Position[FirstBlock] = i;

                foreach(int s in Position)
                {
                    label1.Text = label1.Text + Position[s].ToString();
                }
                if (CheckWin())
                {
                    if (GameSize < 7)
                    {
                        MessageBox.Show("恭喜过关", "提示");
                        GameSize = GameSize + 1;
                        comboBox1.Text = GameSize.ToString();
                        label2.Text = comboBox1.Text;
                        gamestart();
                    }
                    if (GameSize == 7)
                    {
                        MessageBox.Show("恭喜通关，请选择一张新的图片重新开始", "提示");
                    }
                   
                }
          }
        }

        private bool CheckWin()
        {
            
            for(int t = 0; t < GameSize* GameSize; t++)//修正书上例题bug
            {
                if (Position[t] != t)
                {
                    return false;
                }
            }
            return true;
        }

        private Bitmap create_image(int n)
        {
            int W = 0;
            W = MAP_WIDTH / GameSize;
            Bitmap bit = new Bitmap(W, W);
            Graphics g = Graphics.FromImage(bit);
            Rectangle a = new Rectangle(0, 0, W, W);
            Rectangle b = new Rectangle((n % GameSize) * W, n / GameSize * W, W, W);
            g.DrawImage(Source, a, b, GraphicsUnit.Pixel);
            return bit;
        }


        public Main_Game()
        {
            InitializeComponent();
        }

        private void btn_LoadIMG_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();//打开打开文件窗口
            if (!string.IsNullOrEmpty(openFileDialog1.FileName))//非空则加载图片
            {
                filename = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(filename);
                SaveBmp();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            GameSize = 3;
            init(GameSize);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            GameSize = 4;
            init(GameSize);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            GameSize = 5;
            init(GameSize);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            GameSize = 6;
            init(GameSize);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            GameSize = 7;
            init(GameSize);
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            gamestart();
        }

        private void gamestart()
        {
            //卸载上次的图片块 
            int i = 0;
            int BWidth = 0;
            bool flag = true;
            while (this.Controls.Count > 0 & flag == true)
            {
                flag = false;
                foreach (Control a in this.Controls)
                {
                    if (a.Name.Length > 8)
                    {
                        if (a.Name.Substring(0, 8) == "PicBlock")
                        {
                            this.Controls.Remove(a);
                            flag = true;
                            i = i + 1;
                        }
                    }
                }
            }
            init(GameSize);     //重新加载图片块 
            BWidth = MAP_WIDTH / GameSize;
            for (i = 0; i <= GameSize * GameSize - 1; i++)
            {
                PicBlock[i] = new PictureBox();
                this.Controls.Add(PicBlock[i]);
                PicBlock[i].Left = 160 + BWidth * (i % GameSize);//生成拼图左侧与窗体左侧的距离
                PicBlock[i].Top = BWidth * (int)(i / GameSize) + 20;//生成拼图上侧与窗体上侧的距离
                PicBlock[i].Width = BWidth;
                PicBlock[i].Height = BWidth;
                PicBlock[i].Name = "PicBlock" + i.ToString();
                PicBlock[i].Tag = i;
                PicBlock[i].Image = create_image(Position[i]);
                PicBlock[i].BorderStyle = BorderStyle.FixedSingle;
                //PicBlock[i].BringToFront() 
                ((PictureBox)PicBlock[i]).Click += swap;
            }
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            btn_Start.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String level = comboBox1.Text;
            label2.Text = level;
            GameSize = Convert.ToInt32(level);
        }

        private void Main_Game_Load(object sender, EventArgs e)
        {
            /*
            filename = Application.StartupPath + "\\temp.bmp";
            pictureBox1.Image = Image.FromFile(filename);
            */
            isSwap = false;
            Source = new Bitmap(MAP_WIDTH, MAP_WIDTH);
            GameSize = 3;
            init(GameSize);

            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            comboBox1.Items.Add("6");
            comboBox1.Items.Add("7");
            comboBox1.Text = "3";
            //SaveBmp();

        }
    }
}
