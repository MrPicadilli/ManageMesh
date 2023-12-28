using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
    public static MeshManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<int> IsInsideTriangle(Mesh mesh, Vector3 point)
    {
        /*
        Vector3 A = transform.TransformPoint(mesh.vertices[mesh.triangles[0]]);
        Vector3 B = transform.TransformPoint(mesh.vertices[mesh.triangles[1]]);
        Vector3 C = transform.TransformPoint(mesh.vertices[mesh.triangles[2]]);

        Utilitaires.InstantiateCube(A, 0.1f, Color.black);
        Utilitaires.InstantiateCube(B, 0.1f, Color.black);
        Utilitaires.InstantiateCube(C, 0.1f, Color.black);
        */
        Vector3 a = mesh.vertices[mesh.triangles[0]];
        Vector3 b = mesh.vertices[mesh.triangles[1]];
        Vector3 c = mesh.vertices[mesh.triangles[2]];
        

        Utilitaires.InstantiateCube(a, 0.1f, Color.blue);
        Utilitaires.InstantiateCube(b, 0.1f, Color.blue);
        Utilitaires.InstantiateCube(c, 0.1f, Color.blue);

        Debug.Log("point triangle : " + mesh.triangles[0] + ", coord :" + a);
        Debug.Log("point triangle : " + mesh.triangles[1] + ", coord :" + b);
        Debug.Log("point triangle : " + mesh.triangles[2] + ", coord :" + c);
        /*
        Debug.Log("point triangle : " + mesh.triangles[0] + ", coord :" + A);
        Debug.Log("point triangle : " + mesh.triangles[1] + ", coord :" + B);
        Debug.Log("point triangle : " + mesh.triangles[2] + ", coord :" + C);
        Debug.Log("point impact : " + transform.TransformPoint(point));
        Debug.Log("point impact world : " + point);
        Debug.Log("1 : " + IsPointInTriangle(point, A, B, C));
        Debug.Log("2 : " + IsPointInTriangle2(point, A, B, C));
        Debug.Log("3 : " + IsPointInTriangle3(point, A, B, C));
        Debug.Log("4 : " + IsPointInTriangle4(point, A, B, C));
        Debug.Log("5 : " + IsPointInTriangle5(point, A, B, C));
        */
        Debug.Log("1bis : " + IsPointInTriangle(point, a, b, c));
        Debug.Log("2bis : " + IsPointInTriangle2(point, a, b, c));
        Debug.Log("3bis : " + IsPointInTriangle3(point, a, b, c));
        Debug.Log("4bis : " + IsPointInTriangle4(point, a, b, c));
        Debug.Log("5bis : " + IsPointInTriangle5(point, a, b, c));
        //*/
        int[] triangles = mesh.triangles;
        List<int> triangleFound = new List<int>();
        // Iterate over triangles (every 3 indices form a triangle)
        
        for (int i = 0; i < triangles.Length; i += 3)
        {  
            Debug.Log(i);
            int index1 = triangles[i];
            int index2 = triangles[i + 1];
            int index3 = triangles[i + 2];
            Vector3 vertex1 = mesh.vertices[index1];
            Vector3 vertex2 = mesh.vertices[index2];
            Vector3 vertex3 = mesh.vertices[index3];

            
            /*
            Vector3 vertex1 = transform.TransformPoint(mesh.vertices[index1]);
            Vector3 vertex2 = transform.TransformPoint(mesh.vertices[index2]);
            Vector3 vertex3 = transform.TransformPoint(mesh.vertices[index3]);
            */
            // Check if the hit point is inside the current triangle
            if (IsPointInTriangle5(point, vertex1, vertex2, vertex3))
            {
                triangleFound.Add(index1);
                triangleFound.Add(index2);
                triangleFound.Add(index3);
                Debug.Log("Point is inside the triangle!");
                //break; // You may want to exit the loop if the point is found inside one triangle
            }
        }
        //*/
        return triangleFound;
    }

    // Check if a point is inside a triangle
    private bool IsPointInTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        // Calculate barycentric coordinates
        Vector3 v0 = vertex2 - vertex1;
        Vector3 v1 = vertex3 - vertex1;
        Vector3 v2 = point - vertex1;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Check if point is inside the triangle
        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }

    private bool IsPointInTriangle2(Vector3 P, Vector3 A, Vector3 B, Vector3 C)
    {
        float TriangleSurface = Mathf.Abs(Vector3.Distance(A, B) * Vector3.Distance(A, C)) / 2.0f;
        float alpha = Mathf.Abs(Vector3.Distance(P, B) * Vector3.Distance(P, B)) / (2 * TriangleSurface);
        float beta = Mathf.Abs(Vector3.Distance(P, C) * Vector3.Distance(P, A)) / (2 * TriangleSurface);
        float gamma = 1.0f - alpha - beta;

        bool alphaCondition = alpha >= 0.0f && alpha <= 1.0f;
        bool betaCondition = beta >= 0.0f && beta <= 1.0f;
        bool gammaCondition = gamma >= 0.0f && gamma <= 1.0f;
        bool sumCondition = alpha + beta + gamma == 1.0f;

        return alphaCondition && betaCondition && gammaCondition && sumCondition;
    }

    private bool SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b)
    {
        Vector3 cp1 = Vector3.Cross(b - a, p1 - a);
        Vector3 cp2 = Vector3.Cross(b - a, p2 - a);
        bool dot = Vector3.Dot(cp1, cp2) >= 0;
        return dot;
    }


    private bool IsPointInTriangle3(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        if (SameSide(p, a, b, c) && SameSide(p, b, a, c) && SameSide(p, c, a, b))
        {
            return true;
        }
        return false;
    }

    private bool IsPointInTriangle4(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
    {
        Vector3 d, e;
        float w1, w2;
        d = b - a;
        e = c - a;
        w1 = (e.x * (a.y - p.y) + e.y * (p.x - a.x)) / (d.x * e.y - d.y * e.x);
        w2 = (p.y - a.y - w1 * d.y) / e.y;
        return (w1 >= 0.0) && (w2 >= 0.0) && ((w1 + w2) <= 1.0);
    }

    bool IsPointInTriangle5(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
{
    Vector3 d, e;
    double w1, w2;
    d = b - a;
    e = c - a;
 
    if (Mathf.Approximately(e.y, 0))
    {
        e.y = 0.0001f;
    }
     
    w1 = (e.x * (a.y - p.y) + e.y * (p.x - a.x)) / (d.x * e.y - d.y * e.x);
    w2 = (p.y - a.y - w1 * d.y) / e.y;
    return (w1 >= 0f) && (w2 >= 0.0) && ((w1 + w2) <= 1.0);
}



}
