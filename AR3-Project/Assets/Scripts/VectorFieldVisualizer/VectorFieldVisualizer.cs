using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFieldVisualizer : MonoBehaviour {
    public int gridSize = 10;
    public float spacing = 1.0f;
    public float arrowLength = 0.5f;
    public Color arrowColor = Color.white;
    public GameObject arrowPrefab;

    public delegate Vector3 VectorFieldFunctionDelegate(Vector3 position);
    public event VectorFieldFunctionDelegate OnCustomVectorFieldFunction;

    private void Start() {

        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                for (int z = 0; z < gridSize; z++) {
                    Vector3 position = new Vector3(x * spacing, y * spacing, z * spacing);
                    Vector3 vector = VectorFieldFunction(position);
                    SpawnArrow(position, vector.normalized * arrowLength);
                }
            }
        }
    }

    private Vector3 VectorFieldFunction(Vector3 position) {
        if (OnCustomVectorFieldFunction != null) {
            return OnCustomVectorFieldFunction(position);
        }

        // Fallback to the default function if no custom function is provided
        return new Vector3(
            -Mathf.Sin(position.z),
            Mathf.Sin(position.x),
            Mathf.Sin(position.y)
        );
    }

    private void SpawnArrow(Vector3 position, Vector3 direction) {
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.LookRotation(direction));
        arrow.transform.localScale = new Vector3(1, 1, direction.magnitude);
    }
}
