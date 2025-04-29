using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SustainableProduct
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            pictureBox6.Paint += pictureBox6_Paint;
            pictureBox6.AllowDrop = true;
            pictureBox6.DragEnter += pictureBox6_DragEnter;
            pictureBox6.DragDrop += pictureBox6_DragDrop;
        }

       



        private void pictureBox6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox6.Image = Image.FromFile(ofd.FileName);
                pictureBox6.Invalidate(); // Paint eventini yeniden çalıştır
            }
        }

        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox6.Image == null)
            {
                string message = "You can add Photo";
                Font font = new Font("Arial", 10, FontStyle.Italic);
                SizeF textSize = e.Graphics.MeasureString(message, font);
                PointF location = new PointF(
                    (pictureBox6.Width - textSize.Width) / 2,
                    (pictureBox6.Height - textSize.Height) / 2
                );

                e.Graphics.DrawString(message, font, Brushes.Gray, location);
            }
        }

        private void pictureBox6_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void pictureBox6_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                try
                {
                    pictureBox6.Image = Image.FromFile(files[0]);
                    pictureBox6.Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Geçersiz resim dosyası: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
