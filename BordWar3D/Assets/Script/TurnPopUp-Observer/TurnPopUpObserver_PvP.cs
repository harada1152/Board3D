using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPopUpObserver_PvP : MonoBehaviour, TPUObserver
{
    [SerializeField] GameObject TPUObj;
    // [SerializeField] Sprite player1TurnSprite;
    // [SerializeField] Sprite player2TurnSprite;

    private Image TPUimage;

    void Awake()
    {
        TPUimage = TPUObj.GetComponent<Image>();
    }

    public void OnNotify(string playMode, string CurrentTurn)
    {
        if(playMode == "PvP") // プレイヤーVSプレイヤー
        {
            StartCoroutine(TPUCoroutine(CurrentTurn));
        }
        else {Debug.Log("No PvP");}
    }

    // クリック時に消える場合はコルーチンをwhile文でクリックされない間trueにする
    private IEnumerator TPUCoroutine(string CurrentTurn)
    {
        if(CurrentTurn == "Player1")
        {
            TPUObj.SetActive(true);
            Debug.Log("Player1");
            // TPUimageのSpriteをplayer1TurnSpriteに変える
            // TPUimage.sprite = player1TurnSprite;
        }
        else if(CurrentTurn == "player2")
        {
            TPUObj.SetActive(true);
            Debug.Log("Player2");
            // TPUimageのSpriteをplayer2TurnSpriteに変える
            // TPUimage.sprite = player2TurnSprite;
        }
        else {Debug.LogError("Neither [Player1] or [Player2]");}
        yield return new WaitForSeconds(3); // 一定時間で消える場合はここをいじる
        TPUObj.SetActive(false);
        yield return null;
    } 
}
