using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvaders_Gamecore : MonoBehaviour
{
    public static SpaceInvaders_Gamecore instance;
    public List<ObjectBounds> BulletCheckList = new List<ObjectBounds>();
    public GameObject player;
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

    public void GameOver()
    {
        Debug.Log("Game is Over");
    }

}
