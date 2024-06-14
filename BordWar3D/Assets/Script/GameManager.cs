using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // [HideInInspector] 
    public bool select = false;
    public GameConst.GameState currentState;
    private GameConst.TurnPhase beforeTurnPhase;
    private GameConst.TurnPhase currentTurnPhese;
    [SerializeField] private GameConst.ActionType currentActionType;
    private int basePosx, basePosy, actionPosx, actionPosy;
    private bool isGameEnd = false;
    private GameConst.PlayerType winner = GameConst.PlayerType.None;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // TODO 先攻後攻システム？
        // TODO ゲーム開始処理演出？
        currentState = GameConst.GameState.PLAYERTURN;
        beforeTurnPhase = GameConst.TurnPhase.Next; // スタートフェイズの初期化処理を有効化するため
        SetTurnPhese(GameConst.TurnPhase.Start);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO ゲームが終了した場合の処理
        if (IsGameEnd())
        {
            // TODO 終了演出など
            Debug.Log($"Winner : {GetWinner()}");
            return;
        }

        // フェイズの移り変わり時に一度だけ呼び出される関数群
        if (HasPhaseChanged())
        {
            switch (currentTurnPhese)
            {
                case GameConst.TurnPhase.Start:
                    InitializeStartPhase();
                    break;
                case GameConst.TurnPhase.Action:
                    InitializeActionPhase();
                    break;
                case GameConst.TurnPhase.End:
                    InitializeEndPhase();
                    break;
                case GameConst.TurnPhase.Next:
                    InitializeNextPhase();
                    break;
            }
            ResetPhaseChangeFlag();

            return;
        }

        // 各フェイズの更新関数
        switch (currentTurnPhese)
        {
            case GameConst.TurnPhase.Start:
                UpdateStartPhase();
                break;
            case GameConst.TurnPhase.Action:
                UpdateActionPhase();
                break;
            case GameConst.TurnPhase.End:
                UpdateEndPhase();
                break;
            case GameConst.TurnPhase.Next:
                UpdateNextPhase();
                break;
        }
    }

    // スタートフェイズの初期化処理
    private void InitializeStartPhase()
    {
        Debug.Log("TurnPhese:Start");
        // TODO 初期化処理

        // TODO 仮でフェイズ遷移を追加
        DOVirtual.DelayedCall(0.5f, () => { SetTurnPhese(GameConst.TurnPhase.Action); });
    }

    // TODO スタートフェイズ中の処理
    private void UpdateStartPhase() { }

    // アクションフェイズの初期化処理
    private void InitializeActionPhase() { Debug.Log("TurnPhese:Action"); }

    // アクションフェイズ中の処理
    private void UpdateActionPhase()
    {
        SetActionType();
        // クリック取得がなければ処理なし
        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) { return; }

        // クリック取得後にマス情報が無ければ処理なし
        Vector2Int clickedPos = Vector2Int.zero;
        if (!TryGetClickedPos(out clickedPos)) { return; }

        // 取得したマス情報から入力処理を開始
        BaseInput(clickedPos);

        // アニメーション終了後のコールバック
        void OnCompleteAnimationCallback() => SetTurnPhese(GameConst.TurnPhase.End);

        switch (currentState)
        {
            case GameConst.GameState.PLAYERTURN:
                PlayerInput(OnCompleteAnimationCallback);
                break;
            case GameConst.GameState.ENEMYTURN:
                EnemyInput(OnCompleteAnimationCallback);
                break;
            default:
                break;
        }
    }

    // エンドフェイズの初期化処理
    private void InitializeEndPhase()
    {
        Debug.Log("TurnPhese:End");

        // 勝利判定
        CheckGameEnd();

        // ゲームが終了していたら処理終了
        if (IsGameEnd()) { return; }

        // TODO 仮でフェイズ遷移を追加
        DOVirtual.DelayedCall(0.1f, () => { SetTurnPhese(GameConst.TurnPhase.Next); });
    }

    // TODO エンドフェイズ中の処理
    private void UpdateEndPhase() { }

    // ネクストフェイズの初期化処理
    private void InitializeNextPhase()
    {
        Debug.Log("TurnPhese:Next");
        // TODO 初期化処理

        // TODO 仮でフェイズ遷移を追加
        DOVirtual.DelayedCall(0.5f, () =>
        {
            SwitchToNextTurn();
            SetTurnPhese(GameConst.TurnPhase.Start);
        });
    }

    // TODO ネクストフェイズ中の処理
    private void UpdateNextPhase() { }

    // チェック関数
    private void CheckGameEnd()
    {
        winner = GameConst.PlayerType.None;

        CheckVictory();
        // TODO 勝利判定ロジック
        // TODO リーダーユニットが撃破されているプレイヤーは負け

        if (winner != GameConst.PlayerType.None)
        {
            GameEnd();
            SetWinner(winner);
        }
    }

    // ゲーム終了済みか
    private bool IsGameEnd() { return isGameEnd; }

    // ゲーム終了関数
    private void GameEnd() { isGameEnd = true; }

    // 勝者を決定
    private void SetWinner(GameConst.PlayerType playerType) { winner = playerType; }

    // 勝者を取得
    private GameConst.PlayerType GetWinner() { return winner; }

    // ターンを設定
    public void SetCurrentTurn(GameConst.GameState newTurn)
    {
        currentState = newTurn;
        Debug.Log(newTurn);
    }

    // ターンの切り替え処理
    private void SwitchToNextTurn()
    {
        switch (currentState)
        {
            case GameConst.GameState.PLAYERTURN:
                CameraController.Instance.ChangeCameraPos();
                SetCurrentTurn(GameConst.GameState.ENEMYTURN);
                break;
            case GameConst.GameState.ENEMYTURN:
                CameraController.Instance.ChangeCameraPos();
                SetCurrentTurn(GameConst.GameState.PLAYERTURN);
                break;
            default:
                SetCurrentTurn(GameConst.GameState.PLAYERTURN);
                break;
        }
    }

    // 現在のフェイズを設定
    private void SetTurnPhese(GameConst.TurnPhase turnPhase) { currentTurnPhese = turnPhase; }

    // フェイズの切り替わりを検知
    private bool HasPhaseChanged() { return currentTurnPhese != beforeTurnPhase; }

    // フェイズの初期化フラグを下す
    private void ResetPhaseChangeFlag() { beforeTurnPhase = currentTurnPhese; }

    private void SetActionType()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentActionType = GameConst.ActionType.Move;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            currentActionType = GameConst.ActionType.Attack;
        }
    }

    // クリック情報から、マス情報の取得を試みる
    private bool TryGetClickedPos(out Vector2Int pos)
    {
        pos = Vector2Int.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (!select && Physics.Raycast(ray, out hit, 100.0f) && hit.collider.gameObject.CompareTag("masu")
        || select && currentActionType == GameConst.ActionType.Move && Physics.Raycast(ray, out hit, 100.0f) && hit.collider.gameObject.CompareTag("MoveRangeFrame")
        || select && currentActionType == GameConst.ActionType.Attack && Physics.Raycast(ray, out hit, 100.0f) && hit.collider.gameObject.CompareTag("AttackRangeFrame"))
        {
            GameObject clickedGameObject = hit.collider.gameObject;
            int x = (int)Mathf.Floor(clickedGameObject.transform.position.x);
            int y = (int)Mathf.Floor(clickedGameObject.transform.position.z);

            pos = new Vector2Int(x, y);
            return true;
        }
        else
        {
            select = false;
            BoardManager.Instance.HideFrame();
        }
        return false;
    }

    // クリック時のマス情報を保持
    private void BaseInput(Vector2Int clickedPos)
    {
        if (!select)
        {
            basePosx = clickedPos.x;
            basePosy = clickedPos.y;
            Debug.Log("移動前" + (8 - basePosy) + " " + basePosx);
        }
        else
        {
            actionPosx = clickedPos.x;
            actionPosy = clickedPos.y;
            Debug.Log("移動前" + (8 - basePosy) + " " + basePosx + " 移動後 " + (8 - actionPosy) + " " + actionPosx);
        }
    }

    // 選択後の処理
    private void BaseAfterInput(System.Action onCompleteCallback)
    {
        if (!select) return;

        if (BoardManager.Instance.error)
        {
            BoardManager.Instance.error = false;
            BoardManager.Instance.HideFrame();
            select = false;
            return;
        }

        if (select && currentActionType == GameConst.ActionType.Move)
        {
            BoardManager.Instance.PieceMoveAnimation(actionPosx, actionPosy, onCompleteCallback);
        }
        else if (select && currentActionType == GameConst.ActionType.Attack)
        {
            Debug.Log("Attack!!");
            BoardManager.Instance.PieceAttack(actionPosx,actionPosy, onCompleteCallback);
        }
        BoardManager.Instance.HideFrame();
        select = false;
    }

    // 保持したマス情報をプレイヤー処理へ
    private void PlayerInput(System.Action onCompleteCallback)
    {
        if (!select)
            BoardManager.Instance.CheckPlayerSelect(basePosx, basePosy);
        else
        {
            switch (currentActionType)
            {
                case GameConst.ActionType.Move:
                    BoardManager.Instance.CheckPlayerMoveLegality(basePosx, basePosy, actionPosx, actionPosy);
                    BaseAfterInput(onCompleteCallback);
                    break;
                case GameConst.ActionType.Attack:
                    BoardManager.Instance.ChackAttackLegality(actionPosx, actionPosy);
                    BaseAfterInput(onCompleteCallback);
                    break;
            }

        }
    }

    // 保持したマス情報をエネミー処理へ
    private void EnemyInput(System.Action onCompleteCallback)
    {
        if (!select)
            BoardManager.Instance.CheckEnemySelect(basePosx, basePosy);
        else
        {
            switch (currentActionType)
            {
                case GameConst.ActionType.Move:
                    BoardManager.Instance.CheckEnemyMoveLegality(basePosx, basePosy, actionPosx, actionPosy);
                    BaseAfterInput(onCompleteCallback);
                    break;
                case GameConst.ActionType.Attack:
                    BoardManager.Instance.ChackAttackLegality(actionPosx, actionPosy);
                    BaseAfterInput(onCompleteCallback);
                    break;
            }
        }
    }

    private void CheckVictory()
    {
        switch(currentState)
        {
            case GameConst.GameState.PLAYERTURN:
            if(GameObject.Find("Commander2(Clone)")==null)
            {
                Debug.Log("Player1Win!!");
                winner= GameConst.PlayerType.PLAYER;
            }
            break;
            case GameConst.GameState.ENEMYTURN:
            if(GameObject.Find("Commander1(Clone)")== null)
            {
                Debug.Log("Player2Win!!");
                winner= GameConst.PlayerType.ENEMY;
            }
            break;
        }
    }
}
