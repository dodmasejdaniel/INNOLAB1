using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFieldVisualizer : MonoBehaviour {
    public int gridSize = 10;
    public float spacing = 1.0f;
    public float arrowLength = 0.5f;
    public Color arrowColor = Color.white;

    private void OnDrawGizmos() {
        Gizmos.color = arrowColor;

        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                for (int z = 0; z < gridSize; z++) {
                    Vector3 position = new Vector3(x * spacing, y * spacing, z * spacing);
                    Vector3 vector = VectorFieldFunction(position);
                    DrawArrow(position, vector.normalized * arrowLength);
                }
            }
        }
    }

    private Vector3 VectorFieldFunction(Vector3 position) {
        // Define your vector field function here.
        // Example: Simple curl noise
        return new Vector3(
            -Mathf.Sin(position.z),
            Mathf.Sin(position.x),
            Mathf.Sin(position.y)
        );
    }

    private void DrawArrow(Vector3 start, Vector3 direction) {
        Gizmos.DrawLine(start, start + direction);
    }
}
