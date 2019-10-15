using UnityEngine;

public class WorldServer : MonoBehaviour
{
    private WorldResponser _worldResponser;
    private Texture2D _testImage;

   
    private void Start()
    {
       
        _worldResponser = new WorldResponser();
        _worldResponser.Start();
    }



    private void Update()
    {
        string str_message = _worldResponser.GetListenerMessage();
        bool isproc = _worldResponser.IsProcessingMessage();


        //si hi ha algun missatge pendent i no n'estem processant cap
        if ((str_message != null) && (isproc == false))
        {
            //indiquem al thread que estem processant
            _worldResponser.SetProcessingMessage(true);

            //anem a les accions i processem el missatge
            GameObject m_actions = GameObject.Find("Actions");
            m_actions.GetComponent<Actions>().SetDoingAction(true);
            m_actions.GetComponent<Actions>().HandleMessage(str_message);         
        }


        //si estem processant un missatge
        if (isproc == true)
        {
            //anem a les accions i mirem si ha acabat
            //es accions acabades fan que la variable m_doingAction de la classe accio sigui false
            GameObject m_actions = GameObject.Find("Actions");
            if (m_actions.GetComponent<Actions>().IsDoingAction()==false)
            {
                //si ha acabat indiquem posem el string que volem tornar en forma de jsom
                FeedbackInfo fbInfo = new FeedbackInfo();               
                fbInfo.appState = "Done";

                                
                //hi desem la imatge que és una captura de pantalla
                _testImage = ScreenCapture.CaptureScreenshotAsTexture();


                //hi desem la imatge
                byte[] data = _testImage.GetRawTextureData();

                string enc = System.Convert.ToBase64String(data);            
                fbInfo.screenshotData = enc;

                //format de la imatge
                TextureFormat tf = _testImage.format;
                if (tf == TextureFormat.RGB24)  { fbInfo.screenshotMode = "RGB24";}
                else if (tf == TextureFormat.RGBA32) { fbInfo.screenshotMode = "RGBA32"; }

                //mides de la imatge
                fbInfo.screenshotWidth = _testImage.width;
                fbInfo.screenshotHeight = _testImage.height;

                //flag ha sortit bé
                fbInfo.appResult = 1;

                //convertir el JSON en string i posar-lo per retornar-lo
                string str = JsonUtility.ToJson(fbInfo);

                //el posem 
                _worldResponser.SetMessageBack(str);


                //si ha acabat indiquem al thread que ja no estem processant
                _worldResponser.SetProcessingMessage(false);

            }

            //si l'acció encara no ha acabat
            else
            {   
                //si no ha acabat indiquem al thread que estem ocupats
                FeedbackInfo fbInfo = new FeedbackInfo();
                string str = JsonUtility.ToJson(fbInfo);

                //string str = "Busy";
                _worldResponser.SetMessageBack(str);

            }

        }
      
    }


    private void OnDestroy()
    {
        _worldResponser.Stop();
    }
}