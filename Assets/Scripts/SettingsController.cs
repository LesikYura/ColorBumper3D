using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{

    [SerializeField] private GameObject _off;
    [SerializeField] private GameObject _offDisable;
    [SerializeField] private GameObject _on;
    [SerializeField] private GameObject _onDisable;
    [SerializeField] private AudioSource _audioSource;

    private bool isMusicPlay = true;
    
    public void MusicOnOff(bool isOn)
    {
        _off.SetActive(isOn);
        _offDisable.SetActive(isOn);
        _on.SetActive(!isOn);
        _onDisable.SetActive(!isOn);

        if (isMusicPlay)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.Play();
        }

        isMusicPlay = !isMusicPlay;
    }
}
