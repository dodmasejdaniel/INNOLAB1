using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VectorFieldNav : MonoBehaviour
{
    public GameObject ParticlePrefab;
    private GameObject spawnedParticleSystem;
    public float DistanceFromCamera = 1.0f;

    // BUTTONCLICK: Bottom Button "RESET". Resets the position of the green particle emitter to in front of the AR camera at DisctanceFromCamera.
    public void ResetParticleSystemPosition() {
        DestroyParticleSystem();
        // place new one
        GameObject arCam = GameObject.FindGameObjectWithTag("ar_cam");
        Vector3 arPos = arCam.transform.position + arCam.transform.forward * DistanceFromCamera;
        spawnedParticleSystem = Instantiate(ParticlePrefab, arPos, Quaternion.identity, arCam.transform);
    }

    // BUTTONCLICK: Bottom Button "FIX". Fixes the position of the green particle emitter to the place in 3D World Space.
    public void FixPositionOfParticleSystem() {
        GameObject ParticleSystemParent = GameObject.FindGameObjectWithTag("particle-system-parent");
        if (ParticleSystemParent == null) {
            Debug.LogError("Parent not found");
            return;
        }

        Vector3 arPos = spawnedParticleSystem.transform.position;
        DestroyParticleSystem();
        spawnedParticleSystem = Instantiate(ParticlePrefab, arPos, Quaternion.identity, ParticleSystemParent.transform);
    }

    // Helper to destroy green particle emitter.
    void DestroyParticleSystem() {
        if (spawnedParticleSystem != null) {
            Debug.Log("destroy");
            Destroy(spawnedParticleSystem);
        }
    }

    // Helper to destroy 10x10x10 arrow-cube, which visualizes the vector-field
    public void DestroyVisualizer() {

        GameObject VectorFieldParent = GameObject.FindGameObjectWithTag("vector-field-parent");
        if (VectorFieldParent == null) {
            Debug.LogError("Parent not found");
            return;
        }

        // check if VectorFieldParent has children (arrows). If yes, destroy all.
        foreach (Transform child in VectorFieldParent.transform) {
            Destroy(child.gameObject);
        }

        GameObject VectorFieldVisualizer = GameObject.FindGameObjectWithTag("vector-field-visualizer");
        if (VectorFieldVisualizer == null) {
            Debug.LogError("Visualizer not found");
            return;
        }
        Destroy(VectorFieldVisualizer);
    }
}
