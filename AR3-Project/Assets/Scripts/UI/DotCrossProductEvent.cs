using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3, bool> { }

public class DotCrossProductEvent : MonoBehaviour {
    public Vector3Event onUpdateDotCrossProduct;

    public void TriggerEvent(Vector3 vector, bool index) {
        onUpdateDotCrossProduct.Invoke(vector, index);
    }
}
