using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEditor;
using Microsoft.MixedReality.Toolkit.Subsystems;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.Events;
using Cas;

public class ETRSSpeech : MonoBehaviour
{

    [SerializeField]
    private List<KeywordAction> keywordActions;
    private void Start()
    {
        var keywordRecognitionSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<KeywordRecognitionSubsystem>();
       
        if (keywordRecognitionSubsystem != null)
        {
            Debug.Log("语音服务已启用");
            foreach (var ka in keywordActions)
            {
                if (!string.IsNullOrEmpty(ka.keyword_) && ka.action_.GetPersistentEventCount() > 0)
                {
                    keywordRecognitionSubsystem.CreateOrGetEventForKeyword(ka.keyword_).AddListener(() => ka.action_.Invoke());
                }
            }
        }
    }
}


[Serializable]
public struct KeywordAction
{
    [SerializeField]
    private string keyword;

    [SerializeField]
    private UnityEvent action;

    public string keyword_ => keyword;

    public UnityEvent action_ => action;

}
