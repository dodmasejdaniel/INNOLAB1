using UnityEngine;

public class CustomVectorFieldAndParticleFunction : MonoBehaviour {
    public VectorFieldVisualizer vectorFieldVisualizer;
    public ParticleFieldController particleFieldController;

    private void Start() {
        vectorFieldVisualizer.OnCustomVectorFieldFunction += CurlNoiseVectorField;
        particleFieldController.OnCustomParticleFieldFunction += CurlNoiseVectorField;
    }

    private void OnDestroy() {
        vectorFieldVisualizer.OnCustomVectorFieldFunction -= CurlNoiseVectorField;
        particleFieldController.OnCustomParticleFieldFunction -= CurlNoiseVectorField;
    }

    private Vector3 MyCustomFunction(Vector3 position) {
        
        return new Vector3(
            Mathf.Cos(position.x),
            Mathf.Cos(position.y),
            Mathf.Cos(position.z)
        );
    }

    private Vector3 CircularVectorField(Vector3 position) {

        return new Vector3(
            -position.z,
            0,
            position.x
        ).normalized;
    }

    private Vector3 CurlNoiseVectorField(Vector3 position) {

        float noiseX = (Mathf.PerlinNoise(position.y, position.z) - 0.5f) * 2f;
        float noiseY = (Mathf.PerlinNoise(position.z, position.x) - 0.5f) * 2f;
        float noiseZ = (Mathf.PerlinNoise(position.x, position.y) - 0.5f) * 2f;

        return new Vector3(noiseX, noiseY, noiseZ).normalized;
    }

    private Vector3 SphericalHarmonicsVectorField(Vector3 position) {

        float r = position.magnitude;
        float theta = Mathf.Acos(position.y / r);
        float phi = Mathf.Atan2(position.z, position.x);

        float Y00 = 0.5f * Mathf.Sqrt(1f / Mathf.PI);
        float Y10 = Mathf.Sqrt(3f / (4f * Mathf.PI)) * Mathf.Cos(theta);
        float Y11 = Mathf.Sqrt(3f / (4f * Mathf.PI)) * Mathf.Sin(theta) * Mathf.Cos(phi);
        float Y1_1 = Mathf.Sqrt(3f / (4f * Mathf.PI)) * Mathf.Sin(theta) * Mathf.Sin(phi);

        return new Vector3(Y10, Y11, Y1_1).normalized;
    }

    private Vector3 TorodialVectorVectorField(Vector3 position) {
        return new Vector3(
            -position.z * (1f + position.magnitude),
            position.y * (1f + position.magnitude),
            position.x * (1f + position.magnitude)
        ).normalized;
    }
}
