using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject namberObj;
    void Awake()
    {
        Instance = this;
    }

    public void ChangeCameraPos()
    {
        switch (GameManager.Instance.currentState)
        {
            case GameConst.GameState.PLAYERTURN:
                namberObj.SetActive(false);
                transform.DOLocalRotate(new Vector3(0, 180, 0), 0.7f)
                .OnComplete(() =>
                {
                    namberObj.SetActive(true);
                });
                break;
            case GameConst.GameState.ENEMYTURN:
                namberObj.SetActive(false);
                transform.DOLocalRotate(new Vector3(0, 360, 0), 0.7f)
                .OnComplete(() =>
                {
                    namberObj.SetActive(true);
                });
                break;
        }
    }
}
