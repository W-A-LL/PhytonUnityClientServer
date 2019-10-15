using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

 
using System.Collections;


/// <summary>
///     Example of requester who only sends Hello. Very nice guy.
///     You can copy this class and modify Run() to suits your needs.
///     To use this class, you just instantiate, call Start() when you want to start and Stop() when you want to stop.
/// </summary>
public class WorldResponser : RunAbleThread
{

    public string msg_back = null;
    string pending_message = null;
    bool processingMessage = false;



    /// <summary>
    ///     Request Hello message to server and receive message back. Do it 10 times.
    ///     Stop requesting when Running=false.
    /// </summary>
    protected override void Run()
    {
        ForceDotNet.Force(); // this line is needed to prevent unity freeze after one use, not sure why yet
        using (ResponseSocket server = new ResponseSocket())
        {
            server.Bind("tcp://*:5555");

            //missatge buit per defecte buit, ocupat i resultat 0
            FeedbackInfo fbInfo = new FeedbackInfo();
            string str = JsonUtility.ToJson(fbInfo);
            msg_back = str;
   
            string message = null;
            bool gotMessage = false;

            // mentre estem escoltant
            while (Running)
            {

                gotMessage = server.TryReceiveFrameString(out message); // this returns true if it's successful
                if (gotMessage)
                {
                    //si estem processant un missatge no desem com a pendent
                    if (processingMessage == true)
                    {
                        Debug.Log("msg_back " + msg_back);                        
                        server.SendFrame(msg_back);
                    
                    //si estem lliures el desem       
                    }  else {
                        pending_message = message;

                        Debug.Log("msg_back " + msg_back);                      
                        server.SendFrame(msg_back);
                       
                    }
                }
               
            }
        }


        NetMQConfig.Cleanup(); // this line is needed to prevent unity freeze after one use, not sure why yet
    }

  
    public void SetMessageBack(string str)
    {
        msg_back = str;
 
    }

    public void SetProcessingMessage(bool s)
    {
        processingMessage = s;
    }

    public bool IsProcessingMessage()
    {
        return processingMessage;
    }

    public string GetListenerMessage()
    {
        return pending_message;
    }


}