using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMeasurement
{
    public partial class Transparent_Form : Form
    {
        int num;

        public Transparent_Form(int count)
        {
            InitializeComponent();
            num = count;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            using (SolidBrush myBrush = new SolidBrush(Color.Red))
            {
                g.FillRectangle(myBrush, new Rectangle(0, 0, 5, 5));

                using (Font drawFont = new Font("Arial", 16))
                {
                    if (num > 0)
                    {
                        g.DrawString(Convert.ToString(num), drawFont, myBrush, new PointF(8, 8));
                    }

                }


            }

            g.Dispose();
        }
    }
}
