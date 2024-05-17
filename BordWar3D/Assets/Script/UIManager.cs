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

        void Awake()
        {
            // これで、別のクラスからUIManagerの変数などを使えるようになる。
            Instance = this;
        }
    }
}
