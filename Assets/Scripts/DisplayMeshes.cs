using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DisplayMeshes : MonoBehaviour
{
    public struct Point
    {
        public Point(int indice, Vector3 coord)
        {
            nbRepetition = 1;
            indices = new List<int>();
            indices.Add(indice);
            coordonnee = coord;
        }
        public Point(int indice)
        {
            nbRepetition = 1;
            indices = new List<int>();
            indices.Add(indice);
            coordonnee = new Vector3();
        }
        public int nbRepetition;
        public List<int> indices;
        public Vector3 coordonnee;

        public override readonly string ToString()
        {
            string retour = $"nombre de répétition : {nbRepetition}, au coordonnée : {coordonnee} \n ";
            string temp = "indices : ";
            foreach (int indice in indices)
            {
                temp += $"{indice},";
            }
            retour += temp;
            return retour;
        }
        public bool hasIndice(int indiceToSearch)
        {
            foreach (int indice in indices)
            {
                if (indiceToSearch == indice)
                    return true;
            }
            return false;
        }
    }


    Dictionary<Vector3, Point> pointDictionary = new Dictionary<Vector3, Point>();
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    public Material[] MaterialsList;
    public Material materialOnSelect;
    public bool showVertices = false;
    public bool showSubmeshes = false;
    public bool showTriangles = false;
    private int nbTriangle;
    // Start is called before the first frame update
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        InitializeNbTriangle();
        AdjustMaterialSize();
        InitializePoint();
        PutMaterial();
    }
    void Start()
    {
        if(showVertices)
            ShowVertices();
        if(showTriangles)
            ShowTriangles();
        if(showSubmeshes)
            ShowSubmesh();

    }

    private void InitializePoint()
    {
        bool hasUpdated;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            hasUpdated = false;
            for (int y = 0; y < pointDictionary.Count; ++y)
            {
                if (pointDictionary.ElementAt(y).Key == mesh.vertices[i])
                {
                    Point x = pointDictionary[pointDictionary.ElementAt(y).Key];
                    x.nbRepetition++;
                    x.indices.Add(i);
                    pointDictionary[pointDictionary.ElementAt(y).Key] = x;
                    hasUpdated = true;
                    break;
                }
            }
            if (!hasUpdated)
            {
                Point p = new Point(i, mesh.vertices[i]);
                pointDictionary.Add(mesh.vertices[i], p);
            }

        }
    }

    private void PutMaterial()
    {
        mesh.subMeshCount = nbTriangle;
        
        int[] triangle = new int[3];
        for (int i = nbTriangle - 1; i >= 0; i--)
        {

            triangle[0] = mesh.triangles[i * 3];
            triangle[1] = mesh.triangles[i * 3 + 1];
            triangle[2] = mesh.triangles[i * 3 + 2];

            mesh.SetTriangles(triangle, i);
        }
        meshRenderer.materials = MaterialsList;
        // Recalculate bounds and normals for the mesh
        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
    }

    private void AdjustMaterialSize(){
        if(MaterialsList.Length == nbTriangle)
            return;
        else if(MaterialsList.Length < nbTriangle){
            Material[] MaterialsListTemp = new Material[nbTriangle];
            for (int i = 0; i < MaterialsListTemp.Length; i++)
            {
                MaterialsListTemp[i] = MaterialsList[i%MaterialsList.Length];
            }
            MaterialsList = MaterialsListTemp;
        }else{
            Material[] MaterialsListTemp = new Material[nbTriangle];
            for (int i = 0; i < MaterialsListTemp.Length; i++)
            {
                MaterialsListTemp[i] = MaterialsList[i%MaterialsList.Length];
            }
            MaterialsList = MaterialsListTemp;
        }
    }



    public int[] InterpretTriangle(int[] triangle)
    {
        Debug.Log("InterpretTriangle nb triangle : " + triangle.Length / 3);
        bool firstBarrier = false;
        bool secondBarrier = false;
        bool thirdBarrier = false;
        int[] triangleIndice = new int[triangle.Length / 3];


        int index = 0;
        for (int i = 0; i * 3 < triangle.Length; i++)
        {
            for (int y = 0; y * 3 < mesh.triangles.Length; y++)
            {
                firstBarrier = false;
                secondBarrier = false;
                thirdBarrier = false;
                if (triangle[i * 3] == mesh.triangles[y * 3])
                    firstBarrier = true;
                if (triangle[i * 3 + 1] == mesh.triangles[y * 3 + 1])
                    secondBarrier = true;
                if (triangle[i * 3 + 2] == mesh.triangles[y * 3 + 2])
                    thirdBarrier = true;
                if (firstBarrier && secondBarrier && thirdBarrier)
                {
                    triangleIndice[index] = y;
                    index++;
                    break;
                }
            }
        }
        AddColor(triangleIndice);

        for (int i = 0; i < triangle.Length; i++)
        {
            foreach (var point in pointDictionary)
            {
                if (point.Value.hasIndice(triangle[i]))
                    triangle[i] = point.Value.indices[0];
            }
        }



        return triangle;
    }

    //it seems that unity not manage the fact to change just one submesh among 
    //all the other one exept if you jut want  to change parameters like the color
    //so you have to put another array of material
    private void AddColor(int[] triangleIndice)
    {
        Debug.Log("AddColor");
        Material[] MaterialListTemp = meshRenderer.materials;
        foreach (int item in triangleIndice)
        {
            MaterialListTemp[item] = materialOnSelect;
        }
        meshRenderer.materials = MaterialListTemp;
    }

    public void ClearColor()
    {
        meshRenderer.materials = MaterialsList;
    }

    private void ShowVertices()
    {
        Debug.Log("ShowVertices");

        Debug.Log("number of vertices with repetition : " + mesh.vertexCount);
        Debug.Log(" number of vertices with no repetition : " + pointDictionary.Count);
        for (int i = 0; i < pointDictionary.Count; ++i)
        {
            Debug.Log(pointDictionary.ElementAt(i));
        }


    }
    private void ShowSubmesh()
    {
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            Debug.Log("Vertex Indices for Submesh " + i + ": " + string.Join(", ", mesh.GetIndices(i)) + " color : " +
            meshRenderer.materials[i].name);
        }
    }
    private void ShowTriangles()
    {
        Debug.Log("ShowTriangles");
        Debug.Log("triangles length " + mesh.triangles.Length);
        Debug.Log("vertices length " + mesh.vertices.Length);
        int index;
        for (int i = 0; i * 3 < mesh.triangles.Length; i++)
        {
            index = 0;
            Debug.Log("triangle " + i + " : " + mesh.triangles[i * 3] + "," + mesh.triangles[i * 3 + 1] + "," + mesh.triangles[i * 3 + 2]);
            foreach (var point in pointDictionary)
            {
                if (point.Value.hasIndice(mesh.triangles[i * 3]))
                    Debug.Log(index + " : " + point.Value);
                if (point.Value.hasIndice(mesh.triangles[i * 3 + 1]))
                    Debug.Log(index + " : " + point.Value);
                if (point.Value.hasIndice(mesh.triangles[i * 3 + 2]))
                    Debug.Log(index + " : " + point.Value);
                index++;
            }
            Debug.Log("endTriangle");
        }

    }
    private void InitializeNbTriangle()
    {
        int temp = 0;
        for (int i = 0; i * 3 < mesh.triangles.Length; i++)
        {
            temp = i;
        }
        nbTriangle = temp + 1;

    }

}
