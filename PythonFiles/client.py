#
#
#   Connects REQ socket to tcp://localhost:5555
#

import zmq
import random
import time
import json
import numpy as np
from PIL import Image
from matplotlib import pyplot as plt
import base64




#  Socket to talk to server
context = zmq.Context()
print("Connecting to server…")
socket = context.socket(zmq.REQ)
socket.connect("tcp://localhost:5555")


nextinstruction = True
messagesend = ""


while True:

    # preparar la instruccio a enviar
    if nextinstruction:
        messagesend = "move " + str(random.uniform(-1.0, 1.0)) + " " + str(random.uniform(-1.0, 1.0)) + " " + str(random.uniform(-1.0, 1.0))
    print("Sending request… " + messagesend)

    # enviar
    socket.send_string(messagesend)

    # espera
    time.sleep(0.005)

    # rebre missatge
    json_string = socket.recv()

    # convertir a string
    messagerecv = json_string.decode('utf-8')
    print("Recieving… " + messagerecv)

    # crear el diccionari
    feedbackInfo = eval(messagerecv)


    # mirar l'estat que diu si esta ocupat, si no ho està
    if feedbackInfo['appState']!='Busy':

        # mirem si ha acabat bé el procés
        if feedbackInfo['appResult']==1 :

            # mides de la imatge que retorna
            imwidth = feedbackInfo['screenshotWidth']
            imheight = feedbackInfo['screenshotHeight']

            # mode de la imatge que retorna
            immode = feedbackInfo['screenshotMode']
            if immode == 'RGBA32':
                immode = 'RGBA'
            if immode == 'RGB24':
                immode = 'RGB'

            # dades de la imatge
            strscreenshot = feedbackInfo['screenshotData']
            image_data = base64.b64decode(strscreenshot);

            # mostrem la imatge
            image = Image.frombytes(immode, (imwidth,imheight), image_data, 'raw')
            arr = np.array(image)

            # no continua fins que es tanca el plot
            plt.figure()
            plt.imshow(image)
            plt.show()

            # es veu fent debug
            # plt.imshow(image)

        nextinstruction = True
    else:
        nextinstruction = False

