using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFieldController : MonoBehaviour {
    public float particleForce = 1.0f;

    public GameObject ParticlePrefab;
    private ParticleSystem particleSystemV;
    private ParticleSystem.Particle[] particles;

    public delegate Vector3 ParticleFieldFunctionDelegate(Vector3 position);
    public event ParticleFieldFunctionDelegate OnCustomParticleFieldFunction;

    private void Awake() {
        InitializeIfNeeded();
        
    }

    private void LateUpdate() {
        InitializeIfNeeded();

        int numParticlesAlive = particleSystemV.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++) {
            Vector3 particlePosition = particles[i].position;
            Vector3 vector = VectorFieldFunction(particlePosition);

            particles[i].velocity = vector * particleForce;
        }

        particleSystemV.SetParticles(particles, numParticlesAlive);
    }

    private Vector3 VectorFieldFunction(Vector3 position) {
        if (OnCustomParticleFieldFunction != null) {
            return OnCustomParticleFieldFunction(position);
        }

        // Fallback to the default function if no custom function is provided
        return new Vector3(
            -Mathf.Sin(position.z),
            Mathf.Sin(position.x),
            Mathf.Sin(position.y)
        );
    }

    private void InitializeIfNeeded() {
        if (particleSystemV == null) {
            GameObject arCam = GameObject.FindGameObjectWithTag("ar_cam");
            Debug.Log(arCam);
            GameObject particleSystem = Instantiate(ParticlePrefab, Vector3.zero, Quaternion.identity, arCam.transform);
            particleSystemV = particleSystem.GetComponent<ParticleSystem>();
        }

        if (particles == null || particles.Length < particleSystemV.main.maxParticles) {
            particles = new ParticleSystem.Particle[particleSystemV.main.maxParticles];
        }
    }
}
