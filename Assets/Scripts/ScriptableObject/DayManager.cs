using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public DayData dayData; // Riferimento allo ScriptableObject

    void Start()
    {
        GenerateCubes();
    }

    void GenerateCubes()
    {
        for (int i = 0; i < dayData.numberOfCubes; i++)
        {
            // Crea un nuovo cubo
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            // Posiziona il cubo in modo casuale nella scena
            cube.transform.position = new Vector3(i * 1.5f, 0, 0);
            
            // Cambia il colore del cubo
            Renderer renderer = cube.GetComponent<Renderer>();
            renderer.material.color = dayData.cubeColor;
        }
    }

}
