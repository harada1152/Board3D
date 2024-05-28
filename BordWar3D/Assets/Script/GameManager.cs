using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PLAYERTURN,
    ENEMYTURN,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数

    public GameState currentState;

    public bool select = false;
    public bool checkComp = false;

    private int basePosx, basePosy, movePosx, movePosy;

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
    }

    public void SetCurrentTurn(GameState newTurn)
    {
        currentState = newTurn;

        switch (currentState)
        {
            case GameState.PLAYERTURN:

                break;
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
                    BoardManager.Instance.CheckEnemySelect(basePosy, basePosx);
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
                    if (select) { BoardManager.Instance.PieceMoveAnimation(movePosx, movePosy); }
                    select = false;
                    Debug.Log("select=false");
                    break;
                case GameState.ENEMYTURN:
                    BoardManager.Instance.CheckEnemyMoveLegality(basePosx, basePosy, movePosx, movePosy);
                    BoardManager.Instance.PieceMoveAnimation(movePosx, movePosy);
                    select = false;

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

