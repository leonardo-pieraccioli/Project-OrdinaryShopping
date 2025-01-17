using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ExampleCombinerImproved : MonoBehaviour
{
	void Start()
	{
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		List<CombineInstance> combines = new List<CombineInstance>();
		List<Material> materials = new List<Material>();

		for (int i = 0; i < meshFilters.Length; i++)
		{
			var mr = meshFilters[i].GetComponent<MeshRenderer>();
			for (int j = 0; j < mr.materials.Length; j++)
			{
				var combine = new CombineInstance();
				combine.mesh = meshFilters[i].mesh;
				combine.subMeshIndex = j;
				
                //combine.transform = meshFilters[i].transform.localToWorldMatrix;
                combine.transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
                if (meshFilters[i] != GetComponent<MeshFilter>())
    			{
		    		meshFilters[i].gameObject.SetActive(false);
	    		}
				combines.Add(combine);
				materials.Add( mr.materials[j]);
            }      
		}

		Mesh combinedMesh = new Mesh();
        combinedMesh.name = "CombinedMesh";
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combines.ToArray(), false);
        transform.GetComponent<MeshFilter>().mesh = combinedMesh; 
		gameObject.GetComponent<MeshRenderer>().materials = materials.ToArray();
	}
}