using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public void LoadScene(string PurposeScene)
    {
        SceneManager.LoadScene(PurposeScene);
    }
}
