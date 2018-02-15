using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Graphic_editor
{
    public partial class Editor : Form
    {
        //Переменные
        Graphics g;                 //Графический элемент
        bool isPressed;             //Нажатие клавиши мыши
        int x1, y1, x2, y2;         //Координаты мыши
        Bitmap snapshot, tempDraw, image;  //Снимки экрана
        Color foreColor;            //Цвет
        int lineWidth;              //Ширина линии
        string selectedTool;        //Выбранный инструмент 
        string smooth = "None";     //Сглаживание

        public Editor()
        {
            InitializeComponent();
            snapshot = new Bitmap(pictureBox.ClientRectangle.Width, pictureBox.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            foreColor = Color.Black;
            lineWidth = 5;
        }

        private void Line_Click(object sender, EventArgs e)
        {

        }

        private void Rectangle_Click(object sender, EventArgs e)
        {

        }

        private void Pencil_Click(object sender, EventArgs e)
        {
            
        }

        private void создатьToolStripButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Enabled == true)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                if (MessageBox.Show("Сохранить изменения?", "", buttons) == System.Windows.Forms.DialogResult.Yes) ;
                {
                    SavePicture();
                }
            }
            pictureBox.Enabled = true;
            pictureBox.Invalidate();
            MessageBox.Show("Рисунок создан");
        }

        private void открытьToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if(tempDraw != null)
            {
                Graphics g = Graphics.FromImage(tempDraw);
                Pen myPen = new Pen(foreColor, lineWidth);
                g.DrawLine(myPen,x1,y1,x2,y2);
                myPen.Dispose();
                x1 = x2;
                y1 = y2;
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            x1 = e.X;
            y1 = e.Y;
            tempDraw = (Bitmap)snapshot.Clone();
        }


        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            g = pictureBox.CreateGraphics();
            if(isPressed)
            {
                pictureBox.Update();
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
            snapshot = (Bitmap)tempDraw.Clone();
            x2 = e.X;
            y2 = e.Y;
            g.DrawLine(new Pen(Brushes.Black, 4), x1, y1, x2, y2);
        }

        private void SavePicture()
        {
            string filename;
            image = (Bitmap)pictureBox.Image;
            saveFileDialog.Title = "Сохранить изображение как...";
            saveFileDialog.FileName = "MyPicture";
            saveFileDialog.Filter = "JPG (*.jpg)";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filename = saveFileDialog.FileName;
                    image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            
        }
    }
}
