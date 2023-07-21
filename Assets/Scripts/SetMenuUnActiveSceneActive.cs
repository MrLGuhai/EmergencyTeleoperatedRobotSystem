using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMenuUnActiveSceneActive : MonoBehaviour
{
    public GameObject NearMenu3x1;
    public GameObject SceneDescriptionPanelRev;

    public void SetActivateObject()
    {
        //NearMenu3x1.SetActive(false);
        NearMenu3x1.SetActive(false);
        SceneDescriptionPanelRev.SetActive(true);
    }

}
