using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ArrayInstanceProduct : MonoBehaviour
{
    private Mesh _mesh;
    private Material[] _materials;

    private int _xn = 1, _yn = 1, _zn = 1;
    private Vector3 _offset;
    private bool _rotate = true;
    [SerializeField] private int _grabbedCount = 0;
    [SerializeField] private bool _grabOne = false, _putOne = false;

    private Stack<Matrix4x4> _matrices, _removed;
    public Productinfo product;

    private bool _isInitialized = false;

    public void GrabObject()
    {
        if (_grabbedCount == _xn * _yn * _zn)
            return;

        gameObject.SetActive(false);
        _grabbedCount++;
        gameObject.SetActive(true);
    }

    public void PutObject()
    {
        if (_grabbedCount == 0)
            return;

        gameObject.SetActive(false);
        _grabbedCount--;
        gameObject.SetActive(true);
    }
    public void Init(Productinfo productInfo)
    {
        product = productInfo;
        if (productInfo == null) return;

        _xn = productInfo._xn;
        _yn = productInfo._yn;
        _zn = productInfo._zn;
        _offset = productInfo._offset;
        _rotate = productInfo._rotate;
        /* _grabbedCount = productInfo._grabbedCount;
        _grabOne = productInfo._grabOne;
        _putOne = productInfo._putOne; */

        InitializeArr();

        _isInitialized = true;


    }
    public void InitializeArr()
    {
        //get collider to get gameobject dimension
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.Log($"collider not found in gameobject {gameObject.name}");
        }
        //get mesh renderer for materials
        _mesh = GetComponent<MeshFilter>().mesh;
        if (_mesh == null)
        {
            Debug.Log($"mesh not found in gameobject {gameObject.name}");
        }
        MeshRenderer mr = GetComponent<MeshRenderer>();
        _materials = mr.materials;
        foreach (Material m in _materials)
        {
            if (m == null)
            {
                Debug.Log($"material not found in gameobject {gameObject.name}");
            }
            m.enableInstancing = true;
        }

        //Block original object render to avoid sovrapposition
        mr.enabled = false;

        // Create transformation matrices
        _matrices = new Stack<Matrix4x4>();
        _removed = new Stack<Matrix4x4>();
        Vector3 startPos = transform.position;
        for (int i = 0; i < _xn; i++)
        {
            for (int k = 0; k < _zn; k++)
            {
                for (int j = _yn - 1; j >= 0; j--)
                {
                    //skip grabbed objects
                    Vector3 currentOffset = _offset;
                    currentOffset.Scale(new Vector3(i, j, k));
                    currentOffset.Scale(collider.size);
                    Vector3 position = currentOffset + startPos;
                    _matrices.Push(
                        Matrix4x4.TRS(position, 
              _rotate ? transform.rotation * Quaternion.Euler(Vector3.up * Random.Range(0, 180)) 
                      : transform.rotation,
              transform.localScale));

                }
            }
        }

        //new collider to match the stock size
        BoxCollider newCollider = gameObject.AddComponent<BoxCollider>();
        newCollider.center += Vector3.Scale(collider.size, new Vector3(_xn, _yn, _zn) / 2) - collider.size / 2;
        newCollider.size = Vector3.Scale(collider.size, new Vector3(_xn, _yn, _zn));
        collider.enabled = false;
        //reverse the stack order
        _matrices = new Stack<Matrix4x4>(_matrices);
    }

    void OnEnable()
    {

        if (!_isInitialized) return;

        while (_grabbedCount > _removed.Count)
        {
            _removed.Push(_matrices.Pop());
            return;
        }
        while (_removed.Count > _grabbedCount)
        {
            _matrices.Push(_removed.Pop());
        }
    }

    void Update()

    {

        if (!_isInitialized) return;
        /////this piece is for debug only////
        if (_grabOne == true)
        {
            GrabObject();
            _grabOne = false;
        }
        if (_putOne == true)
        {
            PutObject();
            _putOne = false;
        }
        /////////////////////////
        for (int i = 0; i < _materials.Length; ++i)
        {
            if (_materials[i] != null)
            {
                Graphics.DrawMeshInstanced(_mesh, i, _materials[i], _matrices.ToArray());
            }
        }
    }

}
