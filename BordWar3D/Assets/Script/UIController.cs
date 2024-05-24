using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        void Update()
        {
            UIManager.Instance.timeLimitBarManager.TimeLimitBar();
            // ターンポップアップオブザーバーチェック用コード
            if(Input.GetMouseButtonUp(0))
            {
                UIManager.Instance.currentTurn = "AI";
            }
        }
    }
}
