#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DirectionHandles : MonoBehaviour
{
    public float labelDistance = 2f; // Distanza delle etichette

    private void OnDrawGizmos()
    {
        // Definisci uno stile per le label
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black; // Imposta il colore del testo a nero
        style.fontSize = 20;                  // Imposta la dimensione del font (opzionale)
        style.richText = true;                // Abilita il rich text se necessario

        Vector3 pos = transform.position;

        // Disegna le etichette nelle 4 direzioni cardinali con lo stile definito
        Handles.Label(pos + Vector3.forward * labelDistance, "North (N)", style);
        Handles.Label(pos - Vector3.forward * labelDistance, "South (S)", style);
        Handles.Label(pos + Vector3.right * labelDistance, "East (E)", style);
        Handles.Label(pos - Vector3.right * labelDistance, "West (W)", style);
    }
}
#endif
