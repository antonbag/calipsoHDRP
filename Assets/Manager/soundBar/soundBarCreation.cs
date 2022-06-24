using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.CALIPSO;
using Unity.CALIPSO.MIC;

public class soundBarCreation : MonoBehaviour
{
    
    private processAudio _processAudio;
    
    [Range(1, 100)]
    private float optimizationLevel = 1.0f;

    //prefabs
    public GameObject soundBar;
    public GameObject soundBarBias;

    //wrappers
    public GameObject SoundBarCanvas;
    public GameObject SoundBarBiasCanvas;


    private bool _soundBarActive = false;
    private bool _soundBarBiasActive = false;


    private calipsoManager cm;
    private micController mic;

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
        _processAudio =  FindObjectOfType<processAudio>();
        //SoundBarCanvas = GameObject.Find("SoundBarCanvas");
        cm = FindObjectOfType<calipsoManager>();
        mic= FindObjectOfType<micController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
    
                 
    }

    // Update is called once per frame
    void Update()
    {
        //creare o destruiré desde el manager
        /*
        //SOUNDBAR IN dev MODE
        if(cm.DevMode && !soundBarActive){
            createSoundBar();
            soundBarActive = true;
        }

        //SOUNDBAR IN SETTINGS MODE
        if(
            (!cm.DevMode && soundBarActive) || 
            mic.IsWorking == false
        ){
            Debug.Log("Destroy SoundBar");
            deleteSoundBar();
            soundBarActive = false;
        }
        */


    }


    public void createSoundBar(){

        instantiateSoundBar();

        //To visualize the soundBarBias
        instantiateSoundBarBias();

    }


    public void deleteSoundBar(){

        destroyInstancesSoundBar();

        destroyInstancesSoundBarBias();


    }


    /**SOUNDBARCREATION**/
    public void instantiateSoundBar(){

        optimizationLevel = PlayerPrefsManager.GetOptimizeSamples();
        //ya activo o no es dev mode!
        if(_soundBarActive || !cm.DevMode) return;
 
        //Debug.Log("*--createSoundBar--*"+_soundBarActive.ToString());

        //int totalBars = mic.checkSamplesRange();
        int totalBars = mic.checkSamplesRange() / (int) optimizationLevel;
        int anchoBars = (Screen.width/totalBars);

        int fundamentalWidth = totalBars/8;
        int totalFundamental = totalBars/8;

        //Debug.Log("totalFundamental: "+totalFundamental);
        //Debug.Log("totalBars: "+totalBars);


        int fundContador = 0;
        //NORMAL SPECTRUM
        for(int i = 0; i < totalBars; i++){

            GameObject soundBarPrefab = Instantiate(soundBar, new Vector3(anchoBars*i, 0, 0), Quaternion.identity) as GameObject;
            soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(anchoBars, 10);
            soundBarPrefab.GetComponent<soundBarManager>().arrayNumber = (mic.checkSamplesRange()/totalBars)*i;
            soundBarPrefab.GetComponent<soundBarManager>().currentWidth = anchoBars;
            soundBarPrefab.transform.SetParent (SoundBarCanvas.transform, false);


            if(i % totalFundamental == 0){

                GameObject soundBarPrefabFundamental = Instantiate(soundBar, new Vector3(fundamentalWidth*fundContador, 0, 0), Quaternion.identity) as GameObject;
                soundBarPrefabFundamental.name = "fundamental";
                soundBarPrefabFundamental.GetComponent<RectTransform>().sizeDelta = new Vector2(fundamentalWidth, 10);
                soundBarPrefabFundamental.GetComponent<soundBarManager>().arrayNumber = fundContador;
                soundBarPrefabFundamental.GetComponent<soundBarManager>().currentWidth = fundamentalWidth;
                soundBarPrefabFundamental.GetComponent<Image>().color = new Color(
                    0,
                    1,
                    0,
                    0.5f
                );

                //Debug.Log(soundBarPrefabFundamental.name);
                soundBarPrefabFundamental.transform.SetParent (SoundBarCanvas.transform, false);  

                fundContador += 1;
            }


        }

        //FUNDAMENTAL SPECTRUM



        _soundBarActive = true;

    }

    public void instantiateSoundBarBias(){

        optimizationLevel = PlayerPrefsManager.GetOptimizeSamples();
        //ya activo o no es dev mode!
        if(_soundBarBiasActive || !cm.DevMode) return;

        Debug.Log("*--createSoundBiasBar--*"+_soundBarBiasActive.ToString());

        //int totalBars = mic.checkSamplesRange();
        int totalBars = mic.checkSamplesRange() / (int) optimizationLevel;
        int anchoBars = (Screen.width/totalBars);

        for(int i = 0; i < totalBars; i++){
            if(i%4 == 0){
            GameObject soundBarBiasPrefab = Instantiate(soundBarBias, new Vector3(anchoBars*i, 0, 0), Quaternion.identity) as GameObject;
            soundBarBiasPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(anchoBars, 10);
            soundBarBiasPrefab.GetComponent<soundBarBiasManager>().arrayNumber = (mic.checkSamplesRange()/totalBars)*i;
            soundBarBiasPrefab.GetComponent<soundBarBiasManager>().currentWidth = anchoBars;
            soundBarBiasPrefab.transform.SetParent (SoundBarBiasCanvas.transform, false);
            }

        }
        _soundBarBiasActive = true;

    }



    /**SOUNDBAR DESTRUCCION**/
    public void destroyInstancesSoundBar(){


        //ya desactivado!
        if(!_soundBarActive) return;

        Debug.Log("*--deleteSoundBar--*"+_soundBarActive.ToString());
        
        //int totalBars = mic.checkSamplesRange()/(int)optimizationLevel;
        //int anchoBars = Screen.width/totalBars;
        foreach (Transform child in SoundBarCanvas.transform)
        {
            Destroy(child.gameObject);
        }
        _soundBarActive = false;

    }

    /**BIAS DESTRUCCION**/
    public void destroyInstancesSoundBarBias(){


        //ya desactivado!
        if(!_soundBarBiasActive) return;

        Debug.Log("*--deleteSoundBarBias--*"+_soundBarBiasActive.ToString());
        
        //int totalBars = mic.checkSamplesRange()/(int)optimizationLevel;
        //int anchoBars = Screen.width/totalBars;
        foreach (Transform child in SoundBarBiasCanvas.transform)
        {
            Destroy(child.gameObject);
        }
        _soundBarBiasActive = false;

    }



}
