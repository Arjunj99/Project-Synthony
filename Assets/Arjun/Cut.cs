using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ATTACH CUT SCRIPT TO A GAMEOBJECT TO MAKE THEM CUTTABLE
public class Cut : MonoBehaviour {
    //==========================================================================
    // VARIABLES
    Vector2[] vertices;
    public Vector3 mouseEnter, mouseExit, particleScale; // Where mouseEnter Enters and Exits
    public GameObject particle; // Add Prefab Here
    public Material inMaterial;
    public List<Vector2> polygonVertices = new List<Vector2>();
    PolygonCollider2D polygonCollider2D;
    //==========================================================================

    //==========================================================================
    // FUNCTIONS
    private void Start() {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    public void updateCorners()
    {
        int pointCount = polygonCollider2D.points.Length;
        for (int i = 0, j = pointCount - 1; i < pointCount; j = i++)
        {
            polygonVertices.Add(polygonCollider2D.gameObject.transform.TransformPoint(polygonCollider2D.points[i]));
        }
    }

    public void cutGameObject() {
        //CHANGE THE VERTICES IN THIS VECTOR
        //2 CASES CORNER CUT, MIDDLE CUT, TEST THIS BY DOING SOMETHING
        Vector2[] vertices2D = polygonVertices.ToArray();
        Vector2 mouseMove = mouseExit - mouseEnter;
        Vector2 normalVec = Vector2.Perpendicular(mouseMove);

        List<Vector2> side_a = new List<Vector2>();
        List<Vector2> side_b = new List<Vector2>();

        foreach(Vector2 v in vertices2D)
        {
            float dist = (v.x - mouseEnter.x) * (mouseExit.y - mouseEnter.y) - (v.y - mouseEnter.y)*(mouseExit.x - mouseEnter.x);
            if (dist > 0)
            {
                side_a.Add(v);
            }
            else
            {
                side_b.Add(v);
            }
        }
        side_a.Add(mouseEnter);
        side_a.Add(mouseExit);

        side_b.Add(mouseEnter);
        side_b.Add(mouseExit);

        List<Vector2> organized_a = new List<Vector2>(), organized_b = new List<Vector2>();
        //Find center of both
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach(Vector2 v in side_a)
        {
            sum += v;
            count++;
        }
        Vector2 center_a = sum / count;
        //Find center of both
        sum = Vector2.zero;
        count = 0;
        foreach (Vector2 v in side_b)
        {
            sum += v;
            count++;
        }
        Vector2 center_b = sum / count;
        order(ref side_a, ref organized_a, center_a);
        order(ref side_b, ref organized_b, center_b);

        meshGen(organized_a.ToArray());
        meshGen(organized_b.ToArray());

        Destroy(gameObject); 
    }

    private void order(ref List<Vector2> inList, ref List<Vector2> outlist, Vector2 center)
    {
        
        if(outlist.Count < 1)
        {
            //Take starting point
            outlist.Add(inList[0]);
            inList.RemoveAt(0);
            order(ref inList, ref outlist, center);
        }
        else if (inList.Count == 1)
        {
            outlist.Add(inList[0]);
        }
        else
        {
            Vector2 relative = center - outlist[outlist.Count-1];
            Vector2 nextGuess = new Vector2(relative.y, -relative.x).normalized;
            int bestGuessIndex = 0;
            float tempDist = 0;
            for(int i = 0; i < inList.Count; i++)
            {
                float distance = Vector2.Distance(outlist[outlist.Count - 1] + nextGuess, inList[i]);
                if (tempDist == 0 || distance < tempDist)
                {
                    tempDist = distance;
                    bestGuessIndex = i;
                }
            }
            //After you have best guess, remove it from inList and append to outlist
            outlist.Add(inList[bestGuessIndex]);
            inList.RemoveAt(bestGuessIndex);

            //Call function again
            order(ref inList, ref outlist, center);
        }
    }

    private void meshGen(Vector2[] vertices)
    {
        var triangulator = new Triangulator(vertices);
        int[] indices = triangulator.Triangulate();

        Vector3[] vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices, v => v);
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (Vector3 v in vertices3D)
        {
            sum += v;
            count++;
        }
        Vector3 center = sum / count;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices3D;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GameObject empty = new GameObject("test");
        //empty.AddComponent(typeof(PolygonCollider2D));
        //empty.GetComponent<PolygonCollider2D>().points = vertices;
        //empty.layer = LayerMask.NameToLayer("Chunk");
        
        empty.AddComponent(typeof(MeshRenderer));
        MeshRenderer meshrenderer = empty.GetComponent<MeshRenderer>();
        meshrenderer.material = inMaterial;
        meshrenderer.material.color = new Color32(255, 255, 255, 255);
        MeshFilter filter = empty.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = mesh;

        empty.AddComponent(typeof(Drift));
        Drift drift = empty.GetComponent<Drift>();
        drift.center = center;
        drift.UpdatePosition(mouseEnter);

        //empty.AddComponent(typeof(Cut));
    }
}
