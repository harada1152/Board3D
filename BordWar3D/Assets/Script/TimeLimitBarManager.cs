using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimeLimitBarManager : MonoBehaviour
    {
        [SerializeField] GameObject timeLimitBarInSide_Top;
        [SerializeField] GameObject timeLimitBarInSide_Middle;
        [SerializeField] GameObject timeLimitBarInSide_Bottom;
        [SerializeField] float constantTime;

        private Coroutine coroutine;

        private RectTransform TimeLimitBarInSide_Middle_TF = new RectTransform();

        void Awake()
        {
            TimeLimitBarInSide_Middle_TF = timeLimitBarInSide_Middle.GetComponent<RectTransform>();
        }

        public void TimeLimitBar()
        {
            // 現在のターンが自分のターンかつポップアップが消えた場合表示。
            // プレイヤー切り替えの時はプレイヤー１またはプレイヤー２のターンかつポップアップが消えた場合表示。
            // 現在のターン取得はゲームマネージャーからにしたいため要相談
            if(true && true)
            {
                // TimeLimitBarInSide_Topがアクティブの場合非アクティブにする
                if(timeLimitBarInSide_Top.activeSelf == true) {timeLimitBarInSide_Top.SetActive(false);}
                // バーを徐々に減少させるコルーチンを開始させる
                if(coroutine == null)
                {
                    coroutine = StartCoroutine(DecreaseTimeLimitBar(constantTime));
                }
            }
        }

        // バーのサイズを設定
        private void SetTimeLimitBarSize(float normalizedSize)
        {
            // 親オブジェクトの高さを基準にする
            float parentHeight = TimeLimitBarInSide_Middle_TF.parent.GetComponent<RectTransform>().rect.height;

            TimeLimitBarInSide_Middle_TF.sizeDelta = new Vector2(TimeLimitBarInSide_Middle_TF.sizeDelta.x, parentHeight * normalizedSize);
        }

        private void TimeLimitBarAllActibe()
        {
            timeLimitBarInSide_Top.SetActive(true);
            timeLimitBarInSide_Middle.SetActive(true);
            timeLimitBarInSide_Bottom.SetActive(true);
        }

        // 制限時間をバーで表すコルーチン
        private IEnumerator DecreaseTimeLimitBar(float constantTime)
        {
            float startTime = Time.time; // 開始時間
            while(Time.time < startTime + constantTime)// 開始時間＋制限時間が現在時間より大きい場合ループ
            {
                float elapsed = Time.time - startTime; // 経過時間
                // 1.0fから0.0fの値の間を線形補完（補完値は経過時間/制限時間）した値をサイズとして取得
                float normalizedSize = Mathf.Lerp(1.0f, 0.0f, elapsed / constantTime);
                SetTimeLimitBarSize(normalizedSize);
                yield return null;
            }
            SetTimeLimitBarSize(0.0f);
            timeLimitBarInSide_Middle.SetActive(false);
            timeLimitBarInSide_Bottom.SetActive(false);
            coroutine = null;
        }
    }
}
