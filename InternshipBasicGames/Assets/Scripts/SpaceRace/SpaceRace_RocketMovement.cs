using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;
using TMPro;
public class SpaceRace_RocketMovement : MonoBehaviour
{
    public float speed;
    public Rect allowedArea;
    Vector2 movement;
    public Vector2 startPoint;
    public Vector2 newPos;


    public int score = 0;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        startPoint = transform.position;
    }
    void Start()
    {

    }

    private void Update()
    {
        float direction = Input.GetAxisRaw("Vertical");
        Vector2 newPos = transform.position;
        newPos.y += speed * direction * Time.deltaTime;

        if (newPos.y - transform.localScale.y/2 < allowedArea.yMin)
        {
            newPos = transform.position;
        }
        else if (newPos.y + transform.localScale.y/2 > allowedArea.yMax)
        {
            GoScore();
            newPos = startPoint;
        }

        /*if (!allowedArea.Contains(newPos))
        {
            
        }*/
        transform.position = newPos;
    }

    public void ResetTheRocket()
    {
        transform.position = startPoint;
    }

    public void GoScore()
    {
        score++;
        scoreText.text = "" + score;
    }

}
