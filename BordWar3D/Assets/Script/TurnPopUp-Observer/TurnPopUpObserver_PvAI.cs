using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPopUpObserver_PvAI : MonoBehaviour, TPUObserver
{
    [SerializeField] GameObject TPUObj;
    // [SerializeField] Sprite MyTurnSprite;
    // [SerializeField] Sprite AITurnSprite;
    private Image TPUimage;

    void Awake()
    {
        TPUimage = TPUObj.GetComponent<Image>();
    }
    public void OnNotify(string playMode, string CurrentTurn)
    {
        if(playMode == "PvAI") // プレイヤーVSAI
        {
            StartCoroutine(TPUCoroutine(CurrentTurn));
        }
        else {Debug.Log("No PvAI");}
    }
    // クリック時に消える場合はコルーチンをwhile文でクリックされない間trueにする
    private IEnumerator TPUCoroutine(string CurrentTurn)
    {
        if(CurrentTurn == "Player")
        {
            TPUObj.SetActive(true);
            Debug.Log("Player");
            // TPUimageのSpriteをMyTurnSpriteに変える
            // TPUimage.sprite = MyTurnSprite;
        }
        else if(CurrentTurn == "AI")
        {
            TPUObj.SetActive(true);
            Debug.Log("AI");
            // TPUimageのSpriteをAITurnSpriteに変える
            // TPUimage.sprite = AITurnSprite;
        }
        else {Debug.Log(CurrentTurn);Debug.LogError("Neither [Player] or [AI]");}
        yield return new WaitForSeconds(3); // 一定時間で消える場合はここをいじる
        TPUObj.SetActive(false);
        yield return null;
    } 
}
