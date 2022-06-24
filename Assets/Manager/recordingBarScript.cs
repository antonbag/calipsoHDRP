using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recordingBarScript : MonoBehaviour
{

    private processAudio _processAudio;

    // Start is called before the first frame update
    void Start()
    {
        _processAudio = FindObjectOfType<processAudio>();
 
    }

    // Update is called once per frame
    void Update()
    {

        if(_processAudio.isRecording){
            float valorSine = Mathf.Sin(Time.time)*100;

            GetComponent<RectTransform>().sizeDelta = new Vector2(
                10f,
                Mathf.Abs(valorSine)
            );
        }else{
            GetComponent<RectTransform>().sizeDelta = new Vector2(
                10f,
                0f
            );
        }
        
    }
}
