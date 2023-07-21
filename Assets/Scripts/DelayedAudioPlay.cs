using System.Collections;
using UnityEngine;

public class DelayedAudioPlay : MonoBehaviour
{
    public AudioSource audioSource;
    public float delayTime = 2f; // �ӳ�ʱ�䣬��λΪ��

    private Coroutine delayedAudioCoroutine;


    void OnEnable()
    {
        // �����ɼ�ʱ�����ӳٲ���Э��
        delayedAudioCoroutine = StartCoroutine(PlayAudioWithDelay());
    }

    void OnDisable()
    {
        // ����岻�ɼ�ʱֹͣ�ӳٲ���Э��
        if (delayedAudioCoroutine != null)
        {
            StopCoroutine(delayedAudioCoroutine);
        }
    }

    IEnumerator PlayAudioWithDelay()
    {
        // �ӳ�ָ��ʱ��
        yield return new WaitForSeconds(delayTime);

        // ������Ƶ
        audioSource.Play();
    }
}