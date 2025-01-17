using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ArrayInstances : MonoBehaviour
{
    private Mesh _mesh; 
    private Material[] _materials;

    [SerializeField] private int xn, yn, zn;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private bool _rotate = true;
    
    private Matrix4x4[] _matrices;

    void Start()
    {
        _offset += Vector3.one;
        BoxCollider collider = GetComponent<BoxCollider>();
        if(collider == null) {
            Debug.Log($"collider not found in gameobject {gameObject.name}");
        }
        _mesh = GetComponent<MeshFilter>().mesh;
        if(_mesh == null) {
            Debug.Log($"mesh not found in gameobject {gameObject.name}");
        }
        _materials = GetComponent<MeshRenderer>().materials;
        foreach(Material m in _materials)
        {
            if(m == null) 
            {
                Debug.Log($"material not found in gameobject {gameObject.name}");
            }
            m.enableInstancing = true;
        }
        

        // Create transformation matrices
        _matrices = new Matrix4x4[xn * yn * zn];
        Vector3 startPos = transform.position;
        bool skip = true;
        for (int i = 0; i < xn; i++)
        {
           for (int j = 0; j < yn; j++)
           {
                for (int k = 0; k < zn; k++)
                {
                    if(skip)
                    {
                        skip = !skip;
                        continue;
                    }

                    Vector3 currentOffset = _offset;
                    currentOffset.Scale(new Vector3(i,j,k));
                    currentOffset.Scale(collider.size);
                    Vector3 position =  currentOffset + startPos;
                    _matrices[(i * yn * zn) + (j * zn) + k] = 
                        Matrix4x4.TRS(position, 
                        _rotate? Quaternion.Euler(Vector3.up * Random.Range(0, 180)) : Quaternion.identity, 
                        Vector3.one);
                }
           }
        }        
    }

    void Update()
    {
        Graphics.DrawMeshInstanced(_mesh, 0, _materials[0], _matrices);
        Graphics.DrawMeshInstanced(_mesh, 1, _materials[1], _matrices);
    }
}