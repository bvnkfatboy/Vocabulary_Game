using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MusicControl : MonoBehaviour
{
    public AudioSource audioSource;
    public Button soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool isSoundOn = true;

    void Start()
    {
        soundButton.onClick.AddListener(ToggleSound);
        UpdateSoundButton();
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        if (isSoundOn)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Pause();
        }
        UpdateSoundButton();
    }

    void UpdateSoundButton()
    {
        if (isSoundOn)
        {
            soundButton.image.sprite = soundOnSprite;
            Debug.Log("Sound ON");
        }
        else
        {
            soundButton.image.sprite = soundOffSprite;
            Debug.Log("Sound OFF");
        }
    }
}
