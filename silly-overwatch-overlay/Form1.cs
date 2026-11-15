using silly_overwatch_overlay.Properties;
using System.Drawing.Imaging;

namespace silly_overwatch_overlay
{
    public partial class Form1 : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_LAYERED = 0x80000;
                const int WS_EX_TRANSPARENT = 0x20;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED;
                cp.ExStyle |= WS_EX_TRANSPARENT;
                return cp;
            }
        }

        BloodSplatter bloodSplatter;
        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;

            TopMost = true;

            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            //bloodSplatter = new GifBox(this, GlobalTimer);
            bloodSplatter = new BloodSplatter(this, SplatterTimer);

            KillTimer.Tick += CheckForKill;

            //bloodSplatter.SplatterBlood();
        }

        public void CheckForKill(object sender, EventArgs e)
        {
            // The kill skull is roughly a square (I measured it being 61x64 pixels on a 2560x1440 display)
            // The skull originates around the centre of the screen

            int width = 61;
            int height = 64;
            int topLeft = this.Bounds.Width / 2 - width / 2;
            int topRight = this.Bounds.Height / 2 - height / 2;

            Rectangle bounds = new Rectangle(topLeft, topRight, width, height);

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                //bitmap.Save("test.jpeg", ImageFormat.Jpeg);

                int count = 0;
                for (int x = 0; x < bounds.Width; x++)
                {
                    for (int y = 0; y < bounds.Height; y++)
                    {
                        Color pixel = bitmap.GetPixel(x, y);
                        int r = pixel.R;
                        int g = pixel.G;
                        int b = pixel.B;

                        //if (Within10Percent(referenceR, r) && Within10Percent(referenceG, g) && Within10Percent(referenceB, b))
                        if (Within10Percent(255, r) && (!Within10Percent(255, g) || !Within10Percent(255, b)))
                        {
                            count++;
                        }
                    }
                }

                if (count >= width * height * 0.8)
                {
                    //bloodSplatter.PlayGif(Image.FromFile("1.gif"), new Point(0, 0), 1000);
                    bloodSplatter.SplatterBlood();
                }
            }
        }

        public bool Within10Percent(int reference, int value)
        {
            return Math.Abs((double) (value - reference) / reference) <= 0.1;
        }
    }
}
