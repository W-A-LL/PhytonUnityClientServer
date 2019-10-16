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



def fun_GetAction():

    # accions possibles:
    # moveworld x y z
    # moveself x y z
    # rotateworld x y z
    # wait i

    # creem una accio de moure en una posicio aleatoria del mon entre [-1 -1 -1] i [1 1 1]
    action = "moveworld " + str(random.uniform(-1.0, 1.0)) + " " + str(random.uniform(-1.0, 1.0)) + " " + str(random.uniform(-1.0, 1.0))
    return action





while True:

    # preparar la instruccio a enviar
    if nextinstruction:
        messagesend = fun_GetAction()
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

    # JSON fields
    # int appResult;           //0 1
    # string appState;         //busy done
    # string screenshotData;   //data base 64
    # string screenshotMode;   //RGB24 RGBA32
    # int screenshotWidth;
    # int screenshotHeight;

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
            imdata = base64.b64decode(strscreenshot);

            # mostrem la imatge
            image = Image.frombytes(immode, (imwidth,imheight), imdata, 'raw')
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


