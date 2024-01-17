using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle{
    public Vector3 vertex1;
    public Vector3 vertex2;
    public Vector3 vertex3;
    public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
   {
      this.vertex1 = vertex1;
      this.vertex2 = vertex2;
      this.vertex3 = vertex3;
   }
}
public class Intersection : MonoBehaviour
{

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

    Vector3 CalculateAxis(Vector3 e1, Vector3 e2, Vector3 f1, Vector3 f2)
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
    public void AreSecant()
    {
        Mesh meshA = meshFilterA.mesh;
        Mesh meshB = meshFilterB.mesh;
        Debug.Log("meshA normal :" + meshA.normals.Length);
        List<int> tabTriangle = new List<int>();
        Vector3 A1 = meshA.vertices[meshA.triangles[0]];
        Vector3 A2 = meshA.vertices[meshA.triangles[1]];
        Vector3 A3 = meshA.vertices[meshA.triangles[2]];
        Triangle A = new Triangle(A1,A2,A3);

        Vector3 B1 = meshB.vertices[meshB.triangles[0]];
        Vector3 B2 = meshB.vertices[meshB.triangles[1]];
        Vector3 B3 = meshB.vertices[meshB.triangles[2]];
        Triangle B = new Triangle(B1,B2,B3);

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
        }
        Vector3 axis = CalculateAxis(E1,E2,F1,F2);
        
        if(!OverlapOnAxis(axis,A,B)){
            Debug.Log("don't overlap on axis");
        }
        // Perform edge tests
        if (EdgeIntersect(A1, A2, B1, B2, n) ||
            EdgeIntersect(A2, A3, B1, B2, n) ||
            EdgeIntersect(A3, A1, B1, B2, n) ||
            EdgeIntersect(B1, B2, A1, A2, n) ||
            EdgeIntersect(B2, B3, A1, A2, n) ||
            EdgeIntersect(B3, B1, A1, A2, n))
        {
            Debug.Log("intersect");
        }
        Debug.Log("not intersect");

    }
}
