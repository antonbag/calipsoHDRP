using UnityEngine;
using UnityEngine.UI;
using Unity.CALIPSO;

/*****  cada bar *****/



public class soundBarManager : MonoBehaviour
{

    private processAudio _processAudio;
    public GameObject micController;

    public int arrayNumber;
    public int currentWidth;

    private float valorAnterior;

    private calipsoManager cm;

    void Awake() {
        
 
    }

    // Start is called before the first frame update
    void Start()
    {
        micController = GameObject.Find("micController");
        _processAudio =  micController.GetComponent<processAudio>();
        cm =  FindObjectOfType<calipsoManager>();
     } 

    // Update is called once per frame
    void Update()
    {   
        
        
        if(_processAudio.spectrumData.Length > 0){
           //Debug.Log(_processAudio.spectrumData[arrayNumber]);

            //Debug.Log(arrayNumber);
            //IMAGES 

            //ATENUACIÓN POR ANTERIOR
            //Tengo que hacerlo en el processAudio
            /*
            GetComponent<RectTransform>().sizeDelta = new Vector2(
                currentWidth,
                ((_processAudio.spectrumData[arrayNumber]*valorAnterior)/2)
                *(_processAudio.powerMultiplier*50)
            );
            */


            if(gameObject.name=="fundamental"){
                //FUNDAMENTAL
                GetComponent<RectTransform>().sizeDelta = new Vector2(
                    currentWidth,
                    _processAudio.fundamentalSpectrum[arrayNumber]
                );

                //finally! la magia!
                GetComponent<Image>().color = new Color(
                    cm.mapToDigital(_processAudio.fundamentalSpectrum[arrayNumber],_processAudio.averageMin[arrayNumber],_processAudio.averageMax[arrayNumber],0,1)
                    ,
                    cm.mapToDigital(_processAudio.fundamentalSpectrum[arrayNumber],_processAudio.averageMin[arrayNumber],_processAudio.averageMax[arrayNumber],0,1)
                    ,
                    0
                    ,
                    0.5f
                );
 

                //Debug.Log(_processAudio.averageMin[arrayNumber]);

            }else{
                //NORMAL
                GetComponent<RectTransform>().sizeDelta = new Vector2(
                    currentWidth,
                    _processAudio.spectrumData[arrayNumber]
                );
            }

            
            //valorAnterior = _processAudio.spectrumData[arrayNumber];


         

            
   
            //_processAudio.spectrumData[arrayNumber]*(_processAudio.powerMultiplier*10)
            


            //soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(_processAudio.spectrumData[arrayNumber], 10);
        }
        
    }





}
