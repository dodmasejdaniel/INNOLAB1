using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFieldController : MonoBehaviour {
    public float particleForce = 1.0f;

    //public GameObject ParticlePrefab;
    private ParticleSystem particleSystemV;
    private ParticleSystem.Particle[] particles;

    public event Common.VectorFunction OnCustomParticleFieldFunction;

    public void SetCustomVectorFieldFunction(Common.VectorFunction function) {
        OnCustomParticleFieldFunction = function;
    }

    // Awake, Start, Update, LateUpdate: https://docs.unity3d.com/Manual/ExecutionOrder.html
    // Get and set particle system (flying/moving dots)
    private void Awake() {
        particleSystemV = gameObject.GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleSystemV.main.maxParticles];
    }

    // Set vector-field function (3x cos,..)
    private void Start() {
        if (CustomVectorFieldAndParticleFunction.Instance != null)
            OnCustomParticleFieldFunction = CustomVectorFieldAndParticleFunction.Instance.currentFunction;
    }

    // Set vector-field function (3x cos,..) in each frame
    private void Update() {
        if (CustomVectorFieldAndParticleFunction.Instance != null)
            OnCustomParticleFieldFunction = CustomVectorFieldAndParticleFunction.Instance.currentFunction;
    }

    // Update position of particles, depending on the custom vector-field function
    private void LateUpdate() {
        //InitializeIfNeeded();

        int numParticlesAlive = particleSystemV.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++) {
            Vector3 particlePosition = particles[i].position;
            Vector3 vector = VectorFieldFunction(particlePosition);

            particles[i].velocity = vector * particleForce;
        }

        particleSystemV.SetParticles(particles, numParticlesAlive);
    }

    // Wrapper around the custom function for safety reasons (if no vector-field function was found)
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

    public void RestartParticleSystem() {
        particleSystemV.Clear();
        particleSystemV.Play(); 
    }

    //private void InitializeIfNeeded() {
    //    if (particleSystemV == null) {
    //        GameObject arCam = GameObject.FindGameObjectWithTag("ar_cam");
    //        Vector3 arPos = new Vector3(arCam.transform.position.x, arCam.transform.position.y - 0.5f, arCam.transform.position.z + 0.5f);
    //        GameObject particleSystem = Instantiate(ParticlePrefab, arPos, Quaternion.identity, arCam.transform);
    //        particleSystemV = particleSystem.GetComponent<ParticleSystem>();
    //    }

    //    if (particles == null || particles.Length < particleSystemV.main.maxParticles) {
    //        particles = new ParticleSystem.Particle[particleSystemV.main.maxParticles];
    //    }
    //}
}
