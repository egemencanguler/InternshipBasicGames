using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Border;

namespace InternLibrary.Vektors
{
    public class VektorProperties
    {

        /// <summary>
        /// Finds whether two vectors intersect at an infinite point
        /// </summary>
        public static Vector2 LineIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            Vector2 r = p2 - p1;
            Vector2 s = p4 - p3;

            float c = (r.x * s.y) - (r.y * s.x);
            var u = ((p3.x - p1.x) * r.y - (p3.y - p1.y) * r.x) / c;
            var t = ((p3.x - p1.x) * s.y - (p3.y - p1.y) * s.x) / c;
            
            return (p1 + t * r);
        }
        /// <summary>
        /// Finds whether two vectors intersect momentarily
        /// </summary>
        public static Vector2 LineSegmentIntersec(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            Vector2 r = p2 - p1;
            Vector2 s = p4 - p3;

            float c = (r.x * s.y) - (r.y * s.x);
            var u = ((p3.x - p1.x) * r.y - (p3.y - p1.y) * r.x) / c;
            var t = ((p3.x - p1.x) * s.y - (p3.y - p1.y) * s.x) / c;

            if (0 <= u && u <= 1 && 0 <= t && t <= 1)
            {
                return (p1 + t * r);
            }
            else
            {
                return new Vector2(-1110, 0); // To check hit is null or not. Ask..
            }
        }


        /// <summary>
        /// Checks whether the entered vector hits the boundary of the object
        /// </summary>
        public static Borders RayIntersec(Vector2 from, Vector2 to, ObjectBounds[] objectBounds)
        {
            Vector2 movementVector = to + from;
            Vector2 point = new Vector2();
            Vector2 shortestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);
            Borders foundedBorder = new Borders();

