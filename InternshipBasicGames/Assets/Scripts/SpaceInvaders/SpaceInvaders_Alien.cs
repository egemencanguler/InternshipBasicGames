using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvaders_Alien : MonoBehaviour
{
    public bool canAttack;
    public int numberAtAlienList;
    float timer;
    public int timeToAttackMax;

    [Header("Bullets")]
    public GameObject bullet;
    public float bulletSpeedMax;
    Vector2 bulletDirection = new Vector2(0, -1);

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(1, timeToAttackMax + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                var newbullet = Instantiate(bullet, new Vector2(transform.position.x , transform.position.y - transform.localScale.y), Quaternion.identity).GetComponent<SpaceInvader_Bullets>();
                newbullet.speed = Random.Range(3,bulletSpeedMax);
                newbullet.movementDirection = bulletDirection;

                timer = Random.Range(1, timeToAttackMax + 1);
            }

        }
    }

    public void Dead()
    {
        var alienListController =  transform.GetComponentInParent<SpaceInvaders_AliensController>();
        if (canAttack)
        {
            if (transform.GetSiblingIndex() - 1 >= 0)
            {
                transform.parent.transform.GetChild( transform.GetSiblingIndex()-1).GetComponent<SpaceInvaders_Alien>().canAttack = true;
            }
        }

        SpaceInvaders_Gamecore.instance.BulletCheckList.Remove(gameObject.GetComponent<ObjectBounds>());
        alienListController.AliensList.Remove(gameObject);
        Destroy(gameObject);


        if (alienListController.AliensList.Count == 0)
        {
            SpaceInvaders_Gamecore.instance.GameOver();

        }
    }
}
