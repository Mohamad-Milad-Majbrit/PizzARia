using UnityEngine;
using UnityEngine.XR.ARFoundation; // Wichtig für AR
using UnityEngine.XR.ARSubsystems; // Wichtig für TrackingStates
using System.Collections.Generic;

public class ImageTracker : MonoBehaviour
{
    // In Unity 6 nutzen wir immer noch den Manager
    [SerializeField]
    private ARTrackedImageManager arTrackedImageManager;

    [SerializeField]
    private GameObject pizzaPrefab;

    // Dictionary zum Speichern der erzeugten Pizzen
    private Dictionary<string, GameObject> spawnedPizzas = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        // Event abonnieren
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        // Event deabonnieren (Wichtig um Fehler zu vermeiden beim Szenenwechsel)
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 1. NEUE MARKER ERKANNT
        foreach (var trackedImage in eventArgs.added)
        {
            UpdatePizza(trackedImage);
        }

        // 2. MARKER AKTUALISIERT (Bewegung oder Tracking-Status Änderung)
        // In Unity 6 ist es gute Praxis, auch "updated" zu prüfen, 
        // falls das Objekt mal ausgeblendet wurde.
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdatePizza(trackedImage);
        }

        // 3. MARKER VERLOREN (aus dem Speicher entfernt)
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedPizzas.TryGetValue(trackedImage.referenceImage.name, out GameObject pizzaInstance))
            {
                Destroy(pizzaInstance);
                spawnedPizzas.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void UpdatePizza(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        // Prüfen, ob wir für dieses Bild schon eine Pizza haben
        if (!spawnedPizzas.ContainsKey(imageName))
        {
            // Neue Pizza erstellen
            // Wir setzen das 'trackedImage.transform' als Parent. 
            // Dadurch bewegt sich die Pizza automatisch mit dem Marker mit.
            GameObject newPizza = Instantiate(pizzaPrefab, trackedImage.transform);

            // Optional: Position/Rotation auf 0 setzen, damit sie exakt auf dem Marker sitzt
            newPizza.transform.localPosition = Vector3.zero;
            newPizza.transform.localRotation = Quaternion.identity;

            spawnedPizzas.Add(imageName, newPizza);
        }
        else
        {
            // Wenn es die Pizza schon gibt, stellen wir sicher, dass sie sichtbar ist
            // (Manchmal blendet ARFoundation Objekte aus, wenn das Tracking schlecht ist)
            GameObject existingPizza = spawnedPizzas[imageName];

            // Zeige Pizza nur, wenn der Marker auch wirklich getrackt wird
            bool isTracked = trackedImage.trackingState == TrackingState.Tracking;
            existingPizza.SetActive(isTracked);
        }
    }
}