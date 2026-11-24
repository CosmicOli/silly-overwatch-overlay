# silly-overwatch-overlay
A project requested to me by a friend, inspired by https://www.instagram.com/reel/DdRGtEKKR2l/?stkn=dTBieTk1MHFpZmI4

All scripts relating to running the code are in the scripts directory.

To run the overlay, first run setup.bat; this will install the relevant python packages needed for this project using pip. Then, run run.bat to start the overlay.

Pip and Python have been bundled with this project such that no software installations outside of setup.bat is needed. I have not included any blood splatter gifs or audio, nor a background gif or background music, so you will need to provide your own. Blood splatter gifs and audios need to be in their own unique folders, where a random file in each folder will be picked every instance of a blood splatter occuring. Place them wherever you would like, and update settings.txt to point towards your provided assets. Example settings are provided.

Please do not judge my commit history for this project it is so awful. I wrote it initually in c# using winforms then realised I made a memory leak (somehow) then tried to make my own effects which ran terribly then fixed the memory leak instead then realised it still ran terribly then rewrote the whole thing in python blah blah blah yeah it was not my finest work.