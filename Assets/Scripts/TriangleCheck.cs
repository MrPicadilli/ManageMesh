using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCheck : MonoBehaviour
{
    public Transform point;
    public Transform vertexA;
    public Transform vertexB;
    public Transform vertexC;

    void Start()
    {
        bool isInsideTriangleAH = IsPointInsideTriangleAntiHoraire(point.position, vertexA.position, vertexB.position, vertexC.position);
        bool isInsideTriangleH = IsPointInsideTriangleHoraire(point.position, vertexA.position, vertexB.position, vertexC.position);
        if (isInsideTriangleAH)
        {
            Debug.Log("AH/ Le point est à l'intérieur du triangle.");
        }
        else
        {
            Debug.Log("AH/ Le point est à l'extérieur du triangle.");
        }
        if (isInsideTriangleH)
        {
            Debug.Log("H/ Le point est à l'intérieur du triangle.");
        }
        else
        {
            Debug.Log("H/ Le point est à l'extérieur du triangle.");
        }
    }

    bool IsPointInsideTriangleAntiHoraire(Vector3 P, Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 crossProduct = Vector3.Cross(AB, AC);
        Vector3 AP = P - A;
        Debug.Log(Vector3.Dot(Vector3.Cross(AB, AP), crossProduct));
        Debug.Log(Vector3.Dot(Vector3.Cross(AC, AP), crossProduct));
        float firstCondition = Mathf.Sign(Vector3.Dot(Vector3.Cross(AB, AP), crossProduct));
        float secondCondition = Mathf.Sign(Vector3.Dot(Vector3.Cross(AC, AP), crossProduct));
        return firstCondition!=secondCondition;
    }

    bool IsPointInsideTriangleHoraire(Vector3 P, Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 AB = B - A;
        Vector3 AC = C - A;
        Vector3 crossProduct = Vector3.Cross(AB, AC);
        Vector3 AP = P - A;
        Debug.Log(Vector3.Dot(Vector3.Cross(AB, AP), crossProduct));
        Debug.Log(Vector3.Dot(Vector3.Cross(AC, AP), crossProduct));
        return Vector3.Dot(Vector3.Cross(AB, AP), crossProduct) <= 0 &&
       Vector3.Dot(Vector3.Cross(AC, AP), crossProduct) <= 0;
    }
}
