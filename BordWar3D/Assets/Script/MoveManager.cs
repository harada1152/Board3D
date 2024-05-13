using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveManager : MonoBehaviour
{
    GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数
    GameObject komaObject;
    public TextMeshProUGUI selectObj;
    private bool select;

    Vector3 pos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;

                if(clickedGameObject.CompareTag("masu")&&select)
                {
                    pos=clickedGameObject.transform.position;
                    komaObject.transform.position=new Vector3(pos.x, pos.y + 0.9f, pos.z);

                    Debug.Log(komaObject.name);//ゲームオブジェクトの名前を出力

                    Initialize();
                }

                if(clickedGameObject.CompareTag("koma"))
                {
                    komaObject=clickedGameObject;
                    select=true;
                    Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                }
                else
                {
                    Initialize();
                }

                if(!clickedGameObject.CompareTag("koma"))
                {
                    Initialize();

                    Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                }

                //clickedGameObject = hit.collider.gameObject;
                //Destroy(clickedGameObject);//ゲームオブジェクトを破壊
            }
        }

        if(clickedGameObject!=null)
        {
            selectObj.text = "Select:"+clickedGameObject.name;
        }
        
    }

    private void Initialize()
    {
        komaObject=null;
        clickedGameObject=null;
        select=false;
    }
}
