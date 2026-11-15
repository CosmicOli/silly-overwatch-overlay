using silly_overwatch_overlay.Properties;

namespace silly_overwatch_overlay
{
    // Presumably want to generate picture boxes on demand to play a gif then close

    // Either want to subscribe to a global timer or a local timer for each gif box

    // May want a custom class to handle gif boxes


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

        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;

            TopMost = true;

            //Opacity = 0.5f;

            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            //CreateGif(Image.FromFile("1.png"), new Point(0, 0), 1f);
            GifBox test = new GifBox(this, GlobalTimer, Image.FromFile("1.png"), new Point(0, 0), 1000);
        }

        public void CreateGif(Image gif, Point position, int duration)
        {
            SuspendLayout();

            PictureBox pictureBox = new PictureBox();

            pictureBox.Image = gif;
            pictureBox.Location = position;
            pictureBox.Name = "GifBox";
            pictureBox.Size = gif.Size;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;

            Controls.Add(pictureBox);

            ResumeLayout(false);
        }

        private void GlobalTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Test");
        }
    }
}
