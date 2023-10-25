//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class CartesianMenu : MonoBehaviour {
//    [SerializeField] private TMP_InputField inputX;
//    [SerializeField] private TMP_InputField inputY;
//    [SerializeField] private TMP_InputField inputZ;

//    [SerializeField] private Slider sliderX;
//    [SerializeField] private Slider sliderY;
//    [SerializeField] private Slider sliderZ;

//    public GameObject arrowPrefab;
//    private GameObject arrowInstance;
//    private GameObject coordSystem;
//    private GameObject canvas;

//    private Vector3 vector = Vector3.zero; // The current vector

//    private void Start() {
//        inputX.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'x'));
//        inputY.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'y'));
//        inputZ.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'z'));

//        sliderX.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'x'));
//        sliderY.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'y'));
//        sliderZ.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'z'));

//        coordSystem = GameObject.FindGameObjectWithTag("coord-system");
//        canvas = GameObject.FindGameObjectWithTag("canvas");
//        arrowInstance = canvas.GetComponent<VectorNav>().getIsArrowB() ? canvas.GetComponent<VectorNav>().getArrowB() : canvas.GetComponent<VectorNav>().getArrowA();

//    }

//    private void OnChangeInputAxis(string value, char axis) {
//        //float floatVal = Mathf.Clamp( ConvertToFloat(value), -5.0f, 5.0f);
//        float floatVal = ConvertToFloat(value);
//        switch (axis) {
//            case 'x':
//                sliderX.value = floatVal;
//                break;
//            case 'y':
//                sliderY.value = floatVal;
//                break;
//            case 'z':
//                sliderZ.value = floatVal;
//                break;
//        }
//        OnChange(axis, floatVal);
//    }

//    private void OnChangeSliderAxis(float value, char axis) {
//        switch (axis) {
//            case 'x':
//                inputX.text = value.ToString("F2");
//                break;
//            case 'y':
//                inputY.text = value.ToString("F2");
//                break;
//            case 'z':
//                inputZ.text = value.ToString("F2");
//                break;
//        }
//        OnChange(axis, value);
//    }

//    private void OnChange(char axis, float newValue) {
//        if (arrowInstance == null) {
//            Debug.LogError("Arrow instance is not instantiated");
//            return;
//        }

//        // Update the relevant component of the vector
//        switch (axis) {
//            case 'x':
//                vector.z = newValue;
//                ChangedXZ();
//                break;
//            case 'y':
//                vector.y = newValue;
//                ChangedY();
//                break;
//            case 'z':
//                vector.x = newValue * -1.0f;
//                ChangedXZ();
//                break;
//            default:
//                Debug.LogError("Invalid axis: " + axis);
//                return;
//        }

//        // Calculate the new magnitude and direction
//        float magnitude = vector.magnitude;
//        // Scale the arrow's length to match the new magnitude
//        arrowInstance.transform.localScale = new Vector3(magnitude, arrowInstance.transform.localScale.y, arrowInstance.transform.localScale.z);
//    }

//    private float ConvertToFloat(string input) {
//        float newValue;
//        if (!float.TryParse(input, out newValue)) {
//            newValue = 0.0f;
//        }
//        return newValue;
//    }


//    void ChangedY() {
//        // Calculate the y angle and set it to the Z rotation of arrowInstance

//        float angleZ = Mathf.Atan2(vector.y, Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z)) * Mathf.Rad2Deg;
//        //arrowInstance.transform.rotation = Quaternion.Euler(arrowInstance.transform.rotation.eulerAngles.x, arrowInstance.transform.rotation.eulerAngles.y, angleZ);
//        arrowInstance.transform.localRotation = Quaternion.Euler(arrowInstance.transform.localRotation.eulerAngles.x, arrowInstance.transform.localRotation.eulerAngles.y, angleZ);

//    }

//    void ChangedXZ() {

//        Vector3 direction = vector.normalized;
//        Debug.Log(direction);
//        Debug.Log(Quaternion.LookRotation(direction));
//        // Rotate the arrow to point in the new direction (converting direction to euler angles)
//        arrowInstance.transform.localRotation = Quaternion.LookRotation(direction);
//    }
//}