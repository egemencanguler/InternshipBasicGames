using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;

public class MyPhysicsSystem : MonoBehaviour
{
    public static MyPhysicsSystem instance;
    //public ObjectBounds[] objectsBounds = new ObjectBounds[2];
    //public MyObjectPhysic[] myObjectPhysic = new MyObjectPhysic[2];
   // public MyObjectPhysic[] myDynamicObjects = new MyObjectPhysic[2];

    public List<ObjectBounds> objectsBounds = new List<ObjectBounds>();
    public List<MyObjectPhysic> myObjectPhysic =new List<MyObjectPhysic>();
    public List<MyObjectPhysic> myDynamicObjects =new List<MyObjectPhysic>();

    private void Awake()
    {
        instance = this;
        GeneratePhysicsList();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AreTheyReadyToMove())
        {
            DoPhysicsWithOrder();
            ResetMovementReadyToMove();
        }
    }

    public void GeneratePhysicsList()
    {
        objectsBounds = new List<ObjectBounds>( Object.FindObjectsOfType<ObjectBounds>());
        myObjectPhysic = new List<MyObjectPhysic>(Object.FindObjectsOfType<MyObjectPhysic>());
        for(int i = 0; i < myObjectPhysic.Count; i++)
        {
            if (myObjectPhysic[i].bodytype == MyObjectPhysic.Bodytype.Dynamic)
            {
                myDynamicObjects.Add(myObjectPhysic[i]);
            }
        }
    }

    public void DoPhysicsWithOrder()
    {
        for (int m = 0; m < myObjectPhysic.Count; m++)
        {
            if (myObjectPhysic[m].bodytype == MyObjectPhysic.Bodytype.Dynamic)
            {
                myObjectPhysic[m].objBounds.hitBorder = ColliderIntersec(myObjectPhysic[m].transform.position, myObjectPhysic[m].movement, objectsBounds, myObjectPhysic[m].objBounds);
            }

            myObjectPhysic[m].DoCollisionEvent();
            myObjectPhysic[m].objBounds.hitMeBorder.Clear();

            if (myObjectPhysic[m].bodytype == MyObjectPhysic.Bodytype.Dynamic)
            {
                myObjectPhysic[m].GoNewPosition();
            }
        }
    }

    public void DoPhysicsWithSameTime()
    {
        CheckCollidingSystem();
        DoCollisionEvents();
        ClearHitMePoints();
        MoveTheObjects();
    }

    public void ClearHitMePoints()
    {
        for (int i = 0; i < objectsBounds.Count; i++)
        {
            objectsBounds[i].hitMeBorder.Clear();
        }
    }

    public void DoCollisionEvents()
    {
        for (int i = 0; i < myObjectPhysic.Count; i++)
        {
            myObjectPhysic[i].DoCollisionEvent();
        }
    }

    public void CheckCollidingSystem()
    {
        for (int m = 0; m < myDynamicObjects.Count; m++)
        {
            myDynamicObjects[m].objBounds.hitBorder = ColliderIntersec(myDynamicObjects[m].transform.position, myDynamicObjects[m].movement, objectsBounds, myDynamicObjects[m].objBounds);

        }
    }

    public void MoveTheObjects()
    {
        for (int i = 0; i < myDynamicObjects.Count; i++)
        {
            myDynamicObjects[i].GoNewPosition();
        }
    }
    public void ResetMovementReadyToMove()
    {
        for (int i = 0; i < myDynamicObjects.Count; i++)
        {
            myDynamicObjects[i].readyToMove = false;
        }
    }

    public bool AreTheyReadyToMove()
    {
        for (int i = 0; i < myDynamicObjects.Count; i++)
        {
            if (!myDynamicObjects[i].readyToMove)
            {
                return false;
            }
        }
        return true;
    }



    public Borders ColliderIntersec(Vector2 from, Vector2 to, List<ObjectBounds> objectBounds, ObjectBounds obj) // TO DO: Derinlemesine test edilmesi gerek
    {
        Vector2 point = new Vector2();
        Vector2 shortestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Borders foundedBorder = new Borders();


        Borders[] objBorders = new Borders[4];
        objBorders = obj.UpdateBorderAndReturn();

        for (int j = 0; j < objBorders.Length; j++)// searching selected object corners
        {
            var newFrom = objBorders[j].p1;
            Vector2 movementVector = to + newFrom;

            for (int i = 0; i < objectBounds.Count; i++) // searching other object s
            {
                if (objectBounds[i] == obj)
                {
                    continue;
                }

                var borders = objectBounds[i].UpdateBorderAndReturn();
                for (int k = 0; k < borders.Length; k++) // searching other objects corners
                {

                    point = VektorProperties.LineSegmentIntersec(newFrom, movementVector, borders[k].p1, borders[k].p2);

                    if (point.x != -1110 && point.magnitude < shortestPoint.magnitude)
                    {
                        objectBounds[i].hitMeBorder.Add(objBorders[j]); //sonradan clearlamayı unutma
                        borders[k].hitPoint = point;
                        foundedBorder = borders[k];
                        shortestPoint = point;
                    }
                }
            }
        }

        if (foundedBorder.hitPoint.x == -1110) // To check hit is null or not. Ask..
        {
            return null;
        }
        else
        {
            point = AlignCenterPointToIntersectionPoint(to, from, point, foundedBorder, obj);

            foundedBorder.hitPoint = point;

            return foundedBorder;
        }


    }

    public Vector2 AlignCenterPointToIntersectionPoint(Vector2 to, Vector2 from, Vector2 interPoint, Borders foundedBorder, ObjectBounds obj)
    {
        Vector2 movementVector = to + from;
        interPoint = VektorProperties.LineIntersect(from, movementVector, foundedBorder.p1, foundedBorder.p2);
        float offsetY;
        float offsetX;
        if (to.y < 0)
        {
            offsetY = Mathf.Abs(foundedBorder.normal.x) * (from.y - interPoint.y);
        }
        else
        {
            offsetY = Mathf.Abs(foundedBorder.normal.x) * (from.y - interPoint.y);
        }

        if (to.x < 0)
        {
            offsetX = Mathf.Abs(foundedBorder.normal.y) * (from.x - interPoint.x);
        }
        else
        {
            offsetX = Mathf.Abs(foundedBorder.normal.y) * (from.x - interPoint.x);
        }

        return new Vector2(interPoint.x + offsetX + (foundedBorder.normal.x * (obj.rect.width / 2)), interPoint.y + offsetY + (foundedBorder.normal.y * (obj.rect.height / 2)));
    }
}
