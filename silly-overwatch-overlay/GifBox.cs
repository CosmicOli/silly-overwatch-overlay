using System;
using System.Collections.Generic;
using System.Text;

namespace silly_overwatch_overlay
{
    internal class GifBox// : IDisposable
    {
        Form parentForm;
        PictureBox pictureBox;
        int TTL;
        System.Windows.Forms.Timer globalTimer;

        private void IncrementTTL(object sender, EventArgs e)
        {
            TTL -= ((System.Windows.Forms.Timer) sender).Interval;

            //pictureBox.Location = new System.Drawing.Point(pictureBox.Location.X + 10, pictureBox.Location.Y + 10);


            if (TTL <= 0)
            {
                parentForm.SuspendLayout();
                parentForm.Controls.Remove(pictureBox);
                globalTimer.Tick -= IncrementTTL;

                parentForm.ResumeLayout(false);

                TTL = 0;
            }

        }

        public GifBox(Form parentForm, System.Windows.Forms.Timer globalTimer)
        {
            this.parentForm = parentForm;

            this.globalTimer = globalTimer;

            pictureBox = new PictureBox
            {
                Name = "GifBox",
                TabIndex = 0,
                TabStop = false
            };
        }

        public void PlayGif(Image gif, Point position, int duration)
        {
            /*if (TTL != 0)
            {
                return;
            }*/

            this.parentForm.SuspendLayout();

            TTL = duration;
            globalTimer.Tick += IncrementTTL;

            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }

            pictureBox.Image = gif;
            pictureBox.Location = position;
            pictureBox.Size = gif.Size;

            this.parentForm.Controls.Add(pictureBox);

            this.parentForm.ResumeLayout(false);
        }
        
        /*public void Dispose()
        {
            TTL = 0;

            parentForm = new Form();
            parentForm.Dispose();

            pictureBox = new PictureBox();
            pictureBox.Dispose();

            globalTimer = new System.Windows.Forms.Timer();
            globalTimer.Dispose();
        }*/

    }
}
