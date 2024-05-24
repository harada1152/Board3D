using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TurnPopUpSubject : MonoBehaviour, TPUSubject
    {
        private List<TPUObserver> observers = new List<TPUObserver>();

        public void Attach(TPUObserver observer)
        {
            observers.Add(observer);
        }
        public void Detach(TPUObserver observer)
        {
            observers.Remove(observer);
        }
        public void Notify(string playMode, string currentTurn)
        {
            foreach(TPUObserver observer in observers)
            {
                observer.OnNotify(playMode, currentTurn);
            }
        }
    }
}
