using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UIElements;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    //----MultipleIstance
    private Mesh _mesh;
    private Material[] _materials;

    //private int _xn , _yn, _zn;
    //private Vector3 _offset;
    //private bool _rotate;
    //private int _grabbedCount;
    //private bool _grabOne, _putOne;

    private Stack<Matrix4x4> _matrices, _removed;
    //-----------------
    public Productinfo[] productInfo;
    private int nistance = 0;

    [SerializeField] Vector3[] storedPositions; //Trasform

    /*Array di GameObject di cui verranno estratte le posizioni.*/
    //[SerializeField] GameObject[] _objectPosition;
    [SerializeField] public PositionClass[] posClass;

    
    int numberShelve;
    private static ProductManager _instance;
    public static ProductManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ProductManager>();
            }

            return _instance;
        }
    }

    [ContextMenu("Store GameObjects Positions for SO")]

    public void Init(Productinfo[] currentProductInfo, int currentDay)


    {
        if (currentProductInfo == null)
        {
            Debug.LogError("Errore: currentProductInfo è nullo");
            return;
        }
        if (currentDay > 1)
        {
            //prima elimino tutti i vecchi oggetti dalla scena
            for (int i = 0; i < currentProductInfo.Length; i++)
            {
                Product.Destroy();
                //nistance--;

            }
        }

        productInfo = currentProductInfo;
        Array.Copy(currentProductInfo, productInfo, currentProductInfo.Length);
       

        /*GameObject[] targetArray = null;

         switch (n)
        {
            case 1:
                targetArray = _objectPositionshelve1;
                break;
            case 2:
                targetArray = _objectPositionshelve2;
                break;
            case 3:
                targetArray = _objectPositionshelve3;
                break;
            default:
                Debug.LogError("Indice n non valido");
                return;
        }
 */
        StoreGameObjectPositionsIntoSO();


        //generare vari prodotti
        for (int i = 0; i < currentProductInfo.Length; i++)
        {
            Product.Generate(currentProductInfo[i]);
            nistance++;
            //Product.GenerateBlock(productInfo.products[i], _objectPosition[i], 1, 1, 1, new Vector3(0,0,0));
            //GenerateMultipleIstance(productInfo.products[i]);
            //GenerateMultipleIstance(instance,productInfo.products[i]);

        }


    }

    
 

    void StoreGameObjectPositionsIntoSO()
    {
        if (posClass.Length > 0)// Controlla che ci siano GameObject nell'array
        {
            /*Usa reflection per accedere al campo _positions all'interno dell'_objectToChange (che è il ScriptableObject). Questo campo deve essere definito nel 
            ScriptableObject, altrimenti restituirà null.*/

            //System.Reflection.FieldInfo product = _objectToChange.GetType().GetField("product");
            //System.Reflection.FieldInfo _positions=_objectToChange.products.GetType().GetField("_positions");
            /*Inizializza un array storedPositions con una dimensione pari al numero di GameObject referenziati.*/
            int totProdotti = 0;
            for (int i = 0; i < posClass.Length; i++)
            {
                totProdotti = totProdotti + posClass[i]._objectPosition.Length;
            }


            storedPositions = new Vector3[totProdotti];
            /*Itera attraverso l'array di GameObject e memorizza la posizione di ciascuno di essi nell'array storedPositions.*/
            int k = 0;
            if (productInfo.Length < totProdotti)
            {
                Debug.Log("Prodotti terminati!");
                return;
            }
            for (int i = 0; i < posClass.Length; i++)
                for (int j = 0; j < posClass[i]._objectPosition.Length; ++j)
                {
                    {
                        storedPositions[k] = posClass[i]._objectPosition[j].transform.position;
                        productInfo[k].LabelPosition = posClass[i].LabelPosition;
                        productInfo[k].emptyPos=posClass[i]._objectPosition[j];
                        ++k;
                    }
                }
            /*Se il campo _positions esiste nel ScriptableObject, lo aggiorna con il valore di storedPositions usando reflection.
            Se _positions non esiste, viene stampato un messaggio di errore nella console.
            */
            for (int i = 0; i < productInfo.Length; i++)
            {
                productInfo[i]._positions = storedPositions[i];
                

            }

            /*if (_positions != null)
            {
                 _objectToChange.products.GetType().GetField("_positions").SetValue(_objectToChange, storedPositions);
             }
             else
             {
                 Debug.Log("_positions variable does not exist in this SO : " + _objectToChange.name);
             }*/
        }
    }


}