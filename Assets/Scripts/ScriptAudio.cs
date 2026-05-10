using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ScriptAudio : MonoBehaviour
{
    public AudioMixer masterMixer;
    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat( "musicVol" , soundLevel);
    }
}

