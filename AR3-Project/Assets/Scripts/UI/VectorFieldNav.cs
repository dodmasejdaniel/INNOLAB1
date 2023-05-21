using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VectorFieldNav : MonoBehaviour
{
    public GameObject ParticlePrefab;
    private GameObject spawnedParticleSystem;
    public float DistanceFromCamera = 1.0f;

    public void ResetParticleSystemPosition() {
        DestroyParticleSystem();

        // place new one
        GameObject arCam = GameObject.FindGameObjectWithTag("ar_cam");
        Vector3 arPos = arCam.transform.position + arCam.transform.forward * DistanceFromCamera;
        spawnedParticleSystem = Instantiate(ParticlePrefab, arPos, Quaternion.identity, arCam.transform);
        Debug.Log(spawnedParticleSystem);
    }

    public void fixPositionOfParticleSystem() {
        GameObject ParticleSystemParent = GameObject.FindGameObjectWithTag("particle-system-parent");
        if (ParticleSystemParent == null) {
            Debug.LogError("Parent not found");
            return;
        }

        Vector3 arPos = spawnedParticleSystem.transform.position;
        DestroyParticleSystem();
        spawnedParticleSystem = Instantiate(ParticlePrefab, arPos, Quaternion.identity, ParticleSystemParent.transform);
        
    }

    void DestroyParticleSystem() {
        if (spawnedParticleSystem != null) {
            Debug.Log("destroy");
            Destroy(spawnedParticleSystem);
        }
    }

    public void DestroyVisualizer() {

        GameObject VectorFieldParent = GameObject.FindGameObjectWithTag("vector-field-parent");
        if (VectorFieldParent == null) {
            Debug.LogError("Parent not found");
            return;
        }

        // check if VectorFieldParent has Children. If yes, destroy all.
        foreach (Transform child in VectorFieldParent.transform) {
            Destroy(child.gameObject);
        }

        GameObject VectorFieldVisualizer = GameObject.FindGameObjectWithTag("vector-field-visualizer");
        if (VectorFieldVisualizer == null) {
            Debug.LogError("Parent not found");
            return;
        }
        Destroy(VectorFieldVisualizer);
    }
}
