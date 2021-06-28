using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 1;
    [SerializeField] Rect allowedArea;
    private Bounds bounds;
    private void Awake()
    {
        bounds = GetComponent<Renderer>().bounds;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Movement();
        bounds = GetComponent<Renderer>().bounds;
    }

    void Movement()
    {
        float direction = Input.GetAxisRaw("Vertical");
        Vector2 newPos = transform.position;
        newPos.y += movementSpeed * direction * Time.deltaTime;
        if (!allowedArea.Contains(newPos))
        {
            newPos = transform.position;
        }
        transform.position = newPos;
    }
    public Bounds Get_bounds()
    {
        return bounds;
    }
}
