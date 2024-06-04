using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance;

    public string[] playerPieceName = new string[] { "Assault1_A", "Assault1_B", "Commander1", "Sniper1", "Grenade1", "MachineGun1" };
    public string[] enemyPieceName = new string[] { "Assault2_A", "Assault2_B", "Commander2", "Sniper2", "Grenade2", "MachineGun2" };

    public List<Vector2Int> moveTypeA = new List<Vector2Int> {new Vector2Int(1,0),new Vector2Int(0,1),
                                                            new Vector2Int(-1,0),new Vector2Int(0,-1)};

    public List<Vector2Int> AttackTypeSniper = new List<Vector2Int> {new Vector2Int(0,1),new Vector2Int(0,2),new Vector2Int(0,3),
                                                                new Vector2Int(0,4)};
    public List<Vector2Int> moveRange = new List<Vector2Int>();
    public List<Vector2Int> attackRange = new List<Vector2Int>();

    public List<Vector2Int> calculatedRange = new List<Vector2Int>();

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        attackRange = new List<Vector2Int>(calculatedRange);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Vector2Int> ReturnMoveRange(GameConst.pieceClass currentPiece, int x, int y)
    {

        switch (currentPiece)
        {
            case GameConst.pieceClass.Assault:
                return moveRange;
            case GameConst.pieceClass.Grenade:

                return moveRange;
            case GameConst.pieceClass.MachineGun:

                return moveRange;
            case GameConst.pieceClass.Sniper:

                return moveRange;
            case GameConst.pieceClass.Commander:

                return moveRange;
        }
        return null;
    }

    void CalcRange(List<Vector2Int> range, int x, int y)
    {

        for (int i = 0; i < range.Count; i++)
        {
            moveRange.Add(range[i] + new Vector2Int(x, y));
        }
    }
}
