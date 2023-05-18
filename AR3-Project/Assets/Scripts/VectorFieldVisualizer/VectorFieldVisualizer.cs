using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFieldVisualizer : MonoBehaviour {

    readonly float spacing = 0.1f;
    Vector3 arPos;
    GameObject VectorFieldParent;

    public int gridSize = 10;
    public float arrowLength = 0.5f;
    public Color arrowColor = Color.white;
    public GameObject arrowPrefab;

    
    public event Common.VectorFunction OnCustomVectorFieldFunction;

    public void SetCustomVectorFieldFunction(Common.VectorFunction function) {
        OnCustomVectorFieldFunction = function;
    }

    public void ResetVectorField(Common.VectorFunction function) {
        SetCustomVectorFieldFunction(function);

        VectorFieldParent = GameObject.FindGameObjectWithTag("vector-field-parent");
        if (VectorFieldParent == null) {
            Debug.LogError("Parent not found");
            return;
        }

        // check if VectorFieldParent has Children. If yes, destroy all.
        foreach (Transform child in VectorFieldParent.transform) {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                for (int z = 0; z < gridSize; z++) {
                    Vector3 position = new Vector3(x * spacing + arPos.x, y * spacing + arPos.y, z * spacing + arPos.z);
                    Vector3 vector = VectorFieldFunction(position);
                    SpawnArrow(position, vector.normalized * arrowLength, VectorFieldParent.transform);

                }
            }
        }
    }

    private void Start() {

        arPos = gameObject.transform.position;
        ResetVectorField(CustomVectorFieldAndParticleFunction.Instance.currentFunction);
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

    private void SpawnArrow(Vector3 position, Vector3 direction, Transform vectorFieldParent) {
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.LookRotation(direction), vectorFieldParent);
        arrow.transform.localScale = new Vector3(1, 1, direction.magnitude);
    }
}
