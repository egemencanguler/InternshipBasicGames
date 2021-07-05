using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvaders_Gamecore : MonoBehaviour
{
    public static SpaceInvaders_Gamecore instance;
    public List<ObjectBounds> BulletCheckList = new List<ObjectBounds>();
    public GameObject player;
    public GameObject WonPanel;
    public GameObject LosePanel;
    private void Awake()
    {
        instance = this;
        BulletCheckList.Add(player.GetComponent<ObjectBounds>());
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(int status)
    {
        if(status == 1)
        {
            Time.timeScale = 0;
            WonPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            LosePanel.SetActive(true);
        }
    }

}
