using System;
using System.Collections.Generic;
using System.Text;

namespace silly_overwatch_overlay
{
    internal class GifBox
    {
        Form parentForm;
        PictureBox pictureBox;
        int TTL;

        private void IncrementTTL(object sender, EventArgs e)
        {
            TTL -= ((System.Windows.Forms.Timer) sender).Interval;

            if (TTL <= 0)
            {
                parentForm.SuspendLayout();
                parentForm.Controls.Remove(pictureBox);
                parentForm.ResumeLayout(false);
            }
        }


        public GifBox(Form parentForm, System.Windows.Forms.Timer globalTimer, Image gif, Point position, int duration)
        {
            this.parentForm = parentForm;
            this.parentForm.SuspendLayout();

            TTL = duration;
            globalTimer.Tick += IncrementTTL;

            pictureBox = new PictureBox();

            pictureBox.Image = gif;
            pictureBox.Location = position;
            pictureBox.Name = "GifBox";
            pictureBox.Size = gif.Size;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;

            this.parentForm.Controls.Add(pictureBox);

            this.parentForm.ResumeLayout(false);
        }
    }
}
