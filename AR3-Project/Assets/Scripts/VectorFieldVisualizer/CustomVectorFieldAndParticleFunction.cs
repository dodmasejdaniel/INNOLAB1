using UnityEngine;
using TMPro;

public class CustomVectorFieldAndParticleFunction : MonoBehaviour {
    public static CustomVectorFieldAndParticleFunction Instance;

    private TMP_Dropdown functionDropdown;

    private enum FunctionType {
        MyCustomFunction,
        CircularVectorField,
        CurlNoiseVectorField,
        SphericalHarmonicsVectorField,
        TorodialVectorVectorField
    }

    public Common.VectorFunction currentFunction;  // made public so VectorFieldVisualizer and ParticleFieldController can access

    private void Awake() {
        // Here we handle the singleton pattern
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        functionDropdown = GameObject.FindGameObjectWithTag("dropdown").GetComponent<TMP_Dropdown>();

        // Set Listener to dropdown changes
        functionDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(functionDropdown); });
        DropdownValueChanged(functionDropdown);
    }

    // Dropdown changes the used vector-field function
    private void DropdownValueChanged(TMP_Dropdown change) {
        FunctionType functionType = (FunctionType)change.value;

        switch (functionType) {
            case FunctionType.MyCustomFunction:
                currentFunction = MyCustomFunction;
                break;
            case FunctionType.CircularVectorField:
                currentFunction = CircularVectorField;
                break;
            case FunctionType.CurlNoiseVectorField:
                currentFunction = CurlNoiseVectorField;
                break;
            case FunctionType.SphericalHarmonicsVectorField:
                currentFunction = SphericalHarmonicsVectorField;
                break;
            case FunctionType.TorodialVectorVectorField:
                currentFunction = TorodialVectorVectorField;
                break;
        }

        GameObject visualizer = GameObject.FindGameObjectWithTag("vector-field-visualizer");
        if (visualizer != null) {
            visualizer.GetComponent<VectorFieldVisualizer>().ResetVectorField(currentFunction);
        }
    }

    // Basic 3x cos
    private Vector3 MyCustomFunction(Vector3 position) {
        
        return new Vector3(
            Mathf.Cos(position.x),
            Mathf.Cos(position.y),
            Mathf.Cos(position.z)
        );
    }

    // Spinnin'
    private Vector3 CircularVectorField(Vector3 position) {

        return new Vector3(
            -position.z,
            0,
            position.x
        ).normalized;
    }

    // Curl Noise 
    private Vector3 CurlNoiseVectorField(Vector3 position) {

        float noiseX = (Mathf.PerlinNoise(position.y, position.z) - 0.5f) * 2f;
        float noiseY = (Mathf.PerlinNoise(position.z, position.x) - 0.5f) * 2f;
        float noiseZ = (Mathf.PerlinNoise(position.x, position.y) - 0.5f) * 2f;

        return new Vector3(noiseX, noiseY, noiseZ).normalized;
    }

    // Spherical harmonics
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

    // Torodial Vector
    private Vector3 TorodialVectorVectorField(Vector3 position) {
        return new Vector3(
            -position.z * (1f + position.magnitude),
            position.y * (1f + position.magnitude),
            position.x * (1f + position.magnitude)
        ).normalized;
    }
}
