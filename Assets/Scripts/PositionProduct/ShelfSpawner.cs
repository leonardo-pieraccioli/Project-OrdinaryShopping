using UnityEngine;

[ExecuteAlways]
public class ShelfSpawner : MonoBehaviour
{
    public GameObject emptyPrefab; // Prefab dell'Empty
    public int rows = 4;
    public int columns = 2;
    public float spacingX = 0.5f;
    public float spacingY = 0.3f;

    private bool isInitialized = false;

    // Funzione che puoi eseguire manualmente
    [ContextMenu("Generate Spawn Points")]
    void GenerateSpawnPoints()
    {
        if (emptyPrefab == null)
        {
            Debug.LogError("emptyPrefab non è assegnato!");
            return;
        }

        // Controlla se gli Empty sono già stati generati
        if (isInitialized) 
        {
            Debug.LogWarning("Gli Empty sono già stati generati!");
            return;
        }

        // Crea i punti di spawn
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 spawnPos = transform.position + new Vector3(j * spacingX, i * spacingY, 0);
                
                // Instanzia l'oggetto senza parent
                GameObject empty = Instantiate(emptyPrefab, spawnPos, Quaternion.identity);
                
                // Assegna il parent dopo l'instanziazione
                empty.transform.SetParent(transform, true); // Il "true" mantiene la posizione mondiale
                //empty.name = $"SpawnPoint_{i}_{j}";
                empty.name = gameObject.name+$"_{i}_{j}";
            }
        }

        isInitialized = true;
        Debug.Log("Punti di spawn generati!");
    }

    // Funzione per rimuovere gli Empty esistenti
    [ContextMenu("Clear Spawn Points")]
    void ClearSpawnPoints()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        isInitialized = false;
        Debug.Log("Punti di spawn cancellati!");
    }

    // Funzione per eseguire manualmente la generazione
    [ContextMenu("Generate or Clear Spawn Points")]
    void ToggleSpawnPoints()
    {
        if (isInitialized)
        {
            ClearSpawnPoints();
        }
        else
        {
            GenerateSpawnPoints();
        }
    }
}
