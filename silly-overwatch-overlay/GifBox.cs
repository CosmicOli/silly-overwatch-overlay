using System;
using System.Collections.Generic;
using System.Text;

namespace silly_overwatch_overlay
{
    internal class GifBox// : IDisposable
    {
        Form1 parentForm;
        public PictureBox pictureBox;
        int TTL;
        System.Windows.Forms.Timer globalTimer;

        private void IncrementTTL(object sender, EventArgs e)
        {
            //parentForm.playingGif = true;
            TTL -= ((System.Windows.Forms.Timer) sender).Interval;

            //pictureBox.Location = new System.Drawing.Point(pictureBox.Location.X + 10, pictureBox.Location.Y + 10);


            if (TTL <= 0)
            {
                parentForm.SuspendLayout();
                parentForm.Controls.Remove(pictureBox);
                globalTimer.Tick -= IncrementTTL;

                parentForm.ResumeLayout(false);

                TTL = 0;

                //parentForm.playingGif = false;
            }

        }

        bool setup = false;
        public GifBox(Form1 parentForm, System.Windows.Forms.Timer globalTimer)
        {
            this.parentForm = parentForm;

            this.globalTimer = globalTimer;

            pictureBox = new PictureBox
            {
                Location = new Point(0, 0),
                Name = "GifBox",
                //SizeMode = PictureBoxSizeMode.Zoom,
                TabIndex = 0,
                TabStop = false
            };

            setup = true;
        }

        public void PlayGif(Image gif, Point position, int duration)
        {
            //this.gif = gif;

            if (TTL != 0 || !setup)
            {
                return;
            }

            pictureBox.Refresh();

            this.parentForm.SuspendLayout();

            pictureBox.Image = gif;
            //pictureBox.Size = new Size(1000, 1000);
            pictureBox.Size = gif.Size;

            pictureBox.Location = position;

            TTL = duration;
            globalTimer.Tick += IncrementTTL;

            this.parentForm.Controls.Add(pictureBox);

            this.parentForm.ResumeLayout(false);
        }
    }
}
