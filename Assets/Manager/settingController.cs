using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // So we can use List<>
using Unity.CALIPSO;
using Unity.CALIPSO.MIC;

public class settingController : MonoBehaviour
{	
	
	public TMPro.TMP_Dropdown micDropdown, numSamplesDropdown;
	public Slider sensitivitySlider, soundBiasSlider, thresholdSlider, optimizeSampleSlider, limitFqSlider;
	public Toggle autoVolumeToggle, recordingToggle;
	public GameObject canvasSetting;
	public GameObject gmThValue;

	//public GameObject openButton;


	private List<string> options = new List<string>();
    
	private calipsoManager cm;
	private micController mic;

 	private bool panelActive = false;
	private string _microphone;

	[Header("==Sound DEFAULTS==")]

	public int numberOfSamples;
	public float sensitivity;
	public float soundBias;
	public float threshold;
	public float optimizeSample;
	public float limitFq;
	public int audioSampleRate;

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
        cm 	= FindObjectOfType<calipsoManager>();
		mic	= FindObjectOfType<micController>();
		
		SetDefaults();
    }

	// INICIO LOS VALORES DE LOS SLIDERS
	void Start () {

		//Listado de micrófonos disponibles
		foreach (string device in Microphone.devices) {
			if (_microphone == null) {
				//default mic
				_microphone = device;
			}
			options.Add(device);
		}
		_microphone = options[PlayerPrefsManager.GetMicrophone()];

		//add mics to dropdown
		micDropdown.AddOptions(options);

		micDropdown.onValueChanged.AddListener(delegate {micDropdownValueChangedHandler(micDropdown);});
		numSamplesDropdown.onValueChanged.AddListener(delegate {numSamplesDropdownValueChangedHandler(numSamplesDropdown);});
		sensitivitySlider.onValueChanged.AddListener(delegate {sensitivityValueChangedHandler(sensitivitySlider);});
		soundBiasSlider.onValueChanged.AddListener(delegate {soundBiasValueChangedHandler(soundBiasSlider);});
		thresholdSlider.onValueChanged.AddListener(delegate {thresholdValueChangedHandler(thresholdSlider);});
		optimizeSampleSlider.onValueChanged.AddListener(delegate {optimizeSampleSliderValueChangedHandler(optimizeSampleSlider);});
		limitFqSlider.onValueChanged.AddListener(delegate {limitFqSliderValueChangedHandler(limitFqSlider);});
		autoVolumeToggle.onValueChanged.AddListener(delegate {autoVolumeToggleValueChangedHandler(autoVolumeToggle);});
		recordingToggle.onValueChanged.AddListener(delegate {recordingToggleValueChangedHandler(recordingToggle);});

		gmThValue.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefsManager.GetThreshold().ToString();
	}


 
	public void SaveAndExit (){
		//Lo guardo on the fly
		//PlayerPrefsManager.SetMicrophone (microphone.value);
		//PlayerPrefsManager.SetSensitivity (sensitivitySlider.value);
		//PlayerPrefsManager.SetThreshold (thresholdSlider.value);
		//PlayerPrefsManager.SetSamples (numSamplesDropdown.value);
	
		//panelActive = !panelActive;
		//canvasSetting.GetComponent<Animator> ().SetBool ("PanelActive",panelActive);
        //Debug.Log("settingsMode: " + cm.settingsMode);
        cm.exitSettings();
        //Debug.Log("settingsMode: " + cm.settingsMode);
	}

	public void SetDefaults(){
		micDropdown.value = 0;
		sensitivitySlider.value = 100f;
		soundBiasSlider.value = 0.5f;
		thresholdSlider.value = 15.0f;
		optimizeSampleSlider.value = 1;
		numSamplesDropdown.value = 2; //1=128, 2=256, 3=512, 4=1024, 5=2048
		limitFqSlider.value = 0.15f; // 1 = total
		//autoVolumeToggle.isOn = true;
		autoVolumeToggle.isOn = false;

		PlayerPrefsManager.SetMicrophone(micDropdown.value);
		PlayerPrefsManager.SetSensitivity(sensitivitySlider.value);
		PlayerPrefsManager.SetThreshold(thresholdSlider.value);
		PlayerPrefsManager.SetOptimizeSamples(optimizeSampleSlider.value);
		PlayerPrefsManager.SetSamples(256);
		PlayerPrefsManager.SetLimitFq(limitFqSlider.value);
		PlayerPrefsManager.SetAutovolume(0);
		PlayerPrefsManager.SetRecording(0);
			
	}
 
	public void OpenSettings(){
		panelActive = !panelActive;
		//canvasSetting.GetComponent<Animator> ().SetBool ("PanelActive",panelActive);
	}

	public void TogglePanel(){
		if (!panelActive) {
			OpenSettings ();
		} else {
			SaveAndExit ();
		}
	}


	//MIC
	public void micDropdownValueChangedHandler(TMPro.TMP_Dropdown micDropdown) {
		_microphone = options[micDropdown.value];
		Debug.Log("micDropdownValueChangedHandler: " + _microphone);
		mic.UpdateMicrophone();
	}

	//SAMPLES
	public void numSamplesDropdownValueChangedHandler(TMPro.TMP_Dropdown numSample){
		mic.WorkStop();
		int currentSamples = PlayerPrefsManager.getSamples();

		int[] samplesArray = {64,128,256,512,1024,2048,4096,8192};
		numberOfSamples = samplesArray[numSample.value];


		//Si ha cambiado, actualizo bars
		if(currentSamples != numberOfSamples){
			Debug.Log("NUEVOS SAMPLES: " + numberOfSamples);
			PlayerPrefsManager.SetSamples(numberOfSamples);
			mic.UpdateMicrophone();
		}else{
			Debug.Log("NO HAY CAMBIO EN samples: " + numberOfSamples);
		}

	}

	//SENSITIVITY
	public void sensitivityValueChangedHandler(Slider SensitivitySlider){
		sensitivity = SensitivitySlider.value;
		PlayerPrefsManager.SetSensitivity(sensitivity);
	}
	
	//SOUNDBIAS
	public void soundBiasValueChangedHandler(Slider soundBiasSlider){
		soundBias = soundBiasSlider.value;
		PlayerPrefsManager.SetSoundBias(soundBias);
	}
	
	//THRESHOLD
	public void thresholdValueChangedHandler(Slider thresholdSlider){
		threshold = thresholdSlider.value;
		//threshold value in interface
		PlayerPrefsManager.SetThreshold(threshold);
		gmThValue.GetComponent<TMPro.TextMeshProUGUI>().text = threshold.ToString();

	}

	//OPTIMIZE SAMPLES
	public void optimizeSampleSliderValueChangedHandler(Slider optimizeSampleSlider){
		optimizeSample = optimizeSampleSlider.value;
		PlayerPrefsManager.SetOptimizeSamples(optimizeSample);
		//Debug.Log("optimizeSampleSliderValueChangedHandler: " + optimizeSampleSlider);
		mic.UpdateMicrophone();
	}
	//LIMIT H
	public void limitFqSliderValueChangedHandler(Slider limitFqSlider){
		limitFq = limitFqSlider.value;
		PlayerPrefsManager.SetLimitFq(limitFq);
		mic.UpdateMicrophone();
	}

	//AUTO VOLUME
	public void autoVolumeToggleValueChangedHandler(Toggle autoVolumeToggle){
		bool autoVolume = autoVolumeToggle.isOn;
		PlayerPrefsManager.SetAutovolume(autoVolume ? 1 : 0);
		//mic.UpdateMicrophone();
	}

	//RECORDING
	public void recordingToggleValueChangedHandler(Toggle recordingToggle){
		bool recording = recordingToggle.isOn;
		PlayerPrefsManager.SetRecording(recording ? 1 : 0);
	}



	public string getMicrophone(){
		return _microphone;
	}


}
