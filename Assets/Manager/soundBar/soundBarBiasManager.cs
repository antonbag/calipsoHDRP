using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*****  cada bar *****/



public class soundBarBiasManager : MonoBehaviour
{

    private processAudio _processAudio;
    private GameObject micController;

    public int arrayNumber;
    public int currentWidth;

    private float valorAnterior;

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
 
    }

    // Start is called before the first frame update
    void Start()
    {
        micController = GameObject.Find("micController");
        _processAudio =  micController.GetComponent<processAudio>();
     } 

    // Update is called once per frame
    void Update()
    {   
        
        
        if(_processAudio.spectrumDataBalanceo.Length > 0){

            GetComponent<RectTransform>().sizeDelta = new Vector2(
                    currentWidth/2,
                    _processAudio.spectrumDataBalanceo[arrayNumber]*(_processAudio.powerMultiplier*5)
            );
            GetComponent<Image>().color = new Color(
                1,
                0,
                0,
                0.5f
            );
            valorAnterior = _processAudio.spectrumData[arrayNumber];
          
   
            //_processAudio.spectrumData[arrayNumber]*(_processAudio.powerMultiplier*10)
            


            //soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(_processAudio.spectrumData[arrayNumber], 10);
        }
        
    }





}
