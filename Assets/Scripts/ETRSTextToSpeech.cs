using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETRSTextToSpeech : MonoBehaviour
{
    private string text;
    public  AudioSource audio;
    public void TextToSpeech(string text)
    {
        var textToSpeechSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<TextToSpeechSubsystem>();
        if (textToSpeechSubsystem != null)
        {
            textToSpeechSubsystem.TrySpeak(text,audio);
        }
    }
}
