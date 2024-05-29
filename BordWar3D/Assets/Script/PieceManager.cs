using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance;

    public string[] playerPieceName = new string[]{"Assault1_A","Assault1_B","Commander1","Sniper1","Grenade1","MachineGun1"};
    public string[] enemyPieceName = new string[]{"Assault2_A","Assault2_B","Commander2","Sniper2","Grenade2","MachineGun2"};

    public enum pieceClass
    {
        Assault,
        Grenade,
        MachineGun,
        Sniper,
        Commander
    }
    void Awake()
    {
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
}
