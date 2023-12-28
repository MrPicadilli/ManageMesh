using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInteractRaycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProjectRay(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            CleanObject(Input.mousePosition);
        }


    }
    private void ProjectRay(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.GetComponent<MeshInteractRaycast>() != null)
            {
                List<int> yo = MeshManager.instance.IsInsideTriangle(hit.collider.GetComponent<MeshFilter>().mesh, hit.point);
                Utilitaires.InstantiateSphere(hit.point, 0.1f);
                

                int[] copyTriangle = hit.collider.GetComponent<DisplayMeshes>().InterpretTriangle(yo.ToArray());
                PrintTriangle(copyTriangle);              
            }
        }
    }
    

    private void CleanObject(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.GetComponent<MeshInteractRaycast>() != null)
            {
                GameObject[] respawns = GameObject.FindGameObjectsWithTag("Destructible");
                foreach (GameObject item in respawns)
                {
                    Destroy(item);
                }
                hit.collider.GetComponent<DisplayMeshes>().ClearColor();



            }

        }
    }
    private void PrintTriangle(int[] copyTriangle)
    {
        string triangle = "triangle : ";
        foreach (int item in copyTriangle)
        {
            triangle += $"{item},";
        }
        Debug.Log(triangle);
    }


}
