using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] AudioMixer masterMixer;
    private float curVolume;
    // Start is called before the first frame update
    private void Start()
    {
       SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void SetVolume(float _value) {
        if (_value < 1) {
            _value = 0.001f;
        }

        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
    } 

    public void SetVolumeFromSlider() {
        SetVolume(slider.value);
    }

    public void RefreshSlider(float _value) {
        slider.value = _value;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
