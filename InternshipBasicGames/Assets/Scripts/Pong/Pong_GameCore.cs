using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pong_GameCore : MonoBehaviour
{
    public static Pong_GameCore instance;
    public ObjectBounds[] objectsBounds = new ObjectBounds[2];
    private int scoreLeft=0,scoreRight=0;
    public TextMeshProUGUI text;
    

    private void Awake()
    {
        instance = this;
    }

    public void GoScore(string direction)
    {
        if (direction == "RightScore")
        {
            scoreLeft++;
        }
        else if(direction == "LeftScore")
        {
            scoreRight++;
        }
        text.text = "" + scoreLeft + " - " + scoreRight;
        
    }
}
