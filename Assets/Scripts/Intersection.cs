using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Triangle
{
    public Vector3 vertex1;
    public Vector3 vertex2;
    public Vector3 vertex3;
    public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        this.vertex1 = vertex1;
        this.vertex2 = vertex2;
        this.vertex3 = vertex3;
    }
    public Triangle()
    {
        this.vertex1 = Vector3.one;
        this.vertex2 = Vector3.one;
        this.vertex3 = Vector3.one;
    }
}
public class Intersection : MonoBehaviour
{
    public float epsilon = 0.1f;
    public MeshFilter meshFilterA;
    public MeshFilter meshFilterB;
    private void Start()
    {

    }

    //Möller–Trumbore intersection algorithm

    public List<int> intersect()
    {
        List<int> tabTriangle = new List<int>();

        return tabTriangle;
    }
    public void Yo()
    {

    }

    static bool AreParallel(Vector3 normal1, Vector3 normal2, float epsilon = 1e-5f)
    {
        // Check if the dot product of the normal vectors is close to 1 or -1 (parallel).
        float dotProduct = Vector3.Dot(normal1, normal2);
        return Math.Abs(dotProduct - 1.0f) < epsilon || Math.Abs(dotProduct + 1.0f) < epsilon;
    }

    public static Vector3 CalculateAxis(Vector3 e1, Vector3 e2, Vector3 f1, Vector3 f2)
    {
        // Calculate normal vectors
        Vector3 nA = Vector3.Cross(e1, e2);
        Vector3 nB = Vector3.Cross(f1, f2);

        // Calculate axis perpendicular to the planes of both triangles
        Vector3 axis = Vector3.Cross(nA, nB);

        // Normalize the axis
        axis.Normalize();

        return axis;
    }

    private static bool EdgeIntersect(Vector3 v1, Vector3 v2, Vector3 u1, Vector3 u2, Vector3 normal)
    {
        Vector3 d = u1 - v1;
        Vector3 h = Vector3.Cross(d, u2 - u1);
        float s = Vector3.Dot(d, normal);
        float t = Vector3.Dot(h, v2 - v1);

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1 && (s + t) <= 1)
        {
            // Edge intersection found
            return true;
        }

