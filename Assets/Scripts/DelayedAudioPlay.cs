using System.Collections;
using UnityEngine;

public class DelayedAudioPlay : MonoBehaviour
{
    public AudioSource audioSource;
    public float delayTime = 2f; // 延迟时间，单位为秒

    private Coroutine delayedAudioCoroutine;


    void OnEnable()
    {
        // 当面板可见时启动延迟播放协程
        delayedAudioCoroutine = StartCoroutine(PlayAudioWithDelay());
    }

    void OnDisable()
    {
        // 当面板不可见时停止延迟播放协程
        if (delayedAudioCoroutine != null)
        {
            StopCoroutine(delayedAudioCoroutine);
        }
    }

    IEnumerator PlayAudioWithDelay()
    {
        // 延迟指定时间
        yield return new WaitForSeconds(delayTime);

        // 播放音频
        audioSource.Play();
    }
}