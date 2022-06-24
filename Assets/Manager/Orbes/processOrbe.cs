using System.Collections;
using UnityEngine;

public class processOrbe : MonoBehaviour
{
    #region Band type definition
    public enum BandType {
        FourBand,
        FourBandVisual,
        EightBand,
        TenBand,
        TwentySixBand,
        ThirtyOneBand
    };

    static float[][] middleFrequenciesForBands = {
        new float[]{ 125.0f, 500, 1000, 2000 },
        new float[]{ 250.0f, 400, 600, 800 },
        new float[]{ 63.0f, 125, 500, 1000, 2000, 4000, 6000, 8000 },
        new float[]{ 31.5f, 63, 125, 250, 500, 1000, 2000, 4000, 8000, 16000 },
        new float[]{ 25.0f, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000 },
        new float[]{ 20.0f, 25, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000 },
    };
    static float[] bandwidthForBands = {
        1.414f, // 2^(1/2)
        1.260f, // 2^(1/3)
        1.414f, // 2^(1/2)
        1.414f, // 2^(1/2)
        1.122f, // 2^(1/6)
        1.122f  // 2^(1/6)
    };
    #endregion

    #region Public variables
    public int numberOfSamples = 1024;
    public BandType bandType = BandType.TenBand;
    public float fallSpeed = 0.08f;
    public float sensibility = 8.0f;
    #endregion

 
    float[] rawSpectrum;
    float[] levels;
    float[] peakLevels;
    float[] meanLevels;


    #region Public property
    public float[] Levels {
        get { return levels; }
    }

    public float[] PeakLevels {
        get { return peakLevels; }
    }
    
    public float[] MeanLevels {
        get { return meanLevels; }
    }
    #endregion

    private  GameObject orbeSphere;
    private  AudioSource _audioSource;


    //TESTS dev
    /*
    public GameObject hexatest;
    public GameObject hexatest1;
    public GameObject hexatest2;
    public GameObject hexatest3;
    public GameObject hexatest4;
    public GameObject hexatest5;
    public GameObject hexatest6;
    public GameObject hexatest7;
    public GameObject hexatest8;
    */
    


  
    void Awake ()
    {
        CheckBuffers ();
        _audioSource = transform.GetComponent<AudioSource>();
    }

    void start(){
        

    }



    void Update ()
    {
        CheckBuffers ();

        //AudioListener.GetSpectrumData (rawSpectrum, 0, FFTWindow.BlackmanHarris);
        _audioSource.GetSpectrumData (rawSpectrum, 0, FFTWindow.BlackmanHarris);

        float[] middlefrequencies = middleFrequenciesForBands [(int)bandType];
        var bandwidth = bandwidthForBands [(int)bandType];

        var falldown = fallSpeed * Time.deltaTime;
        var filter = Mathf.Exp (-sensibility * Time.deltaTime);

        for (var bi = 0; bi < levels.Length; bi++) {
            int imin = FrequencyToSpectrumIndex (middlefrequencies [bi] / bandwidth);
            int imax = FrequencyToSpectrumIndex (middlefrequencies [bi] * bandwidth);

            var bandMax = 0.0f;
            for (var fi = imin; fi <= imax; fi++) {
                bandMax = Mathf.Max (bandMax, rawSpectrum[fi]);
            }

            levels[bi] = bandMax;
            peakLevels[bi] = Mathf.Max(peakLevels [bi] - falldown, bandMax);
            meanLevels[bi] = bandMax - (bandMax - meanLevels [bi]) * filter;
        }


       //orbeSphere.GetComponentInChildren<Renderer>().material.SetColor("_emission", randomColor*emissionMultiplier);   
        orbeSphere = transform.GetChild(0).gameObject;

        orbeSphere.GetComponentInChildren<Renderer>().material.SetFloat("_vertexPosMultiply",(meanLevels[3]*300)*-1);
        orbeSphere.GetComponentInChildren<Renderer>().material.SetFloat("_offsetTimeY",(meanLevels[3]*30));

        /*
        Color colorModificado = orbeSphere.GetComponentInChildren<Renderer>().material.GetColor("_emission");
        orbeSphere.GetComponentInChildren<Renderer>().material.SetColor("_emission",colorModificado*(int)meanLevels[3]);
        */
      
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

/*
        hexatest.transform.localScale = new Vector3 (1, 1, meanLevels[0]*100);
        hexatest1.transform.localScale = new Vector3 (1, 1, meanLevels[1]*100);
        hexatest2.transform.localScale = new Vector3 (1, 1, meanLevels[2]*100);
        hexatest3.transform.localScale = new Vector3 (1, 1, meanLevels[3]*100);
        hexatest4.transform.localScale = new Vector3 (1, 1, meanLevels[4]*100);
        hexatest5.transform.localScale = new Vector3 (1, 1, meanLevels[5]*100);
        hexatest6.transform.localScale = new Vector3 (1, 1, meanLevels[6]*100);
        hexatest7.transform.localScale = new Vector3 (1, 1, meanLevels[7]*100);
        hexatest8.transform.localScale = new Vector3 (1, 1, meanLevels[8]*100);
*/

        //Debug.Log ("Levels: " + meanLevels.Length);
    }



    //Private functions
    void CheckBuffers ()
    {
        if (rawSpectrum == null || rawSpectrum.Length != numberOfSamples) {
            rawSpectrum = new float[numberOfSamples];
        }
        var bandCount = middleFrequenciesForBands [(int)bandType].Length;
        if (levels == null || levels.Length != bandCount) {
            levels = new float[bandCount];
            peakLevels = new float[bandCount];
            meanLevels = new float[bandCount];
        }
    }

    int FrequencyToSpectrumIndex (float f)
    {
        var i = Mathf.FloorToInt (f / AudioSettings.outputSampleRate * 2.0f * rawSpectrum.Length);
        return Mathf.Clamp (i, 0, rawSpectrum.Length - 1);
    }

 
}
