import sys
from PyQt6.QtWidgets import QApplication, QWidget, QLabel
from PyQt6.QtCore import Qt, QTimer
from PyQt6.QtGui import QMovie

from mss import mss
import random
from collections import deque

import cv2
from PIL import Image
import numpy as np

class Window(QWidget):
    def __init__(self):
        super().__init__()

        self.setWindowFlags(
            Qt.WindowType.WindowStaysOnTopHint |
            Qt.WindowType.FramelessWindowHint |
            Qt.WindowType.X11BypassWindowManagerHint |
            Qt.WindowType.WindowTransparentForInput |
            Qt.WindowType.Tool
            )

        self.setAttribute(Qt.WidgetAttribute.WA_TranslucentBackground)
        self.setAttribute(Qt.WidgetAttribute.WA_TransparentForMouseEvents)

        self.gifs = deque()

        screen = QApplication.primaryScreen().geometry()
        self.setGeometry(screen)

        self.killDelayTimer = QTimer(self)
        self.killDelayActive = False

        #timer = QTimer(self)
        #timer.timeout.connect(loop)
        #timer.start(100)

    #def mousePressEvent(self, event):
    #    QApplication.instance().quit()

    def unlockKillDelay(self):
        self.killDelayActive = False

    def endGif(self, frameNumber):
        label, movie = self.gifs[0]

        #print(frameNumber)
        #print(movie.frameCount())

        if (frameNumber == movie.frameCount() - 1):
            #print("pop")
            label, movie = self.gifs.popleft()

            movie.frameChanged.disconnect(self.endGif)

            label.deleteLater()
            movie.deleteLater()

    def addGif(self, path, x, y):
        #print("working")

        if (not self.killDelayActive):
            self.killDelayActive = True
            self.killDelayTimer.singleShot(500, self.unlockKillDelay)

            label = QLabel(self)
            movie = QMovie(path)

            #movie.finished.connect(label.deleteLater)
            #movie.finished.connect(movie.deleteLater)

            label.setMovie(movie)

            label.move(x, y)
            movie.start()

            self.gifs.append((label, movie))
            
            movie.frameChanged.connect(self.endGif)

if __name__ == '__main__':
    app = QApplication(sys.argv)

    window = Window()
    window.show()

    #app.exec()

screenHeight = QApplication.primaryScreen().geometry().height()
screenWidth = QApplication.primaryScreen().geometry().width()

width = 61
height = 64

#top = QApplication.primaryScreen().geometry().height() // 2 - height // 2
#left = QApplication.primaryScreen().geometry().width() // 2 - width // 2

top = screenHeight // 2 - height // 2
left = screenWidth // 2 - width // 2

boundingBox = {'top': top, 'left': left, 'width': width, 'height': height}
MSS = mss()

def Within10Percent(reference, value):
    return abs((reference - value) / reference) <= 0.1

def loop():
    screenshot = MSS.grab(boundingBox)

    cv2.imshow('penis', np.array(Image.frombytes("RGB", screenshot.size, screenshot.bgra, "raw", "RGBX")))
    cv2.waitKey(1)

    #print("working lol")

    count = 0
    for x in range(0, width):
        for y in range(0, height):
            pixel = screenshot.pixel(x, y)

            r = pixel[0]
            g = pixel[1]
            b = pixel[2]

            #print(Within10Percent(255, r))
            #print(Within10Percent(255, r))
            #print(Within10Percent(255, r))
            #print(Within10Percent(255, r) and (not Within10Percent(255, g) or not Within10Percent(255, b)))

            if (Within10Percent(255, r) and (not Within10Percent(255, g) or not Within10Percent(255, b))):
                count += 1

    if (count >= width * height * 0.8):
        window.hide()

        gifWidth = 1008
        gifHeight = 1800

        randomX = random.randint(0, screenWidth - gifWidth)

        window.addGif("overlay.gif", randomX, screenHeight - gifHeight)
        window.show()


while (True):
    loop()
    





