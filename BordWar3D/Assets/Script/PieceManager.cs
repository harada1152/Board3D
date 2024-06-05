using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance;
    public string[] playerPieceName = new string[] { "Assault1_A", "Assault1_B", "Commander1", "Sniper1", "Grenade1", "MachineGun1" };
    public string[] enemyPieceName = new string[] { "Assault2_A", "Assault2_B", "Commander2", "Sniper2", "Grenade2", "MachineGun2" };
    [SerializeField] public GameConst.pieceClass currentPieceClass;

    public List<Vector2Int> moveTypeA = new List<Vector2Int>();
    public List<Vector2Int> moveTypeB = new List<Vector2Int>();
    public List<Vector2Int> AttackTypeSniper1P = new List<Vector2Int> ();
    public List<Vector2Int> AttackTypeSniper2P = new List<Vector2Int> ();

    public List<Vector2Int> AttackTypeMachineGun = new List<Vector2Int>();
    public List<Vector2Int> AttackTypeGrenade = new List<Vector2Int>();                                                           
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

    //選択された駒の種類を判別
    public void SetCurrentPiece(string pieceName)
    {
        if (pieceName == "Assault1_A" || pieceName == "Assault1_B" || pieceName == "Assault2_A" || pieceName == "Assault2_B")
        {
            currentPieceClass = GameConst.pieceClass.Assault;
        }
        else if (pieceName == "Commander1" || pieceName == "Commander2")
        {
            currentPieceClass = GameConst.pieceClass.Commander;
        }
        else if (pieceName == "Sniper1" )
        {
            currentPieceClass = GameConst.pieceClass.Sniper1P;
        }
        else if(pieceName == "Sniper2")
        {
            currentPieceClass = GameConst.pieceClass.Sniper1P;
        }
        else if (pieceName == "Grenade1" || pieceName == "Grenade2")
        {
            currentPieceClass = GameConst.pieceClass.Grenade;
        }
        else if (pieceName == "MachineGun1" || pieceName == "MachineGun2")
        {
            currentPieceClass = GameConst.pieceClass.MachineGun;
        }
    }

    //選択した駒の移動可能な範囲を計算してリストで返す
    public List<Vector2Int> ReturnMoveRange(int x, int y)
    {
        switch (currentPieceClass)
        {
            case GameConst.pieceClass.Assault:
                moveRange = new List<Vector2Int>(CalcMoveRange(moveTypeB, x, y));
                calculatedRange.Clear();
                return moveRange;
            case GameConst.pieceClass.Grenade:
                moveRange = new List<Vector2Int>(CalcMoveRange(moveTypeA, x, y));
                calculatedRange.Clear();
                return moveRange;
            case GameConst.pieceClass.MachineGun:
                moveRange = new List<Vector2Int>(CalcMoveRange(moveTypeA, x, y));
                calculatedRange.Clear();
                return moveRange;
            case GameConst.pieceClass.Sniper1P:
                moveRange = new List<Vector2Int>(CalcMoveRange(moveTypeA, x, y));
                calculatedRange.Clear();
                return moveRange;
            case GameConst.pieceClass.Sniper2P:
                moveRange = new List<Vector2Int>(CalcMoveRange(moveTypeA, x, y));
                calculatedRange.Clear();
                return moveRange;
            case GameConst.pieceClass.Commander:
                moveRange = new List<Vector2Int>(CalcMoveRange(moveTypeA, x, y));
                calculatedRange.Clear();
                return moveRange;
        }
        return null;
    }

    public List<Vector2Int> ReturnAttackRange(int x, int y)
    {
        switch (currentPieceClass)
        {
            case GameConst.pieceClass.Assault:
                return calculatedRange;
            case GameConst.pieceClass.Grenade:
                attackRange = new List<Vector2Int>(CalAttackRange(AttackTypeGrenade, x, y));
                calculatedRange.Clear();
                return attackRange;
            case GameConst.pieceClass.MachineGun:
                attackRange = new List<Vector2Int>(CalAttackRange(AttackTypeMachineGun, x, y));
                calculatedRange.Clear();
                return attackRange;
            case GameConst.pieceClass.Sniper1P:
                attackRange = new List<Vector2Int>(CalAttackRange(AttackTypeSniper1P, x, y));
                calculatedRange.Clear();
                return attackRange;
            case GameConst.pieceClass.Sniper2P:
                attackRange = new List<Vector2Int>(CalAttackRange(AttackTypeSniper2P, x, y));
                calculatedRange.Clear();
                return attackRange;
            case GameConst.pieceClass.Commander:
                return calculatedRange;
        }
        return null;
    }

    //選択された駒の種類、座標を元に移動範囲を計算
    List<Vector2Int> CalcMoveRange(List<Vector2Int> moveType, int x, int y)
    {

        for (int i = 0; i < moveType.Count; i++)
        {
            calculatedRange.Add(moveType[i] + new Vector2Int(x, y));
        }
        return calculatedRange;
    }

    //選択された駒の種類、座標を元に移動範囲を計算
    List<Vector2Int> CalAttackRange(List<Vector2Int> attackType, int x, int y)
    {

        for (int i = 0; i < attackType.Count; i++)
        {
            calculatedRange.Add(attackType[i] + new Vector2Int(x, y));
        }
        return calculatedRange;
    }
}
