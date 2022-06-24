using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexaScript : MonoBehaviour
{

    public bool isActive = false;
    public bool allowPlaying = false;
    public bool isImpacted = false;

    public float zeta = 0.0f;

    public float scaleZOriginal = 0;

    public float altoZ = 0.1f;

    private float currentUpdateTime = 0.0f;
    private float contadorApagarse = 0.0f;

    private createCave cc;

    private  GameObject  hexaLight;

    // starting value for the Lerp


    private int contaSound = 0;

    private float contaRemoveSoundClip = 0;
    public float removeSoundClipEvery = 50000.0f;

    private const byte k_MaxByteForOverexposedColor = 191; //internal Unity const

    public List<AudioClip> soundList = new List<AudioClip>(); //listado de sonidos

    private float tileLerp = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        scaleZOriginal = transform.localScale.z;
        altoZ = 1f;
        currentUpdateTime = 0f;

        //cc = FindObjectOfType<createCave>();
        cc = GetComponentInParent<createCave>();

        hexaLight = gameObject.transform.GetChild(0).gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        


        if(isImpacted){
            currentUpdateTime = 0f;
            contadorApagarse = 0f;
            contaRemoveSoundClip = 0f;

            allowPlaying = true;
            isActive = true;
            contaSound = 0;

            hexaLight.GetComponent<Light>().enabled = true;

            //defino el altoZ
            altoZ = altoZ+(altoZ/soundList.Count);

            //modifico el tile del shader. Contra más sonidos, más tile
            transform.GetComponentInChildren<Renderer>().material.SetFloat("_tiletext",soundList.Count);
        }




        if(allowPlaying == true){
            //transform.GetComponent<AudioSource>().enabled = true;
            //transform.GetComponent<AudioSource>().Play();
            playAllClips();
            //isPlaying = true;
        }



        if(isActive){
            currentUpdateTime += Time.deltaTime;
           //currentUpdateTime = 0f;

            //Debug.Log(transform.localScale.z);

            gameObject.name = "HexaActive";
            //zeta = (Mathf.Sin(Time.fixedTime*1)*10f);
            //transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y, scaleZOriginal+Time.deltaTime);
            

            //if(transform.localScale.z <= (scaleZOriginal+altoZ)-0.01f){
                //LERP SOLUTION
                //float distancia = Mathf.Abs(scaleZOriginal+altoZ - transform.localScale.z);         
                //transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x,transform.localScale.y, (scaleZOriginal+(scaleZOriginal+altoZ))), Time.deltaTime*distancia);
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x,transform.localScale.y, scaleZOriginal+altoZ), Time.deltaTime);
            //}
 




            //APAGARSE
            if(currentUpdateTime >= cc.timeToEndCombustion){
                apagarse();
            }


            //is black
            if(contadorApagarse >=  (cc.timeEndingCombustion)){
                isActive = false;
                currentUpdateTime = 0f;
                contadorApagarse = 0f;
                gameObject.name = "HexaInactive";
                hexaLight.GetComponent<Light>().enabled = false;
                //isActive = false;
            }

            isImpacted = false;         
            
        }else{

            //DECREASE color
            //Color newColor = Color.Lerp(transform.GetComponentInChildren<Renderer>().material.GetColor("_emission"), new Color(0,0,0), Time.deltaTime*2);
            //transform.GetComponentInChildren<Renderer>().material.SetColor("_emission", newColor);
        }


        //contador to remove sound clips
        contaRemoveSoundClip += 0.01f; 

        //Debug.Log(contaRemoveSoundClip);

        if(contaRemoveSoundClip >= removeSoundClipEvery){
            contaRemoveSoundClip = 0f;

            if(soundList.Count >= 1){
                //Debug.Log(altoZ);
                altoZ = altoZ-(altoZ/soundList.Count);
                if(altoZ <= 1.0f){
                    altoZ = 1.0f;
                }
                //Debug.Log(soundList[0].name +"removed");
                soundList.RemoveAt(0);
                isActive = true;

            }else{
                isActive = false;
                Debug.Log("no sounds to remove");
            }
        }


    }


    void apagarse(){

        contadorApagarse += 1;
        float interpolationRatio = (float)contadorApagarse / (cc.timeEndingCombustion*100);


        //MONOLITO
        Color currentColor = transform.GetComponentInChildren<Renderer>().material.GetColor("_emission");

        //Color newColor = Color.Lerp(currentColor, currentColor, interpolationRatio);
        Color newColor = Color.Lerp(currentColor, currentColor, Mathf.PingPong(Time.time, 1));
        transform.GetComponentInChildren<Renderer>().material.SetColor("_emission", newColor);

        //LUZ
        Color newLightColor = Color.Lerp(hexaLight.GetComponent<Light>().color, new Color(0,0,0), interpolationRatio);
        hexaLight.GetComponent<Light>().color = newLightColor;



    }



    void playAllClips(){

        //Debug.Log(soundList.Count);  
        if (transform.GetComponent<AudioSource>().isPlaying == false)
        {
           
            if(soundList.Count <= contaSound){
                allowPlaying = false;
                Debug.Log("enough sounds!"+contaSound);  
                contaSound = 0;
                return;
            }else{
                transform.GetComponent<AudioSource>().enabled = true;
            }
            Debug.Log(contaSound);  
            transform.GetComponent<AudioSource>().clip = soundList[contaSound];
            transform.GetComponent<AudioSource>().Play();
            contaSound += 1;
        }
    
    }







    void playAllClips2(int ontaSound){

        /*
        if(transform.GetComponent<AudioSource>().isPlaying == true) return;



        transform.GetComponent<AudioSource>().enabled = true;
        if (transform.GetComponent<AudioSource>().isPlaying == false)
        {
            transform.GetComponent<AudioSource>().clip = soundList[contaSound];
            transform.GetComponent<AudioSource>().Play();
        }else{
            contaSound += 1;
            if(soundList[contaSound] != null){
               playAllClips(contaSound);
            }else{
                isPlaying = false;
            }

        }
        */

  /*       foreach(AudioClip clip in soundList){
            
            transform.GetComponent<AudioSource>().clip = clip;
            transform.GetComponent<AudioSource>().Play();
            Debug.Log("clip.name");
            Debug.Log(clip.name);
        }
 */

    }


/*
    IEnumerator playAllClips(){

        transform.GetComponent<AudioSource>().enabled = true;
        while(true)
        {
            yield return new WaitForSeconds(1.0f);

            foreach(AudioClip clip in soundList){
            
                transform.GetComponent<AudioSource>().clip = clip;
                transform.GetComponent<AudioSource>().Play();
            }

        }



        transform.GetComponent<AudioSource>().enabled = true;
        foreach(AudioClip clip in soundList){
           
            transform.GetComponent<AudioSource>().clip = clip;
            transform.GetComponent<AudioSource>().Play();
        }


        //transform.GetComponent<AudioSource>().enabled = false;

    }
*/

}
