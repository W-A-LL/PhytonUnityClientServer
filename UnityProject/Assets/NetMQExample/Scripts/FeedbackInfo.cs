using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class FeedbackInfo
{
    public int appResult;           //0 1
    public string appState;         //busy done
    public string screenshotData;   //data base 64
    public string screenshotMode;   //RGB24 RGBA32
    public int screenshotWidth;
    public int screenshotHeight;

    public FeedbackInfo()
    {
         appResult = 0;            
         appState = "Busy";        
         screenshotData = " ";    
         screenshotMode = " ";     
         screenshotWidth = 0;
         screenshotHeight = 0;

    }
}

