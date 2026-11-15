using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace silly_overwatch_overlay
{
    internal class BloodSplatter
    {
        // So I am not using gifs because PictureBox seems to refuse to be disposed or cleaned up by the garbage collector
        // Instead, I will make the blood splatter myself >:)

        // Based on the original video, most blood splatters have an 'origin' that a lot of blood splatters around, and then splashes further away

        private struct Particle
        {
            public PictureBox pictureBox;
            public int startingXPosition;
            public Point targetPosition;
        }

        Form parentForm;
        System.Windows.Forms.Timer splatterTimer;

        private Particle[] bloodParticles = new Particle[25];

        public BloodSplatter(Form parentForm, System.Windows.Forms.Timer splatterTimer)
        {
            // create a collection of different size red particles
            // pick a location from the bottom of the screen to throw particles from

            this.parentForm = parentForm;

            Image image = new Bitmap("test.png");

            for (int i = 0; i < bloodParticles.Length; i++)
            {
                bloodParticles[i] = new Particle()
                {
                    pictureBox = new PictureBox()
                    {
                        Image = image,
                        Location = new Point(0, 0),
                        Size = new Size(10, 10),//image.Size,
                        Name = "Box" + i.ToString(),
                        TabIndex = 0,
                        TabStop = false
                    }
                };

                parentForm.SuspendLayout();

                parentForm.Controls.Add(bloodParticles[i].pictureBox);

                parentForm.ResumeLayout(false);
            }

            this.splatterTimer = splatterTimer;
        }

        private long time;
        private int splatterTime;
        int screenWidth = 2560;
        int screenHeight = 1440;
        public void SplatterBlood()
        {
            time = 0;
            Random randomGenerator = new Random();
            splatterTime = randomGenerator.Next(1000, 2001);

            int randomX = randomGenerator.Next(0, screenWidth + 1);

            for (int i = 0; i < bloodParticles.Length; i++)
            {
                Particle particle = bloodParticles[i];

                int randomOffset = randomGenerator.Next(-screenWidth / 10, screenWidth / 10 + 1); // might change this to be a normal distribution
                double angle = Math.PI * randomGenerator.Next(45, 90 + 45 + 1) / 180;
                int distance = randomGenerator.Next(0, screenHeight + 1);

                particle.startingXPosition = randomX + randomOffset;
                int targetX = Convert.ToInt32(particle.startingXPosition + Math.Cos(angle) * distance);
                int targetY = Convert.ToInt32(screenHeight - Math.Sin(angle) * distance);
                particle.targetPosition = new Point(targetX, targetY);

                parentForm.SuspendLayout();
                particle.pictureBox.Location = new Point(particle.startingXPosition, screenHeight);
                parentForm.ResumeLayout(false);

                bloodParticles[i] = particle;

                splatterTimer.Tick += UpdateSplatter;
            }
        }

        long previousTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; // Due to processing time, the timer interval is not accurate to keep on track with the intended possible times
        
        public void UpdateSplatter(object sender, EventArgs e)
        {
            long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            time += currentTime - previousTime;
            previousTime = currentTime;

            if (time > splatterTime)
            {
                return;
            }

            foreach (Particle particle in bloodParticles)
            {
                int newX = particle.startingXPosition + Convert.ToInt32((particle.targetPosition.X - particle.startingXPosition) * ((double) time / splatterTime));
                int newY = screenHeight - Convert.ToInt32((screenHeight - particle.targetPosition.Y) * ((double) time / splatterTime));

                parentForm.SuspendLayout();
                particle.pictureBox.Location = new Point(newX, newY);
                parentForm.ResumeLayout(false);
            }
        }
    }
}
