using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartesianMenu : MonoBehaviour {
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
        Debug.Log("Start");
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
        Vector3 targetPosition = vector;
        Vector3 arrowStartPosition = new Vector3(0, 0, 0);

        // Direction vector from start to target
        Vector3 direction = targetPosition - arrowStartPosition;

        // Adjust the rotation of the arrow to look at the target
        Quaternion rotation = Quaternion.LookRotation(direction);
        arrowInstance.transform.rotation = rotation;

        // Adjust the scale to match the distance (you might need to adjust the scaling factor)
        float distance = Vector3.Distance(arrowStartPosition, targetPosition);
        arrowInstance.transform.localScale = new Vector3(1, 1, distance);
    }

    Vector3 GetTargetPosition() {
        GameObject arrowInstance = canvas.GetComponent<VectorNav>().getActualArrow();
        Quaternion rotation = arrowInstance.transform.rotation;
        float distance = arrowInstance.transform.localScale.z;  // Assuming the distance is stored in the z component of localScale

        // Calculate the direction vector based on rotation
        Vector3 direction = rotation * Vector3.forward;

        // Find the target position by scaling the direction vector
        Vector3 targetPosition = direction * distance;

        return targetPosition;
    }


}