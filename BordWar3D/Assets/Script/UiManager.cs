using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image=UnityEngine.UI.Image;
using TMPro;
using UnityEditor;
using DG.Tweening;
using NUnit.Framework.Constraints;
using Microsoft.Unity.VisualStudio.Editor;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    //テキスト用変数
    [SerializeField] private TextMeshProUGUI pieceName;
    [SerializeField] private TextMeshProUGUI bigPopupText;
    [SerializeField] private TextMeshProUGUI smallPopupText;

    //選択されたマスを光らせるのに必要な変数
    [SerializeField] private GameObject selectFramePrefab;
    private GameObject selectFrame;

    //Ui関係オブジェクト格納用
    [SerializeField] private GameObject pieceNameObj;
    [SerializeField] private GameObject bigMessageBackGround;
    [SerializeField] private GameObject smallMessageObj;
    [SerializeField] private GameObject bigMessageObj;
    [SerializeField] private GameObject[] resultUi;
    [SerializeField] private Image bgColor;

    //カーソル座標格納用
    Vector3 point = new Vector3Int();

    //ポップアップメッセージの座標格納用
    Vector3 smallPopupPos = new Vector3();

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        selectFrame = Instantiate(selectFramePrefab, point, Quaternion.identity, gameObject.transform);
        selectFrame.SetActive(false);
        pieceName.SetText("");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームが終了していたらリザルトUIを表示
        if (GameManager.Instance.isGameEnd)
        {
            DisplayResultUI();
        }
        //ゲーム進行中なら選択中のマスを強調表示
        else
        {
            CursorHighLight();
        }
    }

    private void CursorHighLight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 50.0f))
        {
            //rayを飛ばして座標を取得し、int型へ変換
            point = hit.point;
            int x = (int)Mathf.RoundToInt(point.x);
            int y = (int)Mathf.RoundToInt(point.z);
            if (x < 0) { x = 0; }
            if (y < 0) { y = 0; }

            //選択中のマスを光らせる
            if (hit.collider.gameObject.CompareTag("Tile"))
            {
                HighLightSelectTile(x, y);
            }
            else { selectFrame.SetActive(false); }

            //駒の上に名前を表示
            if (hit.collider.gameObject.CompareTag("Piece"))
            {
                SetPieceNamePos(x, y);
                PieceManager.Instance.SetCurrentPiece(hit.collider.gameObject.name);
                SetDisplayPieceName();
            }
            else { pieceName.SetText(""); }
        }

    }

    //選択中の駒の場所へテキストを移動
    public void SetPieceNamePos(int x, int y)
    {
        pieceNameObj.transform.position = new Vector3(x, 0, y);
    }

    //選択中の駒に応じてテキストを変更
    public void SetDisplayPieceName()
    {
        switch (PieceManager.Instance.currentPieceClass)
        {
            case GameConst.pieceClass.Assault:
                pieceName.SetText("突撃兵");
                break;
            case GameConst.pieceClass.Grenade:
                pieceName.SetText("工兵");
                break;
            case GameConst.pieceClass.MachineGun:
                pieceName.SetText("機関銃兵");
                break;
            case GameConst.pieceClass.Sniper1P:
                pieceName.SetText("狙撃兵");
                break;
            case GameConst.pieceClass.Sniper2P:
                pieceName.SetText("狙撃兵");
                break;
            case GameConst.pieceClass.Commander:
                pieceName.SetText("司令官");
                break;
            default:

                break;
        }
    }

    //選択中のマスを強調表示
    void HighLightSelectTile(int x, int y)
    {
        selectFrame.SetActive(true);
        if (y == 4 && BoardManager.Instance.infoRows[y].infoColumns[x] != "River")
        {
            selectFrame.transform.position = new Vector3(x, 0.35f, y);
        }
        else
        {
            selectFrame.transform.position = new Vector3(x, 0.05f, y);
        }
    }

    //ポップアップメッセージの表示
    public void PopupMessage(GameConst.MessageType messageType)
    {
        switch (messageType)
        {
            case GameConst.MessageType.TurnStart:
                if (GameManager.Instance.currentState == GameConst.GameState.PLAYERTURN)
                {
                    bigPopupText.color = Color.blue;
                    bgColor.color  = new Color32(0, 220, 255, 255);
                    bigPopupText.SetText("1Pのターン");
                }
                else
                {
                    bigPopupText.color = Color.red;
                    bgColor.color  = new Color32(255, 100, 90, 255);
                    bigPopupText.SetText("2Pのターン");
                }
                BigPopupAnimation();
                break;
            case GameConst.MessageType.MoveError:
                smallPopupText.color = Color.red;
                smallPopupText.SetText("そのマスには移動できません");
                SmallPopupAnimation();
                break;
            case GameConst.MessageType.AttackError:
                smallPopupText.color = Color.red;
                smallPopupText.SetText("攻撃できません");
                SmallPopupAnimation();
                break;
            case GameConst.MessageType.SelectError:
                smallPopupText.color = Color.red;
                smallPopupText.SetText("その駒は選択できません");
                SmallPopupAnimation();
                break;
            case GameConst.MessageType.Kill:
                if (GameManager.Instance.currentState == GameConst.GameState.PLAYERTURN)
                { smallPopupText.color = Color.blue; }
                else
                { smallPopupText.color = Color.blue; }
                smallPopupText.SetText("撃破！");
                SmallPopupAnimation();
                break;
        }
    }

    private void BigPopupAnimation()
    {
        bigMessageBackGround.SetActive(true);
        bigMessageObj.SetActive(true);
        DOVirtual.DelayedCall(2.5f, () => bigMessageBackGround.SetActive(false));
        DOVirtual.DelayedCall(2.5f, () => bigMessageObj.SetActive(false));
    }

    private void SmallPopupAnimation()
    {
        smallMessageObj.SetActive(true);
        DOVirtual.DelayedCall(1.5f, () => smallMessageObj.SetActive(false));
    }

    //リザルト画面の表示
    private void DisplayResultUI()
    {
        selectFrame.SetActive(false);
        pieceName.SetText("");

        switch (GameManager.Instance.currentState)
        {
            case GameConst.GameState.PLAYERTURN:
                resultUi[0].SetActive(true);
                break;
            case GameConst.GameState.ENEMYTURN:
                resultUi[1].SetActive(true);
                break;
        }
    }
}
