using UnityEngine;
using TMPro;

public class ClickScript : MonoBehaviour
{
    public GameObject Canvas;
    public TextMeshProUGUI text;
    private int clickCount = 0;
    private bool isCanvasVisible = false;
    private string[] angryTexts = new string[] {
        "����Ҹ��",
        "���ٵ�һ�����Կ�",
        "���ܱ��ٵ�����",
        "�����ǹ���İɣ�",
        "����ˣ�",
        "����������ˣ�"
    };

    public void OnMouseDown()
    {
        
        if (!isCanvasVisible)
        {
            isCanvasVisible = true;
            Canvas.gameObject.SetActive(true);
            Invoke("HideText", 8f);
        }

        clickCount++;
        if (clickCount <= angryTexts.Length)
        {
            text.text = angryTexts[clickCount - 1];
        }
        else
        {
            text.text = "���Ѿ�������" + clickCount.ToString() + "���ˣ�";
        }
    }

    private void HideText()
    {
        isCanvasVisible = false;
        Canvas.gameObject.SetActive(false);
    }
}

