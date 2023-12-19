using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3, bool, bool> { }

public class DotCrossProductEvent : MonoBehaviour {
    public Vector3Event onUpdateDotCrossProduct;

    public void TriggerEvent(Vector3 vector, bool index, bool isPolar) {
        onUpdateDotCrossProduct.Invoke(vector, index, isPolar);
    }
}
