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

            if (TTL <= 0)
            {
                parentForm.SuspendLayout();
                parentForm.Controls.Remove(pictureBox);
                globalTimer.Tick -= IncrementTTL;

                parentForm.ResumeLayout(false);

                //Dispose();
            }

        }

        public GifBox(Form parentForm, System.Windows.Forms.Timer globalTimer)
        {
            this.parentForm = parentForm;

            this.globalTimer = globalTimer;
        }

        public void PlayGif(Image gif, Point position, int duration)
        {
            this.parentForm.SuspendLayout();

            TTL = duration;
            globalTimer.Tick += IncrementTTL;

            pictureBox = new PictureBox
            {
                Image = gif,
                Location = position,
                Name = "GifBox",
                Size = gif.Size,
                TabIndex = 0,
                TabStop = false
            };

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
