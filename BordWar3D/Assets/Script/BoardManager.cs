using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public GameObject deathObj;
    [SerializeField] List<Column> tileRows = new List<Column>();
    [SerializeField] List<BoardInfo> infoRows = new List<BoardInfo>();
    [SerializeField] GameObject bridgePrefab;
    [SerializeField] GameObject RockPrefab;
    [SerializeField] GameObject[] CommanderPrefab = new GameObject[2];
    [SerializeField] GameObject[] SniperPrefab = new GameObject[2];
    [SerializeField] GameObject[] MachineGunPrefab = new GameObject[2];
    [SerializeField] GameObject[] AssaultPrefab = new GameObject[2];
    [SerializeField] GameObject[] GrenadePrefab = new GameObject[2];
    

    public bool error;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomBridge();

        InitializeBoard();

    }

    // Update is called once per frame
    void Update()
    {

    }
    //選んだ駒は自分の駒か？
    public void CheckPlayerSelect(int x, int y)
    {
        Debug.Log(infoRows[8-y].infoColumns[x]);
        int num=Array.IndexOf(PieceManager.Instance.playerPieceName,infoRows[8-y].infoColumns[x])+1;
        if (0 <num)
        {
            Debug.Log("Select=true");
            GameManager.Instance.select = true;
        }
    }

    public void CheckEnemySelect(int x, int y)
    {
        Debug.Log(infoRows[8-y].infoColumns[x]);
        int num=Array.IndexOf(PieceManager.Instance.enemyPieceName,infoRows[8-y].infoColumns[x])+1;
        if (0 <num )
        {
            Debug.Log("Select=true");
            GameManager.Instance.select = true;
        }
    }
    //移動できるかの判定
    //x1,y2=移動前の座標、x2,y2=移動後の座標
    public void CheckPlayerMoveLegality(int x1, int y1, int x2, int y2)
    {
        //移動先に自分の駒がいないか？
        Debug.Log(x1+" "+y1+" "+x2+" "+x2);
        for (int i = 0; i < 6; i++)
        {
            if (infoRows[8-y2].infoColumns[x2] == PieceManager.Instance.playerPieceName[i]||infoRows[8-y2].infoColumns[x2] == "River"||infoRows[8-y2].infoColumns[x2] =="Rock"
            ||x1==x2&&y1==y2)
            {
                error = true;
                GameManager.Instance.select=false;
                Debug.Log("error!!");
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if(infoRows[8-y2].infoColumns[x2] == PieceManager.Instance.enemyPieceName[i])
            {
                deathObj=GameObject.Find(infoRows[8-y2].infoColumns[x2]+"(Clone)");
            }
        }
        //問題がなければinfoを書き換える
        if (!error)
        {
            infoRows[8-y2].infoColumns[x2]=infoRows[8-y1].infoColumns[x1];
            infoRows[8-y1].infoColumns[x1]="";
            Debug.Log("移動しました！");
        }
    }

    public void CheckEnemyMoveLegality(int x1, int y1, int x2, int y2)
    {

        //移動先に自分の駒がいないか？
        
        Debug.Log(x1+" "+y1+" "+x2+" "+x2);
        for (int i = 0; i < 6; i++)
        {
            if (infoRows[8-y2].infoColumns[x2] == PieceManager.Instance.enemyPieceName[i]||infoRows[8-y2].infoColumns[x2] == "River"||infoRows[8-y2].infoColumns[x2] =="Rock"
            ||x1==x2&&y1==y2)
            {
                error = true;
                GameManager.Instance.select=false;
                Debug.Log("error!!");
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if(infoRows[8-y2].infoColumns[x2] == PieceManager.Instance.playerPieceName[i])
            {
                deathObj=GameObject.Find(infoRows[8-y2].infoColumns[x2]+"(Clone)");
            }
        }
        //問題がなければinfoを書き換える
        if (!error)
        {
            infoRows[8-y2].infoColumns[x2]=infoRows[8-y1].infoColumns[x1];
            infoRows[8-y1].infoColumns[x1]="";
            Debug.Log("移動しました！");
        }
    }

    public void PieceMoveAnimation(int x, int y)
    {
        GameObject obj=GameObject.Find(infoRows[8-y].infoColumns[x]+"(Clone)");
        Vector3 pos= tileRows[8-y].tileColumns[x].transform.position; 
        Vector3 objPos= obj.transform.position;

        obj.transform.DOPath(
            new[]
            {
                new Vector3((pos.x+objPos.x)/2,2f,(pos.z+objPos.z)/2),
                new Vector3(pos.x,objPos.y,pos.z)
            },
            1f, PathType.CatmullRom)
            .SetEase(Ease.OutSine)
            .OnComplete(()=>{
                Destroy(deathObj);
            });
        
    }

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
                    pos.y = 1.2f;
                    Instantiate(CommanderPrefab[0], pos, Quaternion.identity);
                }
                else if (infoRows[i].infoColumns[j] == "Sniper1")
                {
                    pos.y = 0.92f;
                    Instantiate(SniperPrefab[0], pos, Quaternion.identity);
                }
                else if (infoRows[i].infoColumns[j] == "MachineGun1")
                {
                    pos.y = 0.96f;
                    Instantiate(MachineGunPrefab[0], pos, Quaternion.identity);
                }
                else if (infoRows[i].infoColumns[j] == "Assault1_A")
                {
                    pos.y = 0.96f;
                    Instantiate(AssaultPrefab[0], pos, Quaternion.identity);
                }
                else if (infoRows[i].infoColumns[j] == "Assault1_B")
                {
                    pos.y = 0.96f;
                    Instantiate(AssaultPrefab[1], pos, Quaternion.identity);
                }
                else if (infoRows[i].infoColumns[j] == "Grenade1")
                {
                    pos.y = 0.96f;
                    Instantiate(GrenadePrefab[0], pos, Quaternion.identity);
                }
                //2P
                else if (infoRows[i].infoColumns[j] == "Commander2")
                {
                    pos.y = 1.2f;
                    Instantiate(CommanderPrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                }
                else if (infoRows[i].infoColumns[j] == "Sniper2")
                {
                    pos.y = 0.92f;
                    Instantiate(SniperPrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                }
                else if (infoRows[i].infoColumns[j] == "MachineGun2")
                {
                    pos.y = 0.96f;
                    Instantiate(MachineGunPrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                }
                else if (infoRows[i].infoColumns[j] == "Assault2_A")
                {
                    pos.y = 0.96f;
                    Instantiate(AssaultPrefab[2], pos, Quaternion.Euler(0, 180f, 0));
                }
                else if (infoRows[i].infoColumns[j] == "Assault2_B")
                {
                    pos.y = 0.96f;
                    Instantiate(AssaultPrefab[3], pos, Quaternion.Euler(0, 180f, 0));
                }
                else if (infoRows[i].infoColumns[j] == "Grenade2")
                {
                    pos.y = 0.96f;
                    Instantiate(GrenadePrefab[1], pos, Quaternion.Euler(0, 180f, 0));
                }
            }
        }
    }

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

        Debug.Log(num[0] + " " + num[1]);

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