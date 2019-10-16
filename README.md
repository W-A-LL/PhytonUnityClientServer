# Unity3DPythonZeroMQ

Versions:
Python 3.7
Unity 2019.1.1f1



Summary:
Client made in Pyton sends instructions to the server, made in Unity, to move and rotate a cube.

Client sends a string with an instruction. Set of instructions: 
    # moveworld x y z
    # moveself x y z
    # rotateworld x y z
    # wait i

Server retruns a JSON file. JSON fields:
    # int appResult;           //0 1
    # string appState;         //busy done
    # string screenshotData;   //data base 64
    # string screenshotMode;   //RGB24 RGBA32
    # int screenshotWidth;
    # int screenshotHeight;



Run:
Run client.py
Load Unity project and run SampleSceneWorld scene


    
ToDo list (needs to be fixed):
- returned image is y-flipped
