using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	const string MICROPHONE_KEY 	= "microphone";
	const string SENSITIVITY_KEY 	= "sensitivity";
	const string SOUNDBIAS_KEY 		= "soundBias";
	const string SAMPLES_KEY 		= "samples";
	const string OPTIMIZESAMPLES_KEY= "optimizeSamples";
	const string THRESHOLD_KEY 		= "threshold";
	const string LIMITFQ_KEY 		= "limitFq";
	const string AUTOVOLUME_KEY 	= "autovolume";
	const string RECORDING_KEY 		= "recording";

	public static void SetMicrophone (int mic) {
		PlayerPrefs.SetInt (MICROPHONE_KEY, mic);
	}

	public static int GetMicrophone (){
		return PlayerPrefs.GetInt (MICROPHONE_KEY);
	}

	public static void SetSensitivity (float sensitivity) {
		if (sensitivity >= 1f && sensitivity <= 1000f) {
			PlayerPrefs.SetFloat (SENSITIVITY_KEY, sensitivity);
		} else {
			Debug.LogError("Sensitivity out of range");
		}
	}

	public static float GetSensitivity (){
		return PlayerPrefs.GetFloat (SENSITIVITY_KEY);
	}

	public static void SetSoundBias (float soundBias) {
		if (soundBias >= 0f && soundBias <= 10f) {
			PlayerPrefs.SetFloat (SOUNDBIAS_KEY, soundBias);
		} else {
			Debug.LogError("soundBias out of range");
		}
	}

	public static float GetSoundBias (){
		return PlayerPrefs.GetFloat (SOUNDBIAS_KEY);
	}

	public static void SetThreshold (float threshold) {
		if (threshold >= 0f && threshold <= 50f) {
			PlayerPrefs.SetFloat (THRESHOLD_KEY, threshold);
		} else {
			Debug.LogError("Threshold "+threshold.ToString()+" out of range");
		}
	}

	public static float GetThreshold (){
		return PlayerPrefs.GetFloat (THRESHOLD_KEY);
	}

	public static void SetLimitFq (float limitFq) {
		if (limitFq >= 0f && limitFq <= 1f) {
			PlayerPrefs.SetFloat (LIMITFQ_KEY, limitFq);
		} else {
			Debug.LogError("limitFq "+limitFq.ToString()+" out of range");
		}
	}

	public static float GetLimitFq (){
		return PlayerPrefs.GetFloat (LIMITFQ_KEY);
	}

	public static void SetSamples (int samples) {
		if (samples >= 64 && samples <= 1024) {
			PlayerPrefs.SetInt (SAMPLES_KEY, samples);
		} else {
			//no debería pasar nunca... pero porsiaca
			Debug.LogError("samples out of range"+samples);
		}
	}
 
	public static int getSamples () {
		return PlayerPrefs.GetInt(SAMPLES_KEY);
	}

	public static void SetOptimizeSamples (float optimizeSamples) {
		if (optimizeSamples >= 1.00f && optimizeSamples <= 10.0f) {
			PlayerPrefs.SetFloat (OPTIMIZESAMPLES_KEY, optimizeSamples);
		} else {
			Debug.LogError("optimizeSamples "+optimizeSamples.ToString()+" out of range");
		}
	}

	public static float GetOptimizeSamples (){
		if(PlayerPrefs.GetFloat (OPTIMIZESAMPLES_KEY) <=0.0f){
			return 1.0f;
		}
		return PlayerPrefs.GetFloat (OPTIMIZESAMPLES_KEY);
	}

	public static void SetAutovolume (int autovolume) {
		PlayerPrefs.SetInt (AUTOVOLUME_KEY, autovolume);
	}

	public static bool GetAutovolume (){
		if(PlayerPrefs.GetInt (AUTOVOLUME_KEY) == 1){
			return true;
		}else{
			return false;
		}
	}

	public static void SetRecording (int recording) {
		PlayerPrefs.SetInt (RECORDING_KEY, recording);
	}

	public static bool GetRecording (){
		if(PlayerPrefs.GetInt (RECORDING_KEY) == 1){
			return true;
		}else{
			return false;
		}
	}

	
}