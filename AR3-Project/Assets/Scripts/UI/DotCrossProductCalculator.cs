using UnityEngine;

public class DotCrossProductCalculator : MonoBehaviour {

    private Vector3 vecA;
    private Vector3 vecB;

    private float dotProduct;
    private Vector3 crossProduct;

    private void Start() {
        // Hier abonnieren wir das Event
        DotCrossProductEvent eventScript = FindObjectOfType<DotCrossProductEvent>();
        if (eventScript != null) {
            eventScript.onUpdateDotCrossProduct.AddListener(CalculateDotCrossProduct);
        }
    }

    private void CalculateDotCrossProduct(Vector3 vector, bool index) {
        Debug.Log(vecA + " | " + vecB);
        if (vecA == Vector3.zero || vecB == Vector3.zero) {
            Debug.Log("Both Vectors have to be active.");
        }
        if (index) {
            vecB = vector;
        } else {
            vecA = vector;
        }
        dotProduct = Vector3.Dot(vecA, vecB);
        crossProduct = Vector3.Cross(vecA, vecB);
        Debug.Log("VecA: " + vecA + " | VecB: " + vecB);
        Debug.Log("Dot Product: " + dotProduct);
        Debug.Log("Cross Product: " + crossProduct);
    }
}