        // No edge intersection
        return false;
    }

    private static bool EdgeIntersect1(Vector3 v1, Vector3 v2, Vector3 u1, Vector3 u2, Vector3 normal, float epsilon = 1e-10f)
    {
        Vector3 d = u1 - v1;
        Vector3 h = Vector3.Cross(d, u2 - u1);
        float s = Vector3.Dot(d, normal);
        float t = Vector3.Dot(h, v2 - v1);

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            Debug.Log(MathF.Abs(s + t - 1) );
            // Edge is intersecting, but we need to check if it's a partial intersection
            //float epsilon = 1e-10f; // Adjust as needed for numerical stability
            
            if (MathF.Abs(s + t - 1) < epsilon)
            {
                // The intersection is on the edge (partial intersection)
                return false;
            }

            // If not a partial intersection, it's a full intersection
            return MathF.Abs(s + t) > epsilon;
        }

        // No edge intersection
        return false;
    }

    public static bool OverlapOnAxis(Vector3 axis, Triangle triangleA, Triangle triangleB)
    {
        // Project the vertices of both triangles onto the axis
        float projectionA1 = Vector3.Dot(axis, triangleA.vertex1);
        float projectionA2 = Vector3.Dot(axis, triangleA.vertex2);
        float projectionA3 = Vector3.Dot(axis, triangleA.vertex3);

        float projectionB1 = Vector3.Dot(axis, triangleB.vertex1);
        float projectionB2 = Vector3.Dot(axis, triangleB.vertex2);
        float projectionB3 = Vector3.Dot(axis, triangleB.vertex3);

        // Find the minimum and maximum projections for each triangle
        float minA = Math.Min(projectionA1, Math.Min(projectionA2, projectionA3));
        float maxA = Math.Max(projectionA1, Math.Max(projectionA2, projectionA3));

        float minB = Math.Min(projectionB1, Math.Min(projectionB2, projectionB3));
        float maxB = Math.Max(projectionB1, Math.Max(projectionB2, projectionB3));

        // Check for overlap on the axis
        return !(maxA < minB || maxB < minA);
    }

    public void PrintSecantTriangle()
    {
        DisplayMeshes displayMeshesA = meshFilterA.gameObject.GetComponent<DisplayMeshes>();
        DisplayMeshes displayMeshesB = meshFilterB.gameObject.GetComponent<DisplayMeshes>();
        Mesh meshA = meshFilterA.mesh;
        Mesh meshB = meshFilterB.mesh;
        Quaternion rotationOffsetA = meshFilterA.gameObject.transform.localRotation;
        Vector3 positionOffsetA = meshFilterA.gameObject.transform.position;

        Quaternion rotationOffsetB = meshFilterB.gameObject.transform.localRotation;
        Vector3 positionOffsetB = meshFilterB.gameObject.transform.position;

        Triangle A = new Triangle();
        Triangle B = new Triangle();
        // Debug.Log("meshA.triangles.Length " + meshA.triangles.Length);
        // Debug.Log("meshB.triangles.Length " + meshB.triangles.Length);
        List<int> listTriangleSecantA = new List<int>();
        List<int> listTriangleSecantB = new List<int>();


        // A.vertex1 = rotationOffsetA * meshA.vertices[meshA.triangles[0]] + positionOffsetA;
        // A.vertex2 = rotationOffsetA * meshA.vertices[meshA.triangles[1]] + positionOffsetA;
        // A.vertex3 = rotationOffsetA * meshA.vertices[meshA.triangles[2]] + positionOffsetA;

        // B.vertex1 = rotationOffsetB * meshB.vertices[meshB.triangles[0]] + positionOffsetB;
        // B.vertex2 = rotationOffsetB * meshB.vertices[meshB.triangles[1]] + positionOffsetB;
        // B.vertex3 = rotationOffsetB * meshB.vertices[meshB.triangles[2]] + positionOffsetB;
        // AreSecant(A, B);
        Debug.Log(meshB.triangles.Length);
        for (int i = 0; i < meshA.triangles.Length; i+=3)
        {            
            // Debug.Log("i : " + i);
            A.vertex1 = rotationOffsetA * meshA.vertices[meshA.triangles[i]] + positionOffsetA;
            A.vertex2 = rotationOffsetA * meshA.vertices[meshA.triangles[i+1]] + positionOffsetA;
            A.vertex3 = rotationOffsetA * meshA.vertices[meshA.triangles[i+2]] + positionOffsetA;
            for (int j = 0; j < meshB.triangles.Length; j+=3)
            {
                // Debug.Log("j : " + j);
                B.vertex1 = rotationOffsetB * meshB.vertices[meshB.triangles[j]] + positionOffsetB;
                B.vertex2 = rotationOffsetB * meshB.vertices[meshB.triangles[j+1]] + positionOffsetB;
                B.vertex3 = rotationOffsetB * meshB.vertices[meshB.triangles[j+2]] + positionOffsetB;
                
                if(AreSecant(A,B,epsilon)){
                    // listTriangleSecantA.Add(i);
                    listTriangleSecantA.Add(meshA.triangles[i]);
                    listTriangleSecantA.Add(meshA.triangles[i+1]);
                    listTriangleSecantA.Add(meshA.triangles[i+2]);
                    // listTriangleSecantB.Add(j);
                    listTriangleSecantB.Add(meshB.triangles[j]);
                    listTriangleSecantB.Add(meshB.triangles[j+1]);
                    listTriangleSecantB.Add(meshB.triangles[j+2]);
                }
            }
        }
        
        // displayMeshesA.InterpretTriangle(listTriangleSecantA.ToArray());
        displayMeshesB.InterpretTriangle(listTriangleSecantB.ToArray());

    }

    public static bool AreSecant(Triangle A, Triangle B, float epsilon)
    {

        // Debug.Log("meshA normal :" + meshA.normals.Length);
        List<int> tabTriangle = new List<int>();


        Vector3 A1 = A.vertex1;
        Vector3 A2 = A.vertex2;
        Vector3 A3 = A.vertex3;



        Vector3 B1 = B.vertex1;
        Vector3 B2 = B.vertex2;
        Vector3 B3 = B.vertex3;
        /*
        Utilitaires.InstantiateCube(A.vertex1,0.1f,Color.magenta);
        Utilitaires.InstantiateCube(A.vertex2,0.1f,Color.magenta);
        Utilitaires.InstantiateCube(A.vertex3,0.1f,Color.magenta);

        Utilitaires.InstantiateCube(B.vertex1,0.1f,Color.red);
        Utilitaires.InstantiateCube(B.vertex2,0.1f,Color.red);
        Utilitaires.InstantiateCube(B.vertex3,0.1f,Color.red);
        */



        Vector3 E1 = A2 - A1;
        Vector3 E2 = A3 - A1;

        Vector3 F1 = B2 - B1;
        Vector3 F2 = B3 - B1;

        Vector3 n = Vector3.Cross(E1, E2);
        Vector3 normalA = Vector3.Cross(A2 - A1, A3 - A1);
        Vector3 normalB = Vector3.Cross(B2 - B1, B3 - B1);
        if (AreParallel(normalA, normalB))
        {
            Debug.Log("normal parallel so impossible to intersect");
            return false;
        }
        Vector3 axis = CalculateAxis(E1, E2, F1, F2);

        if (!OverlapOnAxis(axis, A, B))
        {
            Debug.Log("don't overlap on axis");
            return false;
        }


        // Perform edge tests
        if (EdgeIntersect1(A1, A2, B1, B2, n,epsilon) ||
            EdgeIntersect1(A2, A3, B1, B2, n,epsilon) ||
            EdgeIntersect1(A3, A1, B1, B2, n,epsilon) ||
            EdgeIntersect1(B1, B2, A1, A2, n,epsilon) ||
            EdgeIntersect1(B2, B3, A1, A2, n,epsilon) ||
            EdgeIntersect1(B3, B1, A1, A2, n,epsilon))
        {
            Debug.Log("intersect");

            return true;
        }
        Debug.Log("not intersect");
        return false;

    }
}
