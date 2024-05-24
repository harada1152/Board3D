using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        // 別のクラスからUIManagerの変数などを使えるようにするためのもの。（変更はできない）
        public static UIManager Instance { get; private set;}
        public TimeLimitBarManager timeLimitBarManager;

        // 本来はゲームマネージャーから参照させる
        [NonSerialized] public string playMode = "PvAI"; // タイトルなどでプレイモードを選択する時などに中身を[PvAI]か[PvP]にする
        [NonSerialized] public string currentTurn = "Player"; // 必ず自分のターンが先行になるのか、ランダム(もしくはゲーム内のじゃんけん等)で先攻が決まるのか確認をとる

        void Awake()
        {
            // これで、別のクラスからUIManagerの変数などを使えるようになる。
            Instance = this;
        }
    }
}
