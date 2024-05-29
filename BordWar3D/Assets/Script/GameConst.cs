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

    public enum TurnPhase
    {
        Start,
        Action,
        End,
        Next,
    }
}
