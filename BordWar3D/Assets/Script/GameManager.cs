using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public enum GameState
{
    PLAYERTURN,
    ENEMYTURN,
}

public enum PlayerType
{
    None,
    PLAYER,
    ENEMY,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数
    public GameState currentState;

    public GameConst.TurnPhase turnPhese;

    public bool select = false;
    public bool checkComp = false;

    private int basePosx, basePosy, movePosx, movePosy;

    private bool isGameEnd = false;
    private PlayerType winner = PlayerType.None;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInput();

        SetTurnPhese();
    }

    // チェック関数
    private void CheckGameEnd()
    {
        winner = PlayerType.None;
        // TODO 勝利判定ロジック
        // TODO リーダーユニットが撃破されているプレイヤーは負け

        if (winner != PlayerType.None)
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
    private void SetWinner(PlayerType playerType) { winner = playerType; }

    // 勝者を取得
    private PlayerType GetWinner() { return winner; }

    public void SetCurrentTurn(GameState newTurn)
    {
        currentState = newTurn;
        Debug.Log(newTurn);
    }

    public void SetTurnPhese()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (turnPhese)
            {
                case GameConst.TurnPhase.Start:
                    Debug.Log("TurnPhese:Action");
                    turnPhese = GameConst.TurnPhase.Action;
                    break;
                case GameConst.TurnPhase.Action:
                    Debug.Log("TurnPhese:End");
                    turnPhese = GameConst.TurnPhase.End;
                    break;
                case GameConst.TurnPhase.End:
                    Debug.Log("TurnPhese:Next");
                    turnPhese = GameConst.TurnPhase.Next;
                    break;
                case GameConst.TurnPhase.Next:
                    Debug.Log("TurnPhese:Start");
                    turnPhese = GameConst.TurnPhase.Start;
                    break;
            }
        }
    }

    public void GetPlayerInput()
    {

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Vector2Int clickedPos = Vector2Int.zero;
        if (!TryGetClickedPos(out clickedPos)) { return; }

        if (!select)
        {
            basePosx = clickedPos.x;
            basePosy = clickedPos.y;
            switch (currentState)
            {
                case GameState.PLAYERTURN:
                    BoardManager.Instance.CheckPlayerSelect(basePosx, basePosy);
                    Debug.Log("移動前" + (8 - basePosy) + " " + basePosx);
                    break;

                case GameState.ENEMYTURN:
                    BoardManager.Instance.CheckEnemySelect(basePosx, basePosy);
                    break;
            }
        }
        else
        {
            movePosx = clickedPos.x;
            movePosy = clickedPos.y;
            switch (currentState)
            {
                case GameState.PLAYERTURN:
                    Debug.Log("移動前" + (8 - basePosy) + " " + basePosx + " 移動後 " + (8 - movePosy) + " " + movePosx);
                    BoardManager.Instance.CheckPlayerMoveLegality(basePosx, basePosy, movePosx, movePosy);
                    if (BoardManager.Instance.error)
                    {
                        BoardManager.Instance.error = false;
                        select = false;
                        break;
                    }
                    if (select) { BoardManager.Instance.PieceMoveAnimation(movePosx, movePosy); }
                    select = false;
                    Debug.Log("select=false");
                    SetCurrentTurn(GameState.ENEMYTURN);
                    break;
                case GameState.ENEMYTURN:
                    BoardManager.Instance.CheckEnemyMoveLegality(basePosx, basePosy, movePosx, movePosy);
                    if (BoardManager.Instance.error)
                    {
                        BoardManager.Instance.error = false;
                        select = false;
                        break;
                    }
                    if (select) { BoardManager.Instance.PieceMoveAnimation(movePosx, movePosy); }
                    select = false;
                    SetCurrentTurn(GameState.PLAYERTURN);

                    break;
            }
        }
    }

    private bool TryGetClickedPos(out Vector2Int pos)
    {
        pos = Vector2Int.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100.0f) && hit.collider.gameObject.CompareTag("masu"))
        {
            clickedGameObject = hit.collider.gameObject;
            int x = (int)Mathf.Floor(clickedGameObject.transform.position.x);
            int y = (int)Mathf.Floor(clickedGameObject.transform.position.z);

            pos = new Vector2Int(x, y);
            return true;
        }

        return false;
    }

}

