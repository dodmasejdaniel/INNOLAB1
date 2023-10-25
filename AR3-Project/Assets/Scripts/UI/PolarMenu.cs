using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PolarMenu : MonoBehaviour {
    [SerializeField] private TMP_InputField inputX;
    [SerializeField] private TMP_InputField inputY;
    [SerializeField] private TMP_InputField inputZ;

    [SerializeField] private Slider sliderX;
    [SerializeField] private Slider sliderY;
    [SerializeField] private Slider sliderZ;

    //public GameObject arrowPrefab;
    //private GameObject arrowInstance;
    //private GameObject coordSystem;
    private GameObject canvas;

    private Vector3 vector = Vector3.zero; // The current vector

    private void Start() {
        inputX.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'x'));
        inputY.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'y'));
        inputZ.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'z'));

        sliderX.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'x'));
        sliderY.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'y'));
        sliderZ.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'z'));

        //coordSystem = GameObject.FindGameObjectWithTag("coord-system");
        canvas = GameObject.FindGameObjectWithTag("canvas");
        //arrowInstance = canvas.GetComponent<VectorNav>().getIsArrowB() ? canvas.GetComponent<VectorNav>().getArrowB() : canvas.GetComponent<VectorNav>().getArrowA();

    }

    private void OnEnable() {
        if (canvas == null) {
            canvas = GameObject.FindGameObjectWithTag("canvas");
        }
        try {
            Vector3 targetPosition = GetTargetPosition();

            Debug.Log(targetPosition);
            // Updating input fields based on the arrow's current position
            inputX.text = targetPosition.x.ToString("F2");
            inputY.text = targetPosition.y.ToString("F2");
            inputZ.text = targetPosition.z.ToString("F2");

            // Updating sliders based on the arrow's current position
            sliderX.value = targetPosition.x;
            sliderY.value = targetPosition.y;
            sliderZ.value = targetPosition.z;

            // Update the vector, just to keep things in sync
            vector = targetPosition;
        } catch {
            Debug.Log("No arrow instance found.");
        }
    }

    private void OnChangeInputAxis(string value, char axis) {
        //float floatVal = Mathf.Clamp( ConvertToFloat(value), -5.0f, 5.0f);
        float floatVal = ConvertToFloat(value);
        switch (axis) {
            case 'x':
                sliderX.value = floatVal;
                break;
            case 'y':
                sliderY.value = floatVal;
                break;
            case 'z':
                sliderZ.value = floatVal;
                break;
        }
        OnChange(axis, floatVal);
    }

    private void OnChangeSliderAxis(float value, char axis) {
        switch (axis) {
            case 'x':
                inputX.text = value.ToString("F2");
                break;
            case 'y':
                inputY.text = value.ToString("F2");
                break;
            case 'z':
                inputZ.text = value.ToString("F2");
                break;
        }
        OnChange(axis, value);
    }

    private void OnChange(char axis, float newValue) {
        GameObject arrowInstance = canvas.GetComponent<VectorNav>().getActualArrow();
        if (arrowInstance == null) {
            Debug.LogError("Arrow instance is not instantiated");
            return;
        }

        // Update the relevant component of the vector
        switch (axis) {
            case 'x':
                vector.x = newValue;
                ChangeArrow();
                break;
            case 'y':
                vector.y = newValue;
                ChangeArrow();
                break;
            case 'z':
                vector.z = newValue;
                ChangeArrow();
                break;
            default:
                Debug.LogError("Invalid axis: " + axis);
                return;
        }

    }

    private float ConvertToFloat(string input) {
        float newValue;
        if (!float.TryParse(input, out newValue)) {
            newValue = 0.0f;
        }
        return newValue;
    }

    void ChangeArrow() {
        GameObject arrowInstance = canvas.GetComponent<VectorNav>().getActualArrow();

        // Convert spherical (r, theta, phi) to Cartesian (x, y, z)
        float r = vector.x;  // Radius
        float phi = Mathf.Deg2Rad * vector.y;  // Convert degrees to radians for polar-angle
        float theta = Mathf.Deg2Rad * vector.z;  // Convert degrees to radians for azimuthal-angle

        // Cartesian coordinates
        Vector3 direction = new Vector3(
            r * Mathf.Sin(phi) * Mathf.Cos(theta),
            r * Mathf.Sin(phi) * Mathf.Sin(theta),
            r * Mathf.Cos(phi)
        );

        Quaternion rotation = Quaternion.LookRotation(direction);

        // Hardcoded -90 degree rotation on the x-axis for unitys coordinate-system correction
        Quaternion xRotation = Quaternion.Euler(-90, 0, 0);

        arrowInstance.transform.rotation = xRotation * rotation;

        arrowInstance.transform.localScale = new Vector3(1, 1, r);
    }

    Vector3 GetTargetPosition() {
        GameObject arrowInstance = canvas.GetComponent<VectorNav>().getActualArrow();

        // Get r from local scale
        float r = arrowInstance.transform.localScale.z;

        // Get the direction from the rotation
        Vector3 direction = arrowInstance.transform.rotation * Vector3.forward;

        // Correct for the -90 degree rotation on the x-axis
        Quaternion xInverseRotation = Quaternion.Euler(90, 0, 0);
        direction = xInverseRotation * direction;

        // Convert to spherical coordinates
        float phi = Mathf.Acos(direction.z / r); // polar angle in radians
        float theta = Mathf.Atan2(direction.y, direction.x); // azimuthal angle in radians

        // Convert to degrees
        phi = Mathf.Rad2Deg * phi;
        theta = Mathf.Rad2Deg * theta;

        return new Vector3(r, phi, theta);
    }


}