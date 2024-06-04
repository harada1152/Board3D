using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public GameObject selectFramePrefab;

    private GameObject selectFrame;

    Vector3 point = new Vector3Int();
    // Start is called before the first frame update
    void Start()
    {
        selectFrame=Instantiate(selectFramePrefab,point,Quaternion.identity, gameObject.transform);
        selectFrame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 50.0f) && hit.collider.gameObject.CompareTag("masu"))
        {
            point=hit.point;
            int x = (int)Mathf.RoundToInt(point.x);
            int y = (int)Mathf.RoundToInt(point.z);

            if(x<0){x=0;}
            if(y<0){y=0;}

            selectFrame.SetActive(true);
            if(y==4&&BoardManager.Instance.infoRows[y].infoColumns[x]!="River")
            {
                selectFrame.transform.position=new Vector3(x,0.35f,y);   
            }
            else
            {
                selectFrame.transform.position = new Vector3(x,0.05f,y);
            }
            
        }
        else
        {
            selectFrame.SetActive(false) ;
        }
    }
}
