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
        UpdateDotCrossProduct();
    }

    // Called when the script is enabled => `SetActive(true)`
    private void OnEnable() {
        SetInputBasedOnArrow();
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
            eventScript.TriggerEvent(PolarToCartesian(vector.x, vector.y, vector.z), canvas.GetComponent<VectorNav>().GetIsArrowB(), canvas.GetComponent<VectorNav>().GetIsPolar());
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

    // Update the arrow's rotation and scale based on the current vector
    void ChangeArrow() {
        UpdateDotCrossProduct();

        GameObject arrowInstance = canvas.GetComponent<VectorNav>().GetActualArrow();

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

    // Reverse of ChangeArrow(): Calculate the target position based on arrow's rotation and scale
    Vector3 GetTargetPosition() {
        GameObject arrowInstance = canvas.GetComponent<VectorNav>().GetActualArrow();

        // Get r from local scale
        float r = arrowInstance.transform.localScale.z;

        // Get the direction from the rotation
        Vector3 direction = arrowInstance.transform.rotation * Vector3.forward;

        // Correct for the -90 degree rotation on the x-axis
        Quaternion xInverseRotation =  Quaternion.Euler(90, 0, 0);
        direction = xInverseRotation * direction;

        // Convert to spherical coordinates
        float phi = Mathf.Acos(direction.z / r); // polar angle in radians
        float theta = Mathf.Atan2(direction.y, direction.x); // azimuthal angle in radians

        // Convert to degrees
        phi = Mathf.Rad2Deg * phi;
        theta = Mathf.Rad2Deg * theta;

        return new Vector3(r, phi % 360, (theta + 360) % 360);
    }

    public Vector3 PolarToCartesian(float radius, float theta, float phi) {
        // Theta und Phi sind in Grad, also Umwandlung in Radiant
        float thetaRad = Mathf.Deg2Rad * theta;
        float phiRad = Mathf.Deg2Rad * phi;

        float x = radius * Mathf.Sin(phiRad) * Mathf.Cos(thetaRad);
        float y = radius * Mathf.Sin(phiRad) * Mathf.Sin(thetaRad);
        float z = radius * Mathf.Cos(phiRad);
        Debug.Log(x + " " + y + " " + z);
        return new Vector3(x, y, z);
    }
}