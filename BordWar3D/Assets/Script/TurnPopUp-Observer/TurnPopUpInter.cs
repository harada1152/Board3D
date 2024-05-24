public interface TPUSubject // TPUはTurnPopUPの略
{
    void Attach(TPUObserver observer);
    void Detach(TPUObserver observer);
    void Notify(string playMode, string currentTurn);
}

public interface TPUObserver
{
    // 引数1には「プレイヤーVSAI」か「プレイヤーVSプレイヤー」のどちらのモードかをbool型でいれる。(ゲームマネージャー参照)
    // 引数2にはどちらのターンかをstring型で入れる。(ゲームマネージャー参照)
    void OnNotify(string PlayMode, string CurrentTurn);
}