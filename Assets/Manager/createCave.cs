using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.CALIPSO;


public class createCave : MonoBehaviour
{

    public GameObject tile;

    public int maxAncho = 60;
    public int maxLargo = 60;

    private float currentUpdateTime = 0.0f;

    private calipsoManager cm;

    public float balanceoValor  = 1.5f;

    public Material[] materials;

    private int materialOfTheSeason = 0;


    [Header("===Monolitos tiempos===")]

    public float timeToEndCombustion = 2.0f;
    public float speedToEndCombustion = 1.0f;
    public float timeEndingCombustion = 10.0f;
    //public float timeToEndCombustion = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        cm = FindObjectOfType<calipsoManager>();

        //position center of camera
        transform.position = new Vector3((maxAncho / 2) * -1.0f,-3,0);
        
        
        //materialOfTheSeason = Random.Range(0, materials.Length);
        materialOfTheSeason = 0;

        createHexatiles();


 

    }

    // Update is called once per frame
    void Update()
    {
        

        /*
        //update time
        currentUpdateTime += Time.deltaTime;
        if(currentUpdateTime >= 1.0f){
            currentUpdateTime = 0f;
            //Material imparMaterial = Resources.Load<Material>("Materials/tileImpar");
            //Material parMaterial = Resources.Load<Material>("Materials/tilePar");
            destoyHexatiles();
            createHexatiles();
        }
        */
  


    }


    private void createHexatiles()
    {

        //bucle para el Ancho
        for(int i = 1; i < maxAncho; i++){

            //bucle para el Largo
            for(int f = 1; f < maxLargo; f++){

                //instancia del tile
                Quaternion rot = transform.rotation * Quaternion.AngleAxis(-90, Vector3.right);
                GameObject instaTile = Instantiate(tile, new Vector3(0, 0, 0), rot) as GameObject;
                
                //ajuste para el tile perfecto  
                float newZ = f*(1.5f);
                float newX = i+0.5f;

                //front bias
                float newZDigital = cm.mapToDigital(f, maxLargo/2, maxLargo, 0, 1);
                float exponencial = (Mathf.Pow(newZDigital, balanceoValor)) * Random.Range(18f, 50f);

                //right bias
                float newXDigital = cm.mapToDigital(i, maxAncho/2, maxAncho, 0, 1);
                float exponencialX = (Mathf.Pow(newXDigital, balanceoValor)) * Random.Range(18f, 50f);

                //left bias
                float newSinDigital = cm.mapToDigital(i, 1, maxAncho/4, 1, 0);
                float exponencialSin = Mathf.Pow(Mathf.Sin(newSinDigital),balanceoValor) * Random.Range(18f, 50f);

                //PAR //IMPAR //visualización de los tiles
                if(f % 2 == 0){
                    //instaTile.transform.position = new Vector3(i, 0, f*(1.5f));
                    instaTile.transform.position = new Vector3(newX, 0, newZ/2);
                    //instaTile.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
                }else{
                    instaTile.transform.position = new Vector3(i, 0, newZ/2);
                    //instaTile.GetComponent<MeshRenderer>().materials[0].color = Color.red;
                }

                //Altura de los tiles + random
                instaTile.transform.localScale = new Vector3(
                    instaTile.transform.localScale.x, 
                    instaTile.transform.localScale.y, 
                    Random.Range(0.6f, 1.5f)+((exponencial+exponencialX+exponencialSin)/3)
                );



                //Agregar a parent
                instaTile.transform.SetParent (transform, false);

                Color randomColor = new Color(Random.Range(0.0f, 0.1f), Random.Range(0.0f, 0.1f), Random.Range(0.0f, 0.1f), Random.Range(1.0f, 1.0f));

                //material del tile
                instaTile.GetComponentInChildren<Renderer>().material = materials[materialOfTheSeason];
                instaTile.GetComponentInChildren<Renderer>().material.SetFloat("_metallic", Random.Range(0f, 1f));
                instaTile.GetComponentInChildren<Renderer>().material.SetColor("_emission", randomColor);
                instaTile.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_offsetTimeY", 0f);
                instaTile.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_offsetTimeX", 0f);

            }
        }





    }


    private void destoyHexatiles()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
