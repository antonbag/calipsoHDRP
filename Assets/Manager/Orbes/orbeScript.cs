using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.CALIPSO;

public class orbeScript : MonoBehaviour
{

    private Camera maincam;
    public float speedMultiplier = 1.0f;
    public float speed = 0.5f;

    private processOrbe _processOrbe;
    calipsoManager cm;

    float minBass       = 1.0f;
    float maxBass       = 0.0f;
    float currentBass   = 1.0f;

    float minMed        = 1.0f;
    float maxMed        = 0.0f;
    float currentMed    = 1.0f;

    float minTreb       = 1.0f;
    float maxTreb       = 0.0f;


    //SMOTHNESS
    
    public float smoothSpeed = 0.125f;

    float randomX;


    //ROTATION
    Vector3 positionInicial;
    Vector3 destinoFinal;
    Vector3 haciaDestino;


    //PARAMS
    //Y
    public float ySinFreq = 0.5f;
    public float ySinMultiplier = 0.1f;
    public float yCurrentMultiplier = 11.0f;
    public float yFallStreng =0.7f;
    //rotationY
    private float newRotationY = 0.0f;

    public float multiplyTest = 1f;

    public bool devMovement = false;

    [Header("===Times range===")]
        [Range(0.1f, 3.0f)] public float stepRumbo = 1.0f;
        private float nextTime = 0.0f;
        public float lerpTime = 0.0f;


    [Header("===Collisions===")]

    private Rigidbody rb;
    public bool canCollide = true;

    [Header("===Materials===")]

    public Material hexagonRegularMaterial;
    public Material hexagonShaderMaterial;

    void Awake() {
        maincam = Camera.main;
        cm      = FindObjectOfType<calipsoManager>();
        rb      = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //POSITION INICIAL
        //positionInicial = new Vector3(maincam.transform.position.x, maincam.transform.position.y+Random.Range(-1.0f, 1.0f), (maincam.transform.position.z-2.0f) + speedMultiplier);
        //transform.position = positionInicial;

        _processOrbe = GetComponent<processOrbe>();

        //DEV MODE OFF
        if(devMovement == false){
            randomX = Random.Range(-50.0f, 50.0f);      
            transform.Rotate(0, randomX, 0);      
            destinoFinal = new Vector3(Random.Range(-30f,30f), 0, 30f);
        }



 

        //collision tests
        /*
            GameObject orbeSphere = gameObject.transform.GetChild(0).gameObject;
            Material mat = orbeSphere.GetComponent<Renderer>().material;

            GameObject orbeLight = gameObject.transform.GetChild(1).gameObject;
        
            GameObject monolito = GameObject.Find("hexatileTest");
            //monolito
        
            //Material hexaDefault = Resources.Load("hexaShader") as Material;
            monolito.GetComponent<Renderer>().material = hexagonShaderMaterial;
            monolito.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(mat);
        */
  
        //dev
        speed = 0.5f;
  
    }


    // Update is called once per frame
    void Update()
    {

        if(canCollide == false){
            scaleAndDestroy();
        }

        //DEV MODE ON
        if(devMovement == true){
            Vector3 m = new Vector3(0, 0, Time.deltaTime*10);
            transform.Translate(m);
            return;
        }



        //TIempos
        lerpTime += Time.deltaTime;


        minBass = getMin(_processOrbe.MeanLevels[0], minBass);
        maxBass = getMax(_processOrbe.MeanLevels[0], maxBass);
        currentBass = getCurrent(minBass, maxBass, _processOrbe.MeanLevels[0])*100000;

        minMed = getMin(_processOrbe.MeanLevels[2], minMed);
        maxMed = getMax(_processOrbe.MeanLevels[2], maxMed);
        currentMed = getCurrent(minMed, maxMed, _processOrbe.MeanLevels[2])*100000;

  

        //Debug.Log(maxMed);
        //Debug.Log(currentMed);

       
  
        /***********************/
        //Y
        float sinY = Mathf.Sin(lerpTime*ySinFreq)*(ySinMultiplier/10);
        float minMaxY = getYminMAX(currentBass,maxBass,maxMed);
        float minMaxYResult = (minMaxY*speed);
        
        float y = (sinY)+(minMaxYResult);

        //limitsY
        /***********************/
        //Z
        float explosiveZ = (Mathf.Lerp(10,0, lerpTime)*Time.deltaTime);
        float z = (speed*Time.deltaTime)*speedMultiplier+explosiveZ;

        //MOVIMIENTO BASE
        float step =  speed * Time.deltaTime; // calculate distance to move
        haciaDestino = Vector3.MoveTowards(transform.position, destinoFinal, step);


        /*
        Vector3 desiredPosition = destinoFinal;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        */


        //WITH ROTATION
        //Vector3 p = new Vector3(0, y , explosiveZ+0.01f);

        //test
        Vector3 p = new Vector3(0, sinY , z);


        /*totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        
        p = p * 1.0f;
        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        */

        

        transform.Translate(p);








        /*******************************/
        /*******************************/
        /******** cada  1 seg **********/
        /*******************************/
        /*******************************/
        if (Time.time > nextTime ) {
            nextTime += stepRumbo;
            newRotationY = Random.Range(-10f*maxMed, 10f*maxMed)*Time.deltaTime;
        }


        transform.Rotate(0, Mathf.Lerp(0, newRotationY, lerpTime), 0);


        //transform.rotation = Quaternion.LookRotation(new Vector3(maxBass/10,y*5,1));
        
        /*      
        float cosa = cm.mapToDigital(transform.position.z, positionInicial.z, destinoFinal.z, 0, 1);
        Debug.Log(cosa);
        
        //sin
        //p = new Vector3(0, Mathf.Sin(Time.time * y) * 0.1f, 0);

        p = new Vector3(0, Mathf.Sin(cosa * 2.0f) * 0.1f, 0);

        transform.Translate(p);

        //force.force = new Vector3(0, Mathf.Lerp(50,5,1), 1);

        */
        /*

        //rb.AddForce(0, Mathf.Sin(Time.deltaTime), 1f, ForceMode.Impulse);

        Vector3 toTarget = transform.position - destinoFinal;

        speed = 2.0f;

        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = speed * speed + Vector3.Dot(toTarget, Physics.gravity);    
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;


        rb.AddForce(new Vector3(0,1,1));

        Debug.Log(toTarget.z);
        */



    }


