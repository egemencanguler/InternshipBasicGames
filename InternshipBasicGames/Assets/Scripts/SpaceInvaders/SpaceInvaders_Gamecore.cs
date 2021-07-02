using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvaders_Gamecore : MonoBehaviour
{
    public static SpaceInvaders_Gamecore instance;
    public List<ObjectBounds> alienBoundList = new List<ObjectBounds>();
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
