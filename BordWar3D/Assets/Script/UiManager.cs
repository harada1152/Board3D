using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public GameObject pieceNameObj;
    [SerializeField] private TextMeshProUGUI pieceName;
    public GameObject selectFramePrefab;
    private GameObject selectFrame;
    [SerializeField] private GameObject[] resultUi;
    Vector3 point = new Vector3Int();
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 50.0f))
        {
            point = hit.point;
            int x = (int)Mathf.RoundToInt(point.x);
            int y = (int)Mathf.RoundToInt(point.z);
            if (x < 0) { x = 0; }
            if (y < 0) { y = 0; }

            if (hit.collider.gameObject.CompareTag("Tile"))
            {
                HighLightSelectTile(x, y);
            }
            else { selectFrame.SetActive(false); }

            if (hit.collider.gameObject.CompareTag("Piece"))
            {
                SetPieceNamePos(x, y);
                PieceManager.Instance.SetCurrentPiece(hit.collider.gameObject.name);
                SetDisplayPieceName();
            }
            else{pieceName.SetText("");}
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

    void DisplayGameOverUI()
    {

    }
}
