using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class orbesManager : MonoBehaviour
{
    //prefabs
    public GameObject orbeGroup;
    public GameObject orbesWrapper;

    public Camera maincam;

    public float emissionMultiplier = 10f;

    public Material[] materials;

    public string soundPath;
    public AudioClip audioClip;

    //wake up neo!
    void Awake()
    {
        soundPath = "file://"+Application.persistentDataPath+"/CALIPSO_sounds/";
    }



    // Start is called before the first frame update
    void Start()
    {
        maincam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl)) {  
            Debug.Log("orbe");
            int randomX = Random.Range(1, 5);
            createOrbe(randomX);
        }  
    }


    public void createOrbe(int clipNumber=0) {



        Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        //AudioClip createdClip = (AudioClip) Resources.Load("sounds/"+randomX);
 

        Vector3 positionInicial = new Vector3(0, Random.Range(-1.0f, 1.0f), (2.0f));
        GameObject orbeInstaGroup = Instantiate(orbeGroup, positionInicial, Quaternion.identity) as GameObject;

       
        StartCoroutine(loadAudio(clipNumber+".wav", orbeInstaGroup));

        //PLAY ON FINAL 


        GameObject orbeSphere = orbeInstaGroup.transform.GetChild(0).gameObject;
        GameObject orbeLight = orbeInstaGroup.transform.GetChild(1).gameObject;
        

        //LIGHT
        orbeLight.GetComponentInChildren<Light>().color = randomColor*(emissionMultiplier/10);
        //orbeInstaGroup.transform.GetChild(1).GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
  

        //MATERIAL
        //Material newRandomMaterial = new Material(
        orbeSphere.GetComponentInChildren<Renderer>().material = materials[Random.Range(0, materials.Length)];
        orbeSphere.GetComponentInChildren<Renderer>().material.SetColor("_emission", randomColor*emissionMultiplier);

        orbeSphere.GetComponentInChildren<Renderer>().material.SetFloat("_offsetTimeY", Random.Range(0.0f, 1.0f)*-1);

        //orbeInstaGroup.GetComponentInChildren<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //orbeInstaGroup.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        orbeInstaGroup.transform.SetParent (orbesWrapper.transform, false);

    } 


    private IEnumerator loadAudio(string audioName, GameObject orbeInstaGroup){

        string audioURL = string.Format(soundPath + "{0}", audioName);

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                orbeInstaGroup.GetComponent<AudioSource>().clip = audioClip;
                orbeInstaGroup.GetComponent<AudioSource>().Play();
            }
        }
       
    }



}
