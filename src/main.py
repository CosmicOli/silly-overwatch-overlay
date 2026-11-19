import sys
from PyQt6.QtWidgets import QApplication, QWidget, QLabel
from PyQt6.QtCore import Qt, QTimer, QSize
from PyQt6.QtGui import QMovie
from PySide6.QtMultimedia import QMediaPlayer, QAudioOutput

from mss import mss
import random
from collections import deque

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

        self.loopTimer = QTimer(self)
        self.loopTimer.timeout.connect(loop)
        self.loopTimer.start(100)

        self.splatterPlayer = QMediaPlayer()
        self.splatterAudio = QAudioOutput()
        self.splatterPlayer.setAudioOutput(self.splatterAudio)
        self.splatterPlayer.setSource("./src/splatter.mp3")

        self.tetoPlayer = QMediaPlayer()
        self.tetoAudio = QAudioOutput()
        self.tetoAudio.setVolume(0.4) # ehhh seems like decent audio balancing ig
        self.tetoPlayer.setAudioOutput(self.tetoAudio)
        self.tetoPlayer.setSource("./src/teto.mp3")
        self.tetoPlayer.setLoops(QMediaPlayer.Loops.Infinite)
        self.tetoPlayer.play()

        self.starLabel = QLabel(self)
        self.starMovie = QMovie("./src/star.gif")
        self.starMovie.setScaledSize(QSize(screenWidth, screenHeight))
        self.starLabel.setMovie(self.starMovie)
        self.starMovie.start()

    def unlockKillDelay(self):
        self.killDelayActive = False

    def endGif(self, frameNumber):
        label, movie = self.gifs[0]

        if (frameNumber == movie.frameCount() - 1):
            label, movie = self.gifs.popleft()

            movie.frameChanged.disconnect(self.endGif)

            label.deleteLater()
            movie.deleteLater()

    def playSplatter(self, path, x, y):
        if (not self.killDelayActive):
            self.killDelayActive = True
            self.killDelayTimer.singleShot(500, self.unlockKillDelay)

            self.splatterPlayer.stop()
            self.splatterPlayer.play()

            label = QLabel(self)
            movie = QMovie(path)

            label.setMovie(movie)

            label.move(x, y)
            movie.start()

            self.gifs.append((label, movie))

            movie.frameChanged.connect(self.endGif)


MSS = mss()

def Within10Percent(reference, value):
    return abs((reference - value) / reference) <= 0.1

def loop():
    screenshot = MSS.grab(boundingBox)

    count = 0
    for x in range(0, width):
        for y in range(0, height):
            pixel = screenshot.pixel(x, y)

            r = pixel[0]
            g = pixel[1]
            b = pixel[2]

            if (Within10Percent(255, r) and (not Within10Percent(255, g) or not Within10Percent(255, b))):
                count += 1

    if (count >= width * height * 0.6):
        window.hide()

        gifWidth = 1008
        gifHeight = 1800

        randomX = random.randint(0, screenWidth - gifWidth)

        window.playSplatter("./src/overlay.gif", randomX, screenHeight - gifHeight)
        window.show()


if __name__ == '__main__':
    app = QApplication(sys.argv)

    screenHeight = QApplication.primaryScreen().geometry().height()
    screenWidth = QApplication.primaryScreen().geometry().width()

    height = int((64 / 1440) * screenHeight)
    width = height

    top = screenHeight // 2 - height // 2
    left = screenWidth // 2 - width // 2

    boundingBox = {'top': top, 'left': left, 'width': width, 'height': height}

    window = Window()
    window.show()

    app.exec()
    





