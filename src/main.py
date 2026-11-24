from collections import deque
import os
import random
import re
import sys

from PyQt6.QtWidgets import QApplication, QWidget, QLabel
from PyQt6.QtCore import Qt, QTimer, QSize
from PyQt6.QtGui import QMovie
from PySide6.QtMultimedia import QMediaPlayer, QAudioOutput

from PIL import Image
from mss import mss

settingsDirectory = "./settings.txt"

settingsFile = open(settingsDirectory, "r")
settings = settingsFile.read()

gifsDirectoryPath = re.search("blood splatter gifs directory: ?(.*)", settings).group(1)
audiosDirectoryPath = re.search("blood splatter audios directory: ?(.*)", settings).group(1)

backgroundGifPath = re.search("background gif: ?(.*)", settings).group(1)
musicPath = re.search("music gif: ?(.*)", settings).group(1)

splatterVolume = float(re.search("blood splatter audio volume: ?(.*)", settings).group(1)) / 100
musicVolume = float(re.search("music audio volume: ?(.*)", settings).group(1)) / 100

enableBackground = re.search("enable background gif: ?(.*)", settings).group(1).lower() == "true"

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

        self.splatters = deque()

        screen = QApplication.primaryScreen().geometry()
        self.setGeometry(screen)

        self.killDelayTimer = QTimer(self)
        self.killDelayActive = False

        self.loopTimer = QTimer(self)
        self.loopTimer.timeout.connect(loop)
        self.loopTimer.start(100)

        self.tetoPlayer = QMediaPlayer()
        self.tetoAudio = QAudioOutput()
        self.tetoAudio.setVolume(musicVolume)
        self.tetoPlayer.setAudioOutput(self.tetoAudio)
        self.tetoPlayer.setSource(musicPath)
        self.tetoPlayer.setLoops(QMediaPlayer.Loops.Infinite)
        self.tetoPlayer.play()

        if (enableBackground):
            self.starLabel = QLabel(self)
            self.starMovie = QMovie(backgroundGifPath)
            self.starMovie.setScaledSize(QSize(screenWidth, screenHeight))
            self.starLabel.setMovie(self.starMovie)
            self.starMovie.start()

    def unlockKillDelay(self):
        self.killDelayActive = False

    def endGif(self, frameNumber):
        label, movie, player, audio = self.splatters[0]

        if (frameNumber == movie.frameCount() - 1):
            label, movie, player, audio = self.splatters.popleft()

            movie.frameChanged.disconnect(self.endGif)
            player.stop()

            label.deleteLater()
            movie.deleteLater()
            player.deleteLater()
            audio.deleteLater()

    def playSplatter(self, gifPath, audioPath):
        if (not self.killDelayActive):
            self.killDelayActive = True
            self.killDelayTimer.singleShot(500, self.unlockKillDelay)

            audio = QAudioOutput()
            player = QMediaPlayer()

            audio.setVolume(splatterVolume)
            player.setAudioOutput(audio)
            player.setSource(audioPath)
            player.play()

            label = QLabel(self)
            movie = QMovie(gifPath)
            
            gif = Image.open(gifPath)
            newWidth = int(gif.width * (screenHeight / gif.height))
            movie.setScaledSize(QSize(newWidth, screenHeight))

            label.setMovie(movie)

            randomX = random.randint(0, screenWidth - newWidth)
            label.move(randomX, 0)

            movie.start()

            self.splatters.append((label, movie, player, audio))

            movie.frameChanged.connect(self.endGif)


MSS = mss()

def WithinPercent(reference, value, percent):
    return abs((reference - value) / reference) <= percent / 100

def loop():
    screenshot = MSS.grab(boundingBox)

    count = 0
    for x in range(0, width):
        for y in range(0, height):
            pixel = screenshot.pixel(x, y)

            r = pixel[0]
            g = pixel[1]
            b = pixel[2]

            if (WithinPercent(255, r, 10) and (not WithinPercent(255, g, 40) or not WithinPercent(255, b, 25))):
                count += 1

    if (count >= width * height * 0.6):
        window.hide()

        gifDirectory = gifsDirectoryPath + random.choice(os.listdir(gifsDirectoryPath))

        audioDirectory = audiosDirectoryPath + random.choice(os.listdir(audiosDirectoryPath))

        window.playSplatter(gifDirectory, audioDirectory)
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
    