            for (int i = 0; i < objectBounds.Length; i++)
            {
                var borders = objectBounds[i].UpdateBorderAndReturn();
                for (int k = 0; k < borders.Length; k++)
                {

                    point = LineSegmentIntersec(from, movementVector, borders[k].p1, borders[k].p2);

                    if (point.magnitude < shortestPoint.magnitude)
                    {
                        borders[k].hitPoint = point;
                        foundedBorder = borders[k];
                        shortestPoint = point;
                    }
                }
            }
            if(foundedBorder.hitPoint.x == -1110) // To check hit is null or not. Ask..
            {
                return null;
            }
            else
            {
                return foundedBorder;
            }
        }

        public static ObjectBounds RayIntersecReturnObj(Vector2 from, Vector2 to, List<ObjectBounds> objectBounds)
        {
            Vector2 movementVector = to + from;
            Vector2 point = new Vector2();
            Vector2 shortestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);
            Borders foundedBorder = new Borders();
            ObjectBounds foundedObj = null;

            for (int i = 0; i < objectBounds.Count; i++)
            {
                var borders = objectBounds[i].UpdateBorderAndReturn();
                for (int k = 0; k < borders.Length; k++)
                {

                    point = LineSegmentIntersec(from, movementVector, borders[k].p1, borders[k].p2);

                    if (point.magnitude < shortestPoint.magnitude)
                    {
                        borders[k].hitPoint = point;
                        foundedBorder = borders[k];
                        foundedObj = objectBounds[i];
                        shortestPoint = point;
                    }
                }
            }
            if (foundedBorder.hitPoint.x == -1110) // To check hit is null or not. Ask..
            {
                return null;
            }
            else
            {
                return foundedObj;
            }
        }





        /// <summary>
        /// Checks whether an object is touching another object in the direction of movement
        /// </summary>
        public static Borders ColliderIntersec(Vector2 from, Vector2 to, ObjectBounds[] objectBounds, ObjectBounds obj) // TO DO: Derinlemesine test edilmesi gerek
        {
            Vector2 point = new Vector2();
            Vector2 shortestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);
            Borders foundedBorder = new Borders();

            Borders[] objBorders = new Borders[4];
            objBorders = obj.UpdateBorderAndReturn();
            
            for(int j = 0; j < objBorders.Length; j++)
            {
                var newFrom = objBorders[j].p1;
                Vector2 movementVector = to + newFrom;

                for (int i = 0; i < objectBounds.Length; i++)
                {
                    if(objectBounds[i] == obj)
                    {
                        continue;
                    }

                    var borders = objectBounds[i].UpdateBorderAndReturn();
                    for (int k = 0; k < borders.Length; k++)
                    {

                        point = LineSegmentIntersec(newFrom, movementVector, borders[k].p1, borders[k].p2);

                        if (point.magnitude < shortestPoint.magnitude)
                        {
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
                point = AlignCenterPointToIntersectionPoint(to,from,point,foundedBorder,obj);

                foundedBorder.hitPoint = point;

                return foundedBorder;
            }


        }

        /// <summary>
        /// Checks whether an object is touching another object in the direction of movement (in list)
        /// </summary>
        public static Borders ColliderIntersec(Vector2 from, Vector2 to, List<ObjectBounds> objectBounds, ObjectBounds obj) // TO DO: Derinlemesine test edilmesi gerek
        {
            Vector2 point = new Vector2();
            Vector2 shortestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);
            Borders foundedBorder = new Borders();

            Borders[] objBorders = new Borders[4];
            objBorders = obj.UpdateBorderAndReturn();

            for (int j = 0; j < objBorders.Length; j++)
            {
                var newFrom = objBorders[j].p1;
                Vector2 movementVector = to + newFrom;

                for (int i = 0; i < objectBounds.Count; i++)
                {
                    if (objectBounds[i] == obj)
                    {
                        continue;
                    }

                    var borders = objectBounds[i].UpdateBorderAndReturn();
                    for (int k = 0; k < borders.Length; k++)
                    {

                        point = LineSegmentIntersec(newFrom, movementVector, borders[k].p1, borders[k].p2);

                        if (point.magnitude < shortestPoint.magnitude)
                        {
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
        /// <summary>
        /// Checks whether an object is touching another object in the direction of movement (in list)
        /// </summary>
        public static ObjectBounds FoundCollidedGameObject(Vector2 from, Vector2 to, List<ObjectBounds> objectBounds, ObjectBounds obj)
        {
            Vector2 point = new Vector2();
            Vector2 shortestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);
            Borders foundedBorder = new Borders();
            ObjectBounds foundedObj = null;

            Borders[] objBorders = new Borders[4];
            objBorders = obj.UpdateBorderAndReturn();

            for (int j = 0; j < objBorders.Length; j++)
            {
                var newFrom = objBorders[j].p1;
                Vector2 movementVector = to + newFrom;

                for (int i = 0; i < objectBounds.Count; i++)
                {
                    if (objectBounds[i] == obj)
                    {
                        continue;
                    }

                    var borders = objectBounds[i].UpdateBorderAndReturn();
                    for (int k = 0; k < borders.Length; k++)
                    {

                        point = LineSegmentIntersec(newFrom, movementVector, borders[k].p1, borders[k].p2);

                        if (point.magnitude < shortestPoint.magnitude && Vector2.Dot(borders[k].normal, to.normalized) < 0)
                        {
                            borders[k].hitPoint = point;
                            foundedBorder = borders[k];
                            foundedObj = objectBounds[i];
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
                return foundedObj;
            }

        }

        /// <summary>
        /// Rearranges the intersection point to the size of the object
        /// </summary>
        public static Vector2 AlignCenterPointToIntersectionPoint(Vector2 to, Vector2 from, Vector2 interPoint, Borders foundedBorder, ObjectBounds obj)
        {
            Vector2 movementVector = to + from;
            interPoint = LineIntersect(from, movementVector, foundedBorder.p1, foundedBorder.p2);
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


        public static bool Check2ObjectBoundsCollision(ObjectBounds obj, ObjectBounds other)
        {
            int otherBorderNumber;
            Vector2 point;
            obj.UpdateBorders();
            other.UpdateBorders();

            for (int borderNumber = 0; borderNumber < 4; borderNumber++)
            {
                otherBorderNumber = borderNumber - 1;
                if(otherBorderNumber == -1)
                {
                    otherBorderNumber = 3;
                }

                point = LineSegmentIntersec(obj.borders[borderNumber].p2, obj.borders[borderNumber].p1, other.borders[otherBorderNumber].p2, other.borders[otherBorderNumber].p1);
                if(point.x != -1110)
                {
                    return true;
                }

                otherBorderNumber = borderNumber + 1;
                if (otherBorderNumber == 4)
                {
                    otherBorderNumber = 0;
                }
                point = LineSegmentIntersec(obj.borders[borderNumber].p2, obj.borders[borderNumber].p1, other.borders[otherBorderNumber].p2, other.borders[otherBorderNumber].p1);
                if (point.x != -1110)
                {
                    return true;
                }
            }
            return false;
        }


    }

    
}

