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
    [SerializeField] List<BoardInfo> infoColumns=new List<BoardInfo>();
    [SerializeField] GameObject bridgePrefab;
    // Start is called before the first frame update
    void Start()
    {
        RandomBridge();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Placement()
    {
        
    }

    void RandomBridge()
    {
        int[] num=new int[2];
        Vector3[] pos=new Vector3[2];
        
        //ランダムな値の取得
        for(int i = 0;i<2;i++)
        {
            num[i]=Random.Range(0,6);
        }
        //重複していた時の処理
        while(num[0]==num[1])
        {
            num[1]=Random.Range(0,6);
        }

        Debug.Log(num[0]+" "+num[1]);

        //橋を生成
        for(int i = 0;i<2;i++)
        {
            if(num[i]>=3)
            {
                num[i]+=2;
            }
            pos[i]=tileColumns[4].tileRows[num[i]].transform.position;
            pos[i].y=0.6f;
            Instantiate (bridgePrefab,pos[i], Quaternion.identity);
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