using UnityEngine;
using TMPro;

public class ClickScript : MonoBehaviour
{
    public GameObject Canvas;
    public TextMeshProUGUI text;
    private int clickCount = 0;
    private bool isCanvasVisible = false;
    private string[] angryTexts = new string[] {
        "你点我干嘛？",
        "你再点一次试试看",
        "你能别再点了吗？",
        "你这是故意的吧？",
        "别点了！",
        "我真的生气了！"
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
            text.text = "你已经点了我" + clickCount.ToString() + "次了！";
        }
    }

    private void HideText()
    {
        isCanvasVisible = false;
        Canvas.gameObject.SetActive(false);
    }
}

