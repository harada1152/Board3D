using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    public static RuleManager Instance;

    private GameConst.GameState gameState;
    void Awake(){
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPlayerSelect(int x, int y)
    {
        //BoardManager.Instance.infoColumns[].infoRows[];
    }

    public void CheckEnemySelect(int x, int y)
    {

    }

    public void CheckPlayerMoveLegality(int x1,int y1,int x2,int y2)
    {

    }

    public void CheckEnemyMoveLegality(int x1,int y1,int x2,int y2)
    {

    }
}
