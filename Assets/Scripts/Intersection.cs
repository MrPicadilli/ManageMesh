using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{

    public MeshFilter meshFilterA;
    public MeshFilter meshFilterB;
    private void Start() {
        
    }

    //Möller–Trumbore intersection algorithm

    public List<int> intersect(){
        List<int> tabTriangle = new List<int>();

        return tabTriangle;
    }
    public void Yo(){

    }

    public void AreSecant(){
        Mesh meshA = meshFilterA.mesh;
        Mesh meshB = meshFilterB.mesh;
        Debug.Log("meshA normal :" + meshA.normals.Length);
        List<int> tabTriangle = new List<int>();
        Vector3 A1 = meshA.vertices[meshA.triangles[0]];
        Vector3 A2 = meshA.vertices[meshA.triangles[1]];
        Vector3 A3 = meshA.vertices[meshA.triangles[2]];
        Vector3 B1 = meshB.vertices[meshB.triangles[0]];
        Vector3 B2 = meshB.vertices[meshB.triangles[1]];
        Vector3 B3 = meshB.vertices[meshB.triangles[2]];
        Vector3 E1 = A2-A1;
        Vector3 E2 = A3-A1;
        Vector3.Cross(E1,E2);

    }
}
