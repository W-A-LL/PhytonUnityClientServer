using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Concurrent;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

using System.Globalization;

using UnityEngine.Networking;
using System.IO;



public class Actions : MonoBehaviour
{

    public GameObject m_obj;
    public bool m_doingAction = false;


    // Start is called before the first frame update
    void Start()
    {
        m_doingAction = false;
    }

  
    public void SetDoingAction(bool val)
    {
        m_doingAction = val;
    }


    public bool IsDoingAction()
    {
        return m_doingAction; 
    }



    public void HandleMessage(string message)
    {
        StartCoroutine(HandleMessageCoroutine( message));
    }




    public IEnumerator HandleMessageCoroutine(string message)
    {
        m_doingAction = true;

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        Debug.Log("FENT ACCIO " + message);


        string[] splittedStrings = message.Split(' ');
        if (splittedStrings[0] == "moveself")
        {
           yield return StartCoroutine(ActionMoveSelf(message));  
        }
        else if (splittedStrings[0] == "moveworld")
        {
            yield return StartCoroutine(ActionMoveWorld(message));
        }
        else if (splittedStrings[0] == "wait")
        {
            yield return StartCoroutine(ActionWait(message));
        }
        else if (splittedStrings[0] == "rotateself")
        {
            yield return StartCoroutine(ActionRotateSelf(message));
        }
        else if (splittedStrings[0] == "rotateworld")
        {
            yield return StartCoroutine(ActionRotateWorld(message));
        }

        Debug.Log("ACCIO ACABADA " + message);

        m_doingAction = false;

        yield return 0;
    }



    IEnumerator ActionMoveSelf(string message)
    {

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 4) yield return null;

        //splittedStrings[0] == "moveself
        float x = float.Parse(splittedStrings[1], NumberStyles.Any, ci);
        float y = float.Parse(splittedStrings[2], NumberStyles.Any, ci);
        float z = float.Parse(splittedStrings[3], NumberStyles.Any, ci);

        //m_obj.transform.position = new Vector3(x, y, z);
        Vector3 pos = m_obj.transform.position;
        m_obj.transform.position =  new Vector3(pos.x+x, pos.y+y, pos.z+z);

        //yield return new WaitForSeconds(1.0f);

    }


    IEnumerator ActionMoveWorld(string message)
    {

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 4) yield return null;

        //splittedStrings[0] == "moveworld
        float x = float.Parse(splittedStrings[1], NumberStyles.Any, ci);
        float y = float.Parse(splittedStrings[2], NumberStyles.Any, ci);
        float z = float.Parse(splittedStrings[3], NumberStyles.Any, ci);

        //m_obj.transform.position = new Vector3(x, y, z);
        Vector3 pos = m_obj.transform.position;
        m_obj.transform.position = new Vector3(x, y, z);

        //yield return new WaitForSeconds(1.0f);

    }


    IEnumerator ActionWait(string message)
    {

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 2) yield return null;

        //splittedStrings[0] == "wait
        float x = float.Parse(splittedStrings[1], NumberStyles.Any, ci);
        yield return new WaitForSeconds(x);

    }



    IEnumerator ActionRotateSelf(string message)
    {

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 4) yield return null;

        //splittedStrings[0] == "rotateworld
        float x = float.Parse(splittedStrings[1], NumberStyles.Any, ci);
        float y = float.Parse(splittedStrings[2], NumberStyles.Any, ci);
        float z = float.Parse(splittedStrings[3], NumberStyles.Any, ci);

        m_obj.transform.Rotate(x, y, z, Space.Self);

    }


    IEnumerator ActionRotateWorld(string message)
    {

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 4) yield return null;

        //splittedStrings[0] == "rotateself
        float x = float.Parse(splittedStrings[1], NumberStyles.Any, ci);
        float y = float.Parse(splittedStrings[2], NumberStyles.Any, ci);
        float z = float.Parse(splittedStrings[3], NumberStyles.Any, ci);

        m_obj.transform.Rotate(x, y, z, Space.World);

    }



}




 