    /*******************************/
    /*******************************/
    /******** COLLISIONS **********/
    /*******************************/
    /*******************************/
    void OnTriggerEnter (Collider otherObject)
    {
        
        if(canCollide == false)
        {
            return;
        }


        //ORBE
        GameObject  orbeSphere  = gameObject.transform.GetChild(0).gameObject;
        GameObject  orbeLight   = gameObject.transform.GetChild(1).gameObject;
        Material    orbeMat     = orbeSphere.GetComponent<Renderer>().material;
        AudioClip   orbeClip    = gameObject.GetComponent<AudioSource>().clip;

        //MONOLITO
        GameObject monolito = otherObject.gameObject;

        //COPY PROPERTIES
        //monolito.GetComponent<Renderer>().material = hexagonShaderMaterial;
        monolito.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(orbeMat);
        Color monolitoEmission = orbeSphere.GetComponent<Renderer>().sharedMaterial.GetColor("_emission")*0.1f;
        monolito.GetComponent<Renderer>().sharedMaterial.SetColor("_emission", monolitoEmission);
        monolito.GetComponent<Renderer>().sharedMaterial.SetFloat("_offsetTimeX", 0.0f);
        monolito.GetComponent<Renderer>().sharedMaterial.SetFloat("_offsetTimeY", 0.0f);
        monolito.GetComponent<Renderer>().sharedMaterial.SetFloat("_vertexPosMultiply", 0.0f);


        //LIGHT
        GameObject  hexaLight   = monolito.transform.GetChild(0).gameObject;
        hexaLight.GetComponent<Light>().enabled = true;
        hexaLight.GetComponent<Light>().color = orbeLight.GetComponent<Light>().color;

        //SOUND
        monolito.GetComponent<hexaScript>().soundList.Add(orbeClip);
        monolito.GetComponent<AudioSource>().clip = orbeClip;
        monolito.GetComponent<hexaScript>().isImpacted = true;

        canCollide = false;

    }
 
    /*
    private float getYminMAX(float currentValue, float maxValue, float maxMedValue){
        
        if(currentValue*20 > maxValue){
            Debug.Log("maxMaxY: "+currentValue+"maxValue/2: "+maxValue/2);
            return (currentValue*Time.deltaTime)*multiplyTest;
            //return maxValue*0.125f;
        }else{
            //Debug.Log("minMaxY");
            Debug.Log("minMaxY: "+currentValue+"minMaxY/3: "+maxValue/3);
            return ((currentValue*Time.deltaTime)*10)*-1;
            return Mathf.Lerp(currentValue, ((maxValue*Time.deltaTime)*500)*-1, lerpTime/1000*Time.deltaTime);
            //return ((maxValue*Time.deltaTime)*100)*-1;
        }
    }*/

    private void scaleAndDestroy(){

        transform.localScale = new Vector3(transform.localScale.x-Time.deltaTime,transform.localScale.y-Time.deltaTime,transform.localScale.z-Time.deltaTime);

        if(transform.localScale.x < 0.01f){
            Destroy(gameObject);
        }

        //DESTROY ORBE 
        //Destroy(gameObject);

    }




    private float getYminMAX(float currentValue, float maxValue, float maxMedValue){
        
        if(currentValue*yCurrentMultiplier > maxValue){
            //Debug.Log("maxMaxY: "+currentValue+"maxValue/2: "+maxValue/2);
            return (currentValue/maxValue*Time.deltaTime)*10;
            //return maxValue*0.125f;
        }else{
            //Debug.Log("minMaxY");
            //Debug.Log("minMaxY: "+currentValue+"minMaxY/3: "+maxValue/3);
            return yFallStreng*Time.deltaTime*-1;
            //return Mathf.Lerp(currentValue, ((maxValue*Time.deltaTime)*500)*-1, lerpTime/1000*Time.deltaTime);
            //return ((maxValue*Time.deltaTime)*100)*-1;
        }
    }






    float getMin(float value, float min)
    {   
        if(value == 0f) return min;
        
        if (value*100 < min)
        {
            min = value*100;
        }
        return min;
    }


    float getMax(float value, float max)
    {   
        if(value == 0f) return max;
        
        if (value*100 > max)
        {
            max = value*100;
        }
        return max;
    }


    float getCurrent(float min, float max, float value)
    {   
        float current = value;
        if(value == 0f) return max;
        
        if (max/2 > value)
        {
            current = value*min;
        }else{
            current = value/min;
        }
        return current;
    }





}
