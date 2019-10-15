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

    // Update is called once per frame
    void Update()
    {

    }
    /*
    public void HandleMessage(string message)
    {
        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings[0] == "move")
        {
            ActionMove(message);
        }

        return;
    }



    private void ActionMove(string message)
    {

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 4) return;

        //splittedStrings[0] == "move
        float x = float.Parse(splittedStrings[1], NumberStyles.Any, ci);
        float y = float.Parse(splittedStrings[2], NumberStyles.Any, ci);
        float z = float.Parse(splittedStrings[3], NumberStyles.Any, ci);

        m_obj.transform.position = new Vector3(x, y, z);

       

    }
    */

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
        if (splittedStrings[0] == "move")
        {
           yield return StartCoroutine(ActionMove(message));  
        }

        Debug.Log("ACCIO ACABADA " + message);

        m_doingAction = false;

        yield return 0;
    }



    IEnumerator ActionMove(string message)
    {

        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        string[] splittedStrings = message.Split(' ');
        if (splittedStrings.Length != 4) yield return null;

        //splittedStrings[0] == "move
        float x = float.Parse(splittedStrings[1], NumberStyles.Any, ci);
        float y = float.Parse(splittedStrings[2], NumberStyles.Any, ci);
        float z = float.Parse(splittedStrings[3], NumberStyles.Any, ci);

        m_obj.transform.position = new Vector3(x, y, z);

        yield return new WaitForSeconds(1.0f);
       
    }

    
    
}




 
