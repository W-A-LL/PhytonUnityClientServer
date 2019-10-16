using UnityEngine;

public class WorldServer : MonoBehaviour
{
    private WorldResponser _worldResponser;
    public GameObject _cam;


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
                //-------------
                //si ha acabat indiquem posem el string que volem tornar en forma de jsom
                FeedbackInfo fbInfo = new FeedbackInfo();               
                fbInfo.appState = "Done";


                //-------------
                //hi desem la imatge que és una captura de pantalla
                // _testImage = ScreenCapture.CaptureScreenshotAsTexture();

                Camera camera = _cam.GetComponent<Camera>();
                int resWidth = Screen.width;
                int resHeight = Screen.height;

                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                camera.targetTexture = rt; //Create new renderTexture and assign to camera
                Texture2D _testImage = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false); //Create new texture

                camera.Render();

                RenderTexture.active = rt;
                _testImage.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0); //Apply pixels from camera onto Texture2D

                camera.targetTexture = null;
                RenderTexture.active = null; //Clean
               

                //hi desem la imatge
                byte[] data = _testImage.GetRawTextureData();

                string enc = System.Convert.ToBase64String(data);            
                fbInfo.screenshotData = enc;

                //Free memory
                Destroy(rt);  
                Destroy(_testImage); 


                //-------------
                //format de la imatge
                TextureFormat tf = _testImage.format;
                if (tf == TextureFormat.RGB24)  { fbInfo.screenshotMode = "RGB24";}
                else if (tf == TextureFormat.RGBA32) { fbInfo.screenshotMode = "RGBA32"; }

                //-------------
                //mides de la imatge
                fbInfo.screenshotWidth = _testImage.width;
                fbInfo.screenshotHeight = _testImage.height;

                //-------------
                //flag ha sortit bé
                fbInfo.appResult = 1;

                //-------------

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






/* del supermercat
 * 
               private void SaveDataToFolder()
               {
                   Texture2D output = new Texture2D(camOutput.width, camOutput.height, TextureFormat.RGB24, false);

                   foreach (Camera cam in cameras)
                   {
                       if (!cam.name.Equals("MainCamera")) QualitySettings.antiAliasing = 0;
                       else QualitySettings.antiAliasing = 8;

                       camOutput.Create();  // create the rendered texture

                       cam.targetTexture = camOutput;   // set the camera to render to rTex
                                                   //bg.texture = prod.obj.GetComponent<Product_Controller>().background;    //get the background of the product and put it to the scene

                       cam.Render();   // render the camera, make sure that the image is being generated

                       RenderTexture.active = camOutput;    // set render to texture to active

                       output.ReadPixels(new Rect(0, 0, camOutput.width, camOutput.height), 0, 0);   // read the generated image
                       output.Apply();

                       // save pixels to the file inside the folder of the product
                       byte[] bytes = output.EncodeToPNG();

                       System.IO.File.WriteAllBytes(path + "00E_" + cam.name + ".png", bytes);

                       RenderTexture.active = null;    // desactive render to texture just to avoid modifications
                       cam.targetTexture = null;
                   }

                   QualitySettings.antiAliasing = 8;

                   stream = new FileStream(path + "00E.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                   stream.SetLength(0);
                   serializer.Serialize(stream, sC.scene);
                   stream.Close();

               }

               */
