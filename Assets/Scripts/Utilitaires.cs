using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilitaires : MonoBehaviour
{
    public static void InstantiateSphere(Vector3 position, float scale, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.tag = "Destructible";
        sphere.transform.position = position;
        sphere.GetComponent<MeshRenderer>().material.color = color;
    }

    public static void InstantiateCube(Vector3 position, float scale, Color color)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(scale, scale, scale);
        cube.tag = "Destructible";
        cube.transform.position = position;
        cube.GetComponent<MeshRenderer>().material.color = color;
    }
}
