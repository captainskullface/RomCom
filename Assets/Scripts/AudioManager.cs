using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioMan;

    [SerializeField]
    AudioClip buttonClick;
    [SerializeField]
    AudioClip buttonHover;

    [SerializeField]
    AudioClip newEmail;

    [SerializeField]
    AudioClip screechPosted;

    [SerializeField]
    AudioClip publishSound;

    [SerializeField]
    AudioClip rejectSound;

    [SerializeField]
    AudioClip viewBookSound;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        audioMan = this;
    }

    public void ButtonClicked()
    {
        PlaySound(buttonClick);
    }

    public void ButtonHover()
    {
        PlaySound(buttonHover);
    }

    public void NewEmail()
    {
        PlaySound(newEmail);
    }

    public void ScreechPosted()
    {
        PlaySound(screechPosted);
    }

    public void PublishBook()
    {
        PlaySound(publishSound);
    }

    public void RejectBook()
    {
        PlaySound(rejectSound);
    }

    public void viewBook()
    {
        PlaySound(viewBookSound);
    }    

    public void PlaySound(AudioClip sound, float volume = 1)
    {
        source.volume = volume;

        source.PlayOneShot(sound);
    }    
}
