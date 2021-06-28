using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameCore : MonoBehaviour
{
    public static GameCore instance;
    
    [SerializeField] private GameObject player1, player2;
    [SerializeField] private GameObject ball;
    private int scoreLeft=0,scoreRight=0;
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Get_Ball()
    {
        return ball;
    }

    public GameObject Get_Player1()
    {
        return player1;
    }

    public GameObject Get_Player2()
    {
        return player2;
    }

    public void GoScore(int direction)
    {
        ball.GetComponent<Ball>().ResetTheBall();
        if (direction == 1)
        {
            scoreLeft++;
        }
        else if(direction == -1)
        {
            scoreRight++;
        }
        text.text = "" + scoreLeft + " - " + scoreRight;
        
    }
}
