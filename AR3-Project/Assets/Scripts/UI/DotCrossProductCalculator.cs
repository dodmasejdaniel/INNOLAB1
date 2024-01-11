using UnityEngine;
using TMPro;

public class DotCrossProductCalculator : MonoBehaviour {

    private Vector3 vecA;
    private Vector3 vecB;

    private float dotProduct;
    private Vector3 crossProduct;

    private bool isCrossProduct = false;

    public TMP_Text result;

    // Start is called before the first frame update.
    // Here, it's used to set up a listener for the 'onUpdateDotCrossProduct' event.
    private void Start() {
        // Finds the first DotCrossProductEvent in the scene
        DotCrossProductEvent eventScript = FindObjectOfType<DotCrossProductEvent>();
        if (eventScript != null) {
            // Adds CalculateDotCrossProduct as a listener to the event
            eventScript.onUpdateDotCrossProduct.AddListener(CalculateDotCrossProduct);
        }
    }

    // Called to calculate and update the dot and cross product of two vectors.
    private void CalculateDotCrossProduct(Vector3 vector, bool index, bool isPolar) {
        // Debug logs for tracing the vector values
        Debug.Log(vecA + " | " + vecB);

        // Check if either vector is zero, and log a warning if so
        if (vecA == Vector3.zero || vecB == Vector3.zero) {
            Debug.Log("Both Vectors have to be active.");
        }

        // Assigns the incoming vector to vecB or vecA based on the 'index' parameter
        if (index) {
            vecB = vector;
        } else {
            vecA = vector;
        }

        // Calculates the dot and cross products
        dotProduct = Vector3.Dot(vecA, vecB);
        crossProduct = Vector3.Cross(vecA, vecB);

        // Updates the UI text based on whether dot or cross product is selected
        if (isCrossProduct) {
            result.text = crossProduct.ToString("F1");
        } else {
            result.text = dotProduct.ToString("F2");
        }

        // Debug logs for the results
        // Debug.Log("VecA: " + vecA + " | VecB: " + vecB);
        // Debug.Log("Dot Product: " + dotProduct.ToString("F2"));
        // Debug.Log("Cross Product: " + crossProduct.ToString("F1"));
    }

    // Public method to set whether to display the dot or cross product.
    public void SetDotCrossProduct(int isCross) {
        // Updates the 'isCrossProduct' flag and UI text accordingly
        if (isCross == 1) {
            isCrossProduct = true;
            result.text = crossProduct.ToString("F1");
        } else {
            isCrossProduct = false;
            result.text = dotProduct.ToString("F2");
        }
    }
}

