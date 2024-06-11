using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    public enum MovePhase
    {
        WaitingForPieceSelection, // コマを選択待ち状態
        PieceSelected,            // 選択後の状態
        HighlightMovableTiles,    // 移動できるマスを光らせる
        SelectingAnotherTile,     // 他のマスを選択した時
        PieceMoveAnimation,       // マス移動アニメーション
        Ended                    // 終了
    }

    public enum PlayerType
    {
        None,
        PLAYER,
        ENEMY,
    }

    public enum GameState
    {
        PLAYERTURN,
        ENEMYTURN,
    }

    public enum ActionType
    {
        Attack,
        Move,
    }

    public enum TurnPhase
    {
        Start,
        Action,
        End,
        Next,
    }

    public enum pieceClass
    {
        Assault,
        Grenade,
        MachineGun,
        Sniper1P,
        Sniper2P,
        Commander
    }
}
