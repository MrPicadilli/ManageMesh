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

    Vector3 CalculateOffset(Quaternion rotationOffset, Vector3 positionOffset )
    {
        // Transform the position offset into the local space of the rotation
        positionOffset = rotationOffset * positionOffset;

        // Invert the rotation to compensate for local space transformation
        //Quaternion inverseRotation = Quaternion.Inverse(rotationOffset);
        //positionOffset = inverseRotation * positionOffset;

        return positionOffset;
    }


    Vector3 CalculateOffset()
    {
        // Calculate the offset based on the reference object's local space
        Vector3 positionOffset = transform.InverseTransformPoint(transform.position);
        Quaternion rotationOffset = Quaternion.Inverse(transform.rotation) * transform.rotation;

        // Transform the position offset into the local space of the rotation
        positionOffset = Quaternion.Inverse(rotationOffset) * positionOffset;

        return positionOffset;
    }
    public List<int> IsInsideTriangle(MeshFilter meshFilter, Vector3 point)
    {
        Mesh mesh = meshFilter.mesh;
        Transform transformMesh = meshFilter.gameObject.transform;
        Vector3 offset = CalculateOffset(transformMesh.rotation,transformMesh.position);

        Quaternion rotationOffset = transformMesh.localRotation;
        Quaternion rotationOffset2 = Quaternion.Euler(transformMesh.localEulerAngles);
        Quaternion rotationOffset1 = Quaternion.Inverse(transformMesh.rotation);

        
        Vector3 positionOffset = transformMesh.position;
        Vector3 positionOffset1 = transformMesh.InverseTransformPoint(transformMesh.position);

        rotationOffset1 = Quaternion.Inverse(transformMesh.localRotation) * transform.rotation;
        Vector3 offset1 = rotationOffset1 * positionOffset1;
        offset = Quaternion.Inverse(transformMesh.localRotation) * positionOffset;

        Debug.Log("transformMesh.localEulerAngles" + transformMesh.localEulerAngles);

        Debug.Log("localRotation" + transformMesh.localRotation);
    


         // step 1 : offset = Quaternion.Inverse(transformMesh.localRotation) * positionOffset; -> Vector3 a = mesh.vertices[mesh.triangles[0]] + offset;
         // step 2 : a = rotationOffset * a; -> Quaternion rotationOffset = transformMesh.localRotation;
        /*
        Vector3 A = transform.TransformPoint(mesh.vertices[mesh.triangles[0]])+ transformMesh.position ;
        Vector3 B = transform.TransformPoint(mesh.vertices[mesh.triangles[1]]) + transformMesh.position;
        Vector3 C = transform.TransformPoint(mesh.vertices[mesh.triangles[2]]) + transformMesh.position;
        
        
        Utilitaires.InstantiateCube(A, 0.1f, Color.black);
        Utilitaires.InstantiateCube(B, 0.1f, Color.black);
        Utilitaires.InstantiateCube(C, 0.1f, Color.black);
        */
        
        Vector3 a = rotationOffset * mesh.vertices[mesh.triangles[0]] ;
        Vector3 b = rotationOffset * mesh.vertices[mesh.triangles[1]] ;
        Vector3 c =  rotationOffset * mesh.vertices[mesh.triangles[2]] ;

        Utilitaires.InstantiateCube(a, 0.1f, Color.yellow);
        Utilitaires.InstantiateCube(b, 0.1f, Color.yellow);
        Utilitaires.InstantiateCube(c, 0.1f, Color.yellow);

        // Debug.Log("point triangle : " + mesh.triangles[0] + ", coord :" + a);
        // Debug.Log("point triangle : " + mesh.triangles[1] + ", coord :" + b);
        // Debug.Log("point triangle : " + mesh.triangles[2] + ", coord :" + c);

        a += positionOffset;
        b += positionOffset;
        c += positionOffset;
        Utilitaires.InstantiateCube(a, 0.1f, Color.blue);
        Utilitaires.InstantiateCube(b, 0.1f, Color.blue);
        Utilitaires.InstantiateCube(c, 0.1f, Color.blue);
        
        // Debug.Log("point triangle : " + mesh.triangles[0] + ", coord :" + a);
        // Debug.Log("point triangle : " + mesh.triangles[1] + ", coord :" + b);
        // Debug.Log("point triangle : " + mesh.triangles[2] + ", coord :" + c);
        
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
        /*
        Debug.Log("1bis : " + IsPointInTriangle(point, a, b, c));
        Debug.Log("2bis : " + IsPointInTriangle2(point, a, b, c));
        Debug.Log("3bis : " + IsPointInTriangle3(point, a, b, c));
        Debug.Log("4bis : " + IsPointInTriangle4(point, a, b, c));
        Debug.Log("5bis : " + IsPointInTriangle5(point, a, b, c));
        Debug.Log("6bis : " + IsPointInTriangle6(point, a, b, c));
        Debug.Log("7bis : " + IsPointInTriangle7(point, a, b, c));
        -*
        //*/
        int[] triangles = mesh.triangles;
        List<int> triangleFound = new List<int>();
        // Iterate over triangles (every 3 indices form a triangle)
        Utilitaires.InstantiateSphere(point, 0.1f, Color.yellow);
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int index1 = triangles[i];
            int index2 = triangles[i + 1];
            int index3 = triangles[i + 2];
            Vector3 vertex1 = rotationOffset * mesh.vertices[index1] + positionOffset;
            Vector3 vertex2 = rotationOffset * mesh.vertices[index2] + positionOffset;
            Vector3 vertex3 = rotationOffset * mesh.vertices[index3] + positionOffset;


            /*
            Vector3 vertex1 = transform.TransformPoint(mesh.vertices[index1]);
            Vector3 vertex2 = transform.TransformPoint(mesh.vertices[index2]);
            Vector3 vertex3 = transform.TransformPoint(mesh.vertices[index3]);
            */
            // Check if the hit point is inside the current triangle
            if (ArePointsCoplanar(point, vertex1, vertex2, vertex3))
            {
                if (IsPointInTriangle(point, vertex1, vertex2, vertex3))
                {
                    triangleFound.Add(index1);
                    triangleFound.Add(index2);
                    triangleFound.Add(index3);
                    /*
                    Utilitaires.InstantiateCube(vertex1, 0.1f, Color.blue);
                    Utilitaires.InstantiateCube(vertex2, 0.1f, Color.blue);
                    Utilitaires.InstantiateCube(vertex3, 0.1f, Color.blue);
                    */
                    Debug.Log("Point is inside the triangle!");
                    //break; // You may want to exit the loop if the point is found inside one triangle
                }
            }

        }
        //*/
        return triangleFound;
    }

    // Check if a point is inside a triangle
    public bool IsPointInTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
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

    public bool IsPointInTriangle2(Vector3 P, Vector3 A, Vector3 B, Vector3 C)
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

    public bool SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b)
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

    public bool IsPointInTriangle4(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
    {
        Vector3 d, e;
        float w1, w2;
        d = b - a;
        e = c - a;
        w1 = (e.x * (a.y - p.y) + e.y * (p.x - a.x)) / (d.x * e.y - d.y * e.x);
        w2 = (p.y - a.y - w1 * d.y) / e.y;
        return (w1 >= 0.0) && (w2 >= 0.0) && ((w1 + w2) <= 1.0);
    }

    public bool IsPointInTriangle5(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
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

    public bool IsPointInTriangle6(Vector3 P, Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 crossProduct = Vector3.Cross(AB, AC);
        Vector3 AP = P - A;

        return Vector3.Dot(Vector3.Cross(AB, AP), crossProduct) >= 0 &&
               Vector3.Dot(Vector3.Cross(AC, AP), crossProduct) >= 0;
    }

    public bool IsPointInTriangle7(Vector3 P, Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 crossProduct = Vector3.Cross(AB, AC);
        Vector3 AP = P - A;

        float firstCondition = Mathf.Sign(Vector3.Dot(Vector3.Cross(AB, AP), crossProduct));
        float secondCondition = Mathf.Sign(Vector3.Dot(Vector3.Cross(AC, AP), crossProduct));
        Debug.Log("yoyo" + (firstCondition != secondCondition));
        return firstCondition != secondCondition;
    }

    public bool ArePointsCoplanar(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        // Vecteurs formés par trois points non colinéaires
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 AD = D - A;

        // Calcul du déterminant de la matrice formée par ces vecteurs
        float determinant = Vector3.Dot(Vector3.Cross(AB, AC), AD);

        Debug.Log("determinant " + determinant + " like zero " + Mathf.Approximately(determinant, 0f));

        return Mathf.Abs(determinant) < 0.10f;
        // Les points sont coplanaires si le déterminant est proche de zéro
        //return Mathf.Approximately(determinant, 0f);
    }



}
