using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.VisualScripting;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public GameObject deathObj;
    List<Vector2Int> moveRange = new List<Vector2Int>();
    List<Vector2Int> attackRange = new List<Vector2Int>();
    [SerializeField] public List<Column> tileRows = new List<Column>();
    [SerializeField] public List<BoardInfo> infoRows = new List<BoardInfo>();
    [SerializeField] GameObject selectFramePrefab;
    [SerializeField] GameObject bridgePrefab;
    [SerializeField] GameObject RockPrefab;
    [SerializeField] GameObject[] CommanderPrefab = new GameObject[2];
    [SerializeField] GameObject[] SniperPrefab = new GameObject[2];
    [SerializeField] GameObject[] MachineGunPrefab = new GameObject[2];
    [SerializeField] GameObject[] AssaultPrefab = new GameObject[2];
    [SerializeField] GameObject[] GrenadePrefab = new GameObject[2];

    [SerializeField] private GameObject moveRangeFramePrefab;
    [SerializeField] private GameObject attackRangeFramePrefab;
    [SerializeField] List<GameObject> moveRangeFrames = new List<GameObject>();
    [SerializeField] List<GameObject> attackRangeFrames = new List<GameObject>();
    private int rangeFrameObjCount = 20;


    public bool error;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomBridge();

        MakeFrame();

        InitializeBoard();



    }

    // Update is called once per frame
    void Update()
    {

    }
    //選んだ駒は自分の駒かチェック
    public void CheckPlayerSelect(int x, int y)
    {
        if (Array.IndexOf(PieceManager.Instance.playerPieceName, infoRows[8 - y].infoColumns[x]) + 1 > 0)
        {
            PieceManager.Instance.SetCurrentPiece(infoRows[8 - y].infoColumns[x]);
            Debug.Log(PieceManager.Instance.currentPieceClass);
            ActiveMoveFrame(PieceManager.Instance.ReturnMoveRange(x, y), x, y);
            ActiveAttackFrame(PieceManager.Instance.ReturnAttackRange(x, y), x, y);

            GameManager.Instance.select = true;
        }
        else if(Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - y].infoColumns[x]) + 1 > 0)
        {

        }
    }

    public void CheckEnemySelect(int x, int y)
    {
        if (Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - y].infoColumns[x]) + 1 > 0)
        {
            PieceManager.Instance.SetCurrentPiece(infoRows[8 - y].infoColumns[x]);
            ActiveMoveFrame(PieceManager.Instance.ReturnMoveRange(x, y), x, y);
            ActiveAttackFrame(PieceManager.Instance.ReturnAttackRange(x, y), x, y);

            GameManager.Instance.select = true;
        }
    }
    //移動できるかの判定
    //x1,y2=移動前の座標、x2,y2=移動後の座標
    public void CheckPlayerMoveLegality(int x1, int y1, int x2, int y2)
    {
        //移動先に障害物がないか？
        Debug.Log(x1 + " " + y1 + " " + x2 + " " + x2);
        for (int i = 0; i < 6; i++)
        {
            if (infoRows[8 - y2].infoColumns[x2] == PieceManager.Instance.playerPieceName[i]
            || infoRows[8 - y2].infoColumns[x2] == "River" || infoRows[8 - y2].infoColumns[x2] == "Rock"
            || x1 == x2 && y1 == y2)
            {
                error = true;
                GameManager.Instance.select = false;
                HideFrame();
                Debug.Log("error!!");
            }
        }
        //infoから取られる駒を取得
        for (int i = 0; i < 6; i++)
        {
            if (infoRows[8 - y2].infoColumns[x2] == PieceManager.Instance.enemyPieceName[i])
            {
                deathObj = GameObject.Find(infoRows[8 - y2].infoColumns[x2]);
            }
        }
        //問題がなければinfoを書き換える
        if (!error)
        {
            infoRows[8 - y2].infoColumns[x2] = infoRows[8 - y1].infoColumns[x1];
            infoRows[8 - y1].infoColumns[x1] = "";
            Debug.Log("移動しました！");
        }
        error = false;
    }

    public void CheckEnemyMoveLegality(int x1, int y1, int x2, int y2)
    {

        //移動先に自分の駒がいないか？

        Debug.Log(x1 + " " + y1 + " " + x2 + " " + x2);
        for (int i = 0; i < 6; i++)
        {
            if (infoRows[8 - y2].infoColumns[x2] == PieceManager.Instance.enemyPieceName[i] || infoRows[8 - y2].infoColumns[x2] == "River"
            || infoRows[8 - y2].infoColumns[x2] == "Rock"
            || x1 == x2 && y1 == y2)
            {
                error = true;
                GameManager.Instance.select = false;
                HideFrame();
                Debug.Log("error!!");
            }
        }
        //infoから取られる駒を取得
        for (int i = 0; i < 6; i++)
        {
            if (infoRows[8 - y2].infoColumns[x2] == PieceManager.Instance.playerPieceName[i])
            {
                deathObj = GameObject.Find(infoRows[8 - y2].infoColumns[x2]);
            }
        }
        //問題がなければinfoを書き換える
        if (!error)
        {
            infoRows[8 - y2].infoColumns[x2] = infoRows[8 - y1].infoColumns[x1];
            infoRows[8 - y1].infoColumns[x1] = "";
            Debug.Log("移動しました！");
        }
        error = false;
    }

    public void ChackAttackLegality(int x, int y)
    {
        bool findPiece = false;
        //選択した攻撃範囲のマスに相手の駒があるか判定
        switch (GameManager.Instance.currentState)
        {
            case GameConst.GameState.PLAYERTURN:
                //infoから取られる駒を取得
                for (int i = 0; i < 6; i++)
                {
                    if (infoRows[8 - y].infoColumns[x] == PieceManager.Instance.enemyPieceName[i])
                    {
                        findPiece = true;
                        deathObj = GameObject.Find(infoRows[8 - y].infoColumns[x]);
                    }

                    if (i == 5 && !findPiece)
                    {
                        error = true;
                        GameManager.Instance.select = false;
                        HideFrame();
                    }
                }
                break;
            case GameConst.GameState.ENEMYTURN:
                //infoから取られる駒を取得
                for (int i = 0; i < 6; i++)
                {
                    if (infoRows[8 - y].infoColumns[x] == PieceManager.Instance.playerPieceName[i])
                    {
                        findPiece = true;
                        deathObj = GameObject.Find(infoRows[8 - y].infoColumns[x]);
                    }

                    if (i == 5 && !findPiece)
                    {
                        error = true;
                        GameManager.Instance.select = false;
                        HideFrame();
                    }
                }
                break;
        }
        //問題がなければinfoを書き換える
        if (!error)
        {
            Debug.Log("Attack");
            infoRows[8 - y].infoColumns[x] = "";
        }
    }

    public void PieceAttack(int x, int y, System.Action callback)
    {
        Destroy(deathObj);
        callback?.Invoke();
    }

    //駒の移動とアニメーション
    public void PieceMoveAnimation(int x, int y, System.Action callback)
    {
        GameObject obj = GameObject.Find(infoRows[8 - y].infoColumns[x]);
        Vector3 pos = tileRows[8 - y].tileColumns[x].transform.position;

        Vector3 objPos = obj.transform.position;
        if (8 - y == 4)
        {
            objPos.y = 1.26f;
        }
        else
        {
            objPos.y = 0.96f;
        }

        obj.transform.DOPath(
            new[]
            {
                new Vector3((pos.x+objPos.x)/2,2f,(pos.z+objPos.z)/2),
                new Vector3(pos.x,objPos.y,pos.z)
            },
            1f, PathType.CatmullRom)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                Destroy(deathObj);
                callback?.Invoke();
            });

    }

    //範囲表示用のフレームを生成し、リストに保管
    void MakeFrame()
    {
        for (int i = 0; i < rangeFrameObjCount; i++)
        {
            GameObject obj = Instantiate(moveRangeFramePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, gameObject.transform);
            obj.SetActive(false);
            moveRangeFrames.Add(obj);

            obj = Instantiate(attackRangeFramePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, gameObject.transform);
            obj.SetActive(false);
            attackRangeFrames.Add(obj);
        }

    }

    //移動可能な座標リストを元にフレームを必要な数だけ表示し、配置する
    void ActiveMoveFrame(List<Vector2Int> range, int x, int y)
    {
        for (int i = 0; i < range.Count; i++)
        {

            Vector3 pos;
            pos.x = range[i].x;
            pos.z = range[i].y;
            int infoX = range[i].x;
            int infoY = range[i].y;

            //もしボードの範囲外の座標だったら表示させず、次の座標の処理へ
            if (infoX > 7 || infoY > 8 || infoX < 0 || infoY < 0) { continue; }

            //移動先が空いていて、川でなければフレームを表示する(相手の駒がいる場合も表示)
            switch (GameManager.Instance.currentState)
            {
                case GameConst.GameState.PLAYERTURN:
                    if (infoRows[8 - infoY].infoColumns[infoX] == "" || Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0
                    && infoRows[8 - infoY].infoColumns[infoX] != "River")
                    {
                        moveRangeFrames[i].SetActive(true);
                    }
                    break;

                case GameConst.GameState.ENEMYTURN:
                    if (infoRows[8 - infoY].infoColumns[infoX] == "" || Array.IndexOf(PieceManager.Instance.playerPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0
                    && infoRows[8 - infoY].infoColumns[infoX] != "River")
                    {
                        moveRangeFrames[i].SetActive(true);
                    }
                    break;

            }

            //Assault用判定(岩や川、駒を飛び越えられないようにする)
            //上下左右のマスに障害物がないか判定
            if (PieceManager.Instance.currentPieceClass == GameConst.pieceClass.Assault && i > 7)
            {
                if (infoRows[8 - infoY].infoColumns[infoX] == "River" || infoRows[8 - infoY].infoColumns[infoX] == "Rock"
                || Array.IndexOf(PieceManager.Instance.playerPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0
                || Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0)
                {
                    moveRangeFrames[i - 4].SetActive(false);
                }
            }
            //斜め4方向に進めるか判定
            if (PieceManager.Instance.currentPieceClass == GameConst.pieceClass.Assault && i > 8)
            {
                if (!moveRangeFrames[i - 1].activeSelf && !moveRangeFrames[i].activeSelf)
                {
                    moveRangeFrames[i - 8].SetActive(false);
                }
            }

            //移動先が橋の場合は橋の高さの分、フレームの位置を調整する
            if (pos.z == 4 && infoRows[y].infoColumns[x] != "River")
            {
                moveRangeFrames[i].transform.position = new Vector3(pos.x, 0f, pos.z);
            }
            else
            {
                moveRangeFrames[i].transform.position = new Vector3(pos.x, -0.3f, pos.z);
            }
        }
        //Assault用判定(左上のマスに進めるか判定)
        if (PieceManager.Instance.currentPieceClass == GameConst.pieceClass.Assault
            && !moveRangeFrames[8].activeSelf && !moveRangeFrames[11].activeSelf)
        {
            moveRangeFrames[0].SetActive(false);
        }
    }
    //攻撃範囲の表示
    void ActiveAttackFrame(List<Vector2Int> range, int x, int y)
    {
        for (int i = 0; i < range.Count; i++)
        {

            Vector3 pos;
            pos.x = range[i].x;
            pos.z = range[i].y;
            int infoX = range[i].x;
            int infoY = range[i].y;
            bool activeComplete = false;

            //もしボードの範囲外の座標だったら表示させず、次の座標の処理へ
            if (infoX > 7 || infoY > 8 || infoX < 0 || infoY < 0) { continue; }

            //移動先が橋の場合は橋の高さの分、フレームの位置を調整する
            if (pos.z == 4 && infoRows[y].infoColumns[x] != "River")
            {
                attackRangeFrames[i].transform.position = new Vector3(pos.x, 0.35f, pos.z);
            }
            else
            {
                attackRangeFrames[i].transform.position = new Vector3(pos.x, 0.03f, pos.z);
            }

            //MachineGunの攻撃が障害物や駒を貫通しないようにする
            if (PieceManager.Instance.currentPieceClass == GameConst.pieceClass.MachineGun)
            {
                if (i > 0 && !attackRangeFrames[i - 1].activeSelf && i % 3 != 0) { continue; }
            }

            switch (PieceManager.Instance.currentPieceClass)
            {
                case GameConst.pieceClass.Sniper1P:
                    if (infoRows[8 - infoY].infoColumns[infoX] == "Rock"
                    || Array.IndexOf(PieceManager.Instance.playerPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0)
                    {
                        activeComplete = true;
                    }
                    else if (Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0)
                    {
                        attackRangeFrames[i].SetActive(true);
                        activeComplete = true;
                    }
                    break;
                case GameConst.pieceClass.Sniper2P:
                    if (infoRows[8 - infoY].infoColumns[infoX] == "Rock"
                    || Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0)
                    {
                        activeComplete = true;
                    }
                    else if (Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0)
                    {
                        attackRangeFrames[i].SetActive(true);
                        activeComplete = true;
                    }
                    break;
            }

            if (activeComplete) { break; }

            //移動先が空いていれば表示する(相手の駒がいる場合も表示,味方の駒がいるときは表示しない)
            switch (GameManager.Instance.currentState)
            {
                case GameConst.GameState.PLAYERTURN:
                    if (infoRows[8 - infoY].infoColumns[infoX] == "" || infoRows[8 - infoY].infoColumns[infoX] == "River")
                    {
                        attackRangeFrames[i].SetActive(true);
                    }
                    else if (Array.IndexOf(PieceManager.Instance.enemyPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0)
                    {
                        attackRangeFrames[i].SetActive(true);
                        i++;
                    }
                    break;

                case GameConst.GameState.ENEMYTURN:
                    if (infoRows[8 - infoY].infoColumns[infoX] == "" || infoRows[8 - infoY].infoColumns[infoX] == "River")
                    {
                        attackRangeFrames[i].SetActive(true);
                    }
                    else if (Array.IndexOf(PieceManager.Instance.playerPieceName, infoRows[8 - infoY].infoColumns[infoX]) + 1 > 0)
                    {
                        attackRangeFrames[i].SetActive(true);
                        i++;
                    }
                    break;
            }
        }
    }

    //駒の選択が解除されたときに表示されているフレームを全て隠す
    public void HideFrame()
    {

        for (int i = 0; i < rangeFrameObjCount - 1; i++)
        {
            moveRangeFrames[i].SetActive(false);
            attackRangeFrames[i].SetActive(false);
        }
    }

    //駒の初期配置
    void InitializeBoard()
    {
        Vector3 pos = new Vector3();

        for (int i = 0; i < infoRows.Count; i++)
        {
            for (int j = 0; j < infoRows[i].infoColumns.Count; j++)
            {
                pos = tileRows[i].tileColumns[j].transform.position;

                //1,2で1P,2Pカラーを差別化
                //突撃兵(Assault)は複数いるのでIDを振ってます
                //1P
                if (infoRows[i].infoColumns[j] == "Rock")
                {
                    pos.y = 0.86f;
                    Instantiate(RockPrefab, pos, Quaternion.identity);
                }
                else if (infoRows[i].infoColumns[j] == "Commander1")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(CommanderPrefab[0], pos, Quaternion.identity);
                    obj.name = "Commander1";
                }
                else if (infoRows[i].infoColumns[j] == "Sniper1")
                {
                    pos.y = 0.92f;
                    var obj = Instantiate(SniperPrefab[0], pos, Quaternion.identity);
                    obj.name = "Sniper1";
                }
                else if (infoRows[i].infoColumns[j] == "MachineGun1")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(MachineGunPrefab[0], pos, Quaternion.identity);
                    obj.name = "MachineGun1";
                }
                else if (infoRows[i].infoColumns[j] == "Assault1_A")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(AssaultPrefab[0], pos, Quaternion.identity);
                    obj.name = "Assault1_A";
                }
                else if (infoRows[i].infoColumns[j] == "Assault1_B")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(AssaultPrefab[1], pos, Quaternion.identity);
                    obj.name = "Assault1_B";
                }
                else if (infoRows[i].infoColumns[j] == "Grenade1")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(GrenadePrefab[0], pos, Quaternion.identity);
                    obj.name = "Grenade1";
                }
                //2P
                else if (infoRows[i].infoColumns[j] == "Commander2")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(CommanderPrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                    obj.name = "Commander2";
                }
                else if (infoRows[i].infoColumns[j] == "Sniper2")
                {
                    pos.y = 0.92f;
                    var obj = Instantiate(SniperPrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                    obj.name = "Sniper2";
                }
                else if (infoRows[i].infoColumns[j] == "MachineGun2")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(MachineGunPrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                    obj.name = "MachineGun2";
                }
                else if (infoRows[i].infoColumns[j] == "Assault2_A")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(AssaultPrefab[2], pos, Quaternion.Euler(0, 180f, 0));
                    obj.name = "Assault2_A";
                }
                else if (infoRows[i].infoColumns[j] == "Assault2_B")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(AssaultPrefab[3], pos, Quaternion.Euler(0, 180f, 0));
                    obj.name = "Assault2_B";
                }
                else if (infoRows[i].infoColumns[j] == "Grenade2")
                {
                    pos.y = 0.96f;
                    var obj = Instantiate(GrenadePrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                    obj.name = "Grenade2";
                }
            }
        }
    }

    //ランダムに橋を配置
    void RandomBridge()
    {
        int[] num = new int[2];
        Vector3[] pos = new Vector3[2];

        //ランダムな値の取得
        for (int i = 0; i < 2; i++)
        {
            num[i] = Random.Range(0, 6);
        }
        //重複していた時の処理
        while (num[0] == num[1])
        {
            num[1] = Random.Range(0, 6);
        }

        //橋を生成
        for (int i = 0; i < 2; i++)
        {
            if (num[i] >= 3)
            {
                num[i] += 2;
            }
            pos[i] = tileRows[4].tileColumns[num[i]].transform.position;
            pos[i].y = 0.6f;
            infoRows[4].infoColumns[num[i]] = "";
            Instantiate(bridgePrefab, pos[i], Quaternion.identity);
        }
    }
}



[System.Serializable]
public class Column
{
    public List<GameObject> tileColumns;
}

[System.Serializable]
public class BoardInfo
{
    public List<string> infoColumns;
}