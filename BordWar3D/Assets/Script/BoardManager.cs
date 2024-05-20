using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] List<Column> tileColumns = new List<Column>();
    [SerializeField] List<BoardInfo> infoColumns = new List<BoardInfo>();
    [SerializeField] GameObject bridgePrefab;
    [SerializeField] GameObject RockPrefab;
    [SerializeField] GameObject[] CommanderPrefab = new GameObject[2];
    [SerializeField] GameObject[] SniperPrefab = new GameObject[2];
    [SerializeField] GameObject[] MachineGunPrefab = new GameObject[2];
    [SerializeField] GameObject[] AssaultPrefab = new GameObject[2];
    [SerializeField] GameObject[] GrenadePrefab = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        RandomBridge();

        Placement();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Placement()
    {
        Vector3 pos = new Vector3();

        for (int i = 0; i < infoColumns.Count; i++)
        {
            for (int j = 0; j < infoColumns[i].infoRows.Count; j++)
            {
                pos = tileColumns[i].tileRows[j].transform.position;

                //引数内文字列は略称、1,2で1P,2Pカラーを差別化
                //1P
                //Rock=Roc
                if (infoColumns[i].infoRows[j] == "Roc")
                {
                    pos.y = 0.86f;
                    Instantiate(RockPrefab, pos, Quaternion.identity);
                }
                //Commander=Com
                else if (infoColumns[i].infoRows[j] == "Com1")
                {
                    pos.y = 1.2f;
                    Instantiate(CommanderPrefab[0], pos, Quaternion.identity);
                }
                //Sniper=Sni
                else if (infoColumns[i].infoRows[j] == "Sni1")
                {
                    pos.y = 0.92f;
                    Instantiate(SniperPrefab[0], pos, Quaternion.identity);
                }
                //MachineGun=Mac
                else if (infoColumns[i].infoRows[j] == "Mac1")
                {
                    pos.y = 0.96f;
                    Instantiate(MachineGunPrefab[0], pos, Quaternion.identity);
                }
                //Assault=Ass
                else if (infoColumns[i].infoRows[j] == "Ass1")
                {
                    pos.y = 0.96f;
                    Instantiate(AssaultPrefab[0], pos, Quaternion.identity);
                }
                else if (infoColumns[i].infoRows[j] == "Gre1")
                {
                    pos.y = 0.96f;
                    Instantiate(GrenadePrefab[0], pos, Quaternion.identity);
                }
                //2P
                else if (infoColumns[i].infoRows[j] == "Com2")
                {
                    pos.y = 1.2f;
                    Instantiate(CommanderPrefab[1], pos, Quaternion.identity);
                }
                else if (infoColumns[i].infoRows[j] == "Sni2")
                {
                    pos.y = 0.92f;
                    Instantiate(SniperPrefab[1], pos, Quaternion.identity);
                }
                else if (infoColumns[i].infoRows[j] == "Mac2")
                {
                    pos.y = 0.96f;
                    Instantiate(MachineGunPrefab[1], pos, Quaternion.identity);
                }
                else if (infoColumns[i].infoRows[j] == "Ass2")
                {
                    pos.y = 0.96f;
                    Instantiate(AssaultPrefab[1], pos, Quaternion.identity);
                }
                else if (infoColumns[i].infoRows[j] == "Gre2")
                {
                    pos.y = 0.96f;
                    Instantiate(GrenadePrefab[1], pos, Quaternion.identity);
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
            pos[i] = tileColumns[4].tileRows[num[i]].transform.position;
            pos[i].y = 0.6f;
            Instantiate(bridgePrefab, pos[i], Quaternion.identity);
        }
    }
}



[System.Serializable]
public class Column
{
    public List<GameObject> tileRows;
}

[System.Serializable]
public class BoardInfo
{
    public List<string> infoRows;
}