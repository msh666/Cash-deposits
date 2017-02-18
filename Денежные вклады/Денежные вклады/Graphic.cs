using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Денежные_вклады
{
    public partial class Graphic : Form
    {
        public Graphic(int[]mas)
        {
            InitializeComponent();
            this.mas = mas;
        }
        int grids = 12;
        int scale = 0;
        int gridsH = 10;
        int scaleH = 0;
        private void Graphic_Load(object sender, EventArgs e)
        {
            panel1.Height = 500;
            panel1.Width = 600;
        }

        public void StartGraph()
        {
            Graphics graph = panel1.CreateGraphics();
            Pen bold_pen = new Pen(Brushes.Black, 5);
            Pen middle_pen = new Pen(Brushes.Black, 7);
            Pen think_pen = new Pen(Brushes.Black, 1);
            Pen draw_pen = new Pen(Brushes.Red, 3);



            scale = panel1.Width / grids;
            scaleH = panel1.Height / gridsH;

            Point X0Y0 = new Point(panel1.Width / 2, panel1.Height / 2);


            graph.DrawLine(bold_pen, new Point(0, 0), new Point(0, panel1.Height));

            graph.DrawLine(middle_pen, new Point(0, panel1.Height), new Point(panel1.Width, panel1.Height));
            for (int i = 0; i <= panel1.Height; i++)
            {
                graph.DrawLine(think_pen, new Point(0, i * scaleH), new Point(panel1.Width, i * scaleH));
            }
            for (int i = 0; i <= panel1.Width; i++)
            {
                graph.DrawLine(think_pen, new Point(i * scale, 0), new Point(i * scale, panel1.Height));
            }
            int maxvalue = mas.Max();
            label13.Text = maxvalue.ToString();
            scaleH = panel1.Height / maxvalue;  // Вместо числа, максимальное в массиве
            for (int i = 1; i < 12; i++)
            {
                graph.DrawLine(draw_pen, new Point((i) * scale, panel1.Height - mas[i-1] * scaleH), new Point((i+1) * scale, panel1.Height - mas[i] * scaleH));
            }

            panel1.Width = 601;
        }
        int[] mas = null;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            StartGraph();
        }
    }
}
