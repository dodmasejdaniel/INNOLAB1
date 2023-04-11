using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFieldController : MonoBehaviour
{
    public float particleForce = 1.0f;

    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;

    private void Start() {
        particleSystem = GetComponent<ParticleSystem>();
        
    }

    private void LateUpdate() {
        InitializeIfNeeded();

        int numParticlesAlive = particleSystem.GetParticles(particles);
        

        for (int i = 0; i < numParticlesAlive; i++) {
            Vector3 particlePosition = particles[i].position;
            Vector3 vector = VectorFieldFunction(particlePosition);

            particles[i].velocity = vector * particleForce;
        }

        particleSystem.SetParticles(particles, numParticlesAlive);
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

    private void InitializeIfNeeded() {
        if (particleSystem == null) {
            particleSystem = GetComponent<ParticleSystem>();
        }

        if (particles == null || particles.Length < particleSystem.main.maxParticles) {
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }
    }
}
