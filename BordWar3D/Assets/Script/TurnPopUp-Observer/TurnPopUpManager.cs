using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TurnPopUpManager : MonoBehaviour
    {
        [SerializeField] TurnPopUpSubject TPUsubject;
        [SerializeField] TurnPopUpObserver_PvAI TPUobserver_PvAI;
        [SerializeField] TurnPopUpObserver_PvP TPUobserver_PvP;
        private string formerTurn = ""; // 前のターンを保存


        void Start()
        {
            TPUsubject.Attach(TPUobserver_PvAI);
            TPUsubject.Attach(TPUobserver_PvP);
        }

        void Update()
        {
            if(UIManager.Instance.currentTurn != formerTurn)
            {
                TPUsubject.Notify(UIManager.Instance.playMode, UIManager.Instance.currentTurn);
                formerTurn = UIManager.Instance.currentTurn;
            }
        }
    }
}
