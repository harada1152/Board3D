using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string loadScene;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private float fadeSpeedMultiplier = 1.0f;
    public void OnClick()
    {
        Time.timeScale = 1;
        Initiate.Fade(loadScene, fadeColor, fadeSpeedMultiplier);
    }
}
