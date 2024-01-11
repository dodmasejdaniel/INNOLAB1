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

    private GameObject canvas;

    private Vector3 vector = Vector3.zero; // The current vector

    private DotCrossProductEvent eventScript;

    // Called when the object becomes active; used for initialization
    private void Start() {

        // Listen for changes in Input Fields and Sliders for each axis
        // Call the corresponding function when value changes
        inputX.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'x'));
        inputY.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'y'));
        inputZ.onValueChanged.AddListener(val => OnChangeInputAxis(val, 'z'));

        sliderX.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'x'));
        sliderY.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'y'));
        sliderZ.onValueChanged.AddListener(val => OnChangeSliderAxis(val, 'z'));

        eventScript = FindObjectOfType<DotCrossProductEvent>();
    }

    // Called when the script is enabled => `SetActive(true)`
    private void OnEnable() {
        SetInputBasedOnArrow();
        UpdateDotCrossProduct();
    }

    // Sets input fields to the arrow's position
    public void SetInputBasedOnArrow() {

        // Initialize canvas if it's null
        if (canvas == null) {
            canvas = GameObject.FindGameObjectWithTag("canvas");
        }
        try {
            Vector3 targetPosition = GetTargetPosition();

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

    // Called when an Input Field changes its value
    private void OnChangeInputAxis(string value, char axis) {

        // Convert input to float and update corresponding slider
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

        UpdateArrow(axis, floatVal);
    }

    // Called when a Slider changes its value
    private void OnChangeSliderAxis(float value, char axis) {

        // Update the corresponding input field
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

        UpdateArrow(axis, value);
    }

    // Update arrow's position based on new values for x, y or z axis
    private void UpdateArrow(char axis, float newValue) {

        // Get current arrow instance and check if it's null
        GameObject arrowInstance = canvas.GetComponent<VectorNav>().GetActualArrow();
        if (arrowInstance == null) {
            Debug.LogError("Arrow instance is not instantiated");
            return;
        }

        // Update the corresponding axis in the internal vector
        // Call ChangeArrow to reflect these updates on the arrow
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

    // Trigger event to update product
    public void UpdateDotCrossProduct() {
        if (eventScript != null) {
            eventScript.TriggerEvent(vector, canvas.GetComponent<VectorNav>().GetIsArrowB(), canvas.GetComponent<VectorNav>().GetIsPolar());
        }
    }

    // Utility function to convert string to float
    // Try parsing the float value; default to 0.0 if unsuccessful
    private float ConvertToFloat(string input) {
        float newValue;
        if (!float.TryParse(input, out newValue)) {
            newValue = 0.0f;
        }
        return newValue;
    }

    // Change the arrow's rotation and scale to point it towards the target position
    void ChangeArrow() {
        UpdateDotCrossProduct();

        GameObject arrowInstance = canvas.GetComponent<VectorNav>().GetActualArrow();
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

    // Reverse of ChangeArrow(): Calculate the target position based on arrow's rotation and scale
    Vector3 GetTargetPosition() {
        GameObject arrowInstance = canvas.GetComponent<VectorNav>().GetActualArrow();
        Quaternion rotation = arrowInstance.transform.rotation;
        float distance = arrowInstance.transform.localScale.z;

        // Calculate the direction vector based on rotation
        Vector3 direction = rotation * Vector3.forward;

        // Find the target position by scaling the direction vector
        Vector3 targetPosition = direction * distance;

        return targetPosition;
    }


}