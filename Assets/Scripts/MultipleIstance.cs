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
    [SerializeField] private int _objectGrabbed = 0;
    [SerializeField] private bool grabOne = false;
    
    private Matrix4x4[] _matrices;

    public void GrabObject()
    {
        gameObject.SetActive(false);
        _objectGrabbed++;
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        //get collider to get gameobject dimension
        BoxCollider collider = GetComponent<BoxCollider>();
        if(collider == null) {
            Debug.Log($"collider not found in gameobject {gameObject.name}");
        }
        //get mesh renderer for materials
        _mesh = GetComponent<MeshFilter>().mesh;
        if(_mesh == null) {
            Debug.Log($"mesh not found in gameobject {gameObject.name}");
        }
        MeshRenderer mr = GetComponent<MeshRenderer>();
        _materials = mr.materials;
        foreach(Material m in _materials)
        {
            if(m == null) 
            {
                Debug.Log($"material not found in gameobject {gameObject.name}");
            }
            m.enableInstancing = true;
        }
        
        //Block original object render to avoid sovrapposition
        mr.enabled = false; 

        // Create transformation matrices
        _matrices = new Matrix4x4[xn * yn * zn];
        Vector3 startPos = transform.position;
        int objRemovedCount = _objectGrabbed;
        for (int i = 0; i < xn; i++)
        {
            for (int k = 0; k < zn; k++)
            {
                for (int j = yn - 1; j >= 0; j--)
                {
                    //skip grabbed objects
                    if(objRemovedCount > 0)
                    {
                        objRemovedCount--;
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
        /////this piece is for debug only////
        if(grabOne == true)
        {
            GrabObject();
            grabOne = false;
        }
        /////////////////////////
        
        Graphics.DrawMeshInstanced(_mesh, 0, _materials[0], _matrices);
        Graphics.DrawMeshInstanced(_mesh, 1, _materials[1], _matrices);
    }
}