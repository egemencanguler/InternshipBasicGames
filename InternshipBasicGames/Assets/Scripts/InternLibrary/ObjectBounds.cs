using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Border;
public class ObjectBounds : MonoBehaviour
{
    // TODO bunlar ney borders public ve serialized borders public ve serialized ama asagida hesaplaniyor
    // TODO neden rect kullaniyosun Vector2 size kullan isin cok daha kolaylasir gerekirse Vecto2 offsette ekleyebilirsin
    
    // TODO yapmaya calistigin su yanlis anlamadiysam
    /*
        class Line
            public Vector2 p1,p2
     
        class CustomCollider
            [Nonserialized] public Line[] lines; // normalde serialized yapip editor scriptiyle hesaplatirdim, local pos
            public Vector2 size;
            
            void Awake()
                // linelari sizedan hesapla , localde hesapla
            
            
            public bool FindIntersection(CustomCollider other)
                for line1 in other.lines
                    for line2 in lines
                        // transformPoint costundan kurtulmak istersen CollisionSystem yazarsin orda bi kere hesaplayip cashlersin her frame
                        Line.CheckIntersection(other.transform.TransformPoint(line1.p1),other.transform.TransformPoint(line2.p2),
                                             transform.TransformPoint(line2.p1), transform.TransformPoint(line2.p2)) 
            
     
     */
    
    
    
    
    public ObjectTagList.ObjectTags objectTag;
    public Rect rect;
    public Vector2 scaleBounds = new Vector2(1, 1);
    public Vector2 addBounds = new Vector2(0, 0);
    public Borders[] borders = new Borders[4];
    public Vector2[] corners = new Vector2[4];


    // TODO summarye donkyonun adini yazmanin bir anlami yok
    /// <summary>
    /// Updates the boundaries of the object and return
    /// </summary>
    public Borders[] UpdateBorderAndReturn()
    {
        ConfigureBorder();
        Vector2[] corners = new Vector2[4]; // TODO corners arrayi zaten var neden tekrar allocate ediyosun


        corners[0] = new Vector2(rect.center.x - rect.width / 2, rect.center.y - rect.height / 2);
        corners[1] = new Vector2(rect.center.x - rect.width / 2, rect.center.y + rect.height / 2);
        corners[2] = new Vector2(rect.center.x + rect.width / 2, rect.center.y + rect.height / 2);
        corners[3] = new Vector2(rect.center.x + rect.width / 2, rect.center.y - rect.height / 2);

        borders[0] = new Borders(corners[0], corners[1], Vector2.left, objectTag);

        borders[1] = new Borders(corners[1], corners[2], Vector2.up, objectTag);

        borders[2] = new Borders(corners[2], corners[3], Vector2.right, objectTag);

        borders[3] = new Borders(corners[3], corners[0], Vector2.down, objectTag);


        return borders;
    }

    /// <summary>
    /// Updates the boundaries of the object
    /// </summary>
    public void UpdateBorders()
    {
        ConfigureBorder();
        


        corners[0] = new Vector2(rect.center.x - rect.width / 2, rect.center.y - rect.height / 2);
        corners[1] = new Vector2(rect.center.x - rect.width / 2, rect.center.y + rect.height / 2);
        corners[2] = new Vector2(rect.center.x + rect.width / 2, rect.center.y + rect.height / 2);
        corners[3] = new Vector2(rect.center.x + rect.width / 2, rect.center.y - rect.height / 2);

        borders[0] = new Borders(corners[0], corners[1], Vector2.left, objectTag);

        borders[1] = new Borders(corners[1], corners[2], Vector2.up, objectTag);

        borders[2] = new Borders(corners[2], corners[3], Vector2.right, objectTag);

        borders[3] = new Borders(corners[3], corners[0], Vector2.down, objectTag);
    }


    /// <summary>
    /// Sets the border height and width of the object
    /// </summary>
    public void ConfigureBorder()
    {
        rect.center = transform.position;
        rect.width = transform.localScale.x * scaleBounds.x + addBounds.x;
        rect.height = transform.localScale.y * scaleBounds.y + addBounds.y;
    }

    public void ConfigureBorder(Vector2 center, Vector2 scale, Vector2 add)
    {
        transform.position = center ;
        scaleBounds = scale;
        addBounds = add;
        ConfigureBorder();
    } //???

    private void OnDrawGizmos()
    {
        ConfigureBorder();
        Gizmos.DrawWireCube(rect.center, rect.size);

        var p0 = new Vector2(rect.center.x - rect.width / 2, rect.center.y - rect.height / 2);
        var p1 = new Vector2(rect.center.x - rect.width / 2, rect.center.y + rect.height / 2);
        var p2 = new Vector2(rect.center.x + rect.width / 2, rect.center.y + rect.height / 2);
        var p3 = new Vector2(rect.center.x + rect.width / 2, rect.center.y - rect.height / 2);

        Debug.DrawLine(p0, p1, Color.green);
        Debug.DrawLine(p1, p2, Color.green);
        Debug.DrawLine(p2, p3, Color.green);
        Debug.DrawLine(p3, p0, Color.green);
    }
}
