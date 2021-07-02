using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceRace_Gamecore : MonoBehaviour
{
    public static SpaceRace_Gamecore instance;
    public GameObject player1;
    public GameObject player2;


    private void Awake()
    {
        instance = this;
    }


}
