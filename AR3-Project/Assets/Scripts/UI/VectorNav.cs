using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VectorNav : MonoBehaviour
{
    public GameObject arrowPrefab;
    private GameObject arrowInstanceA;
    private GameObject arrowInstanceB;
    private Transform coordSystem;

    public GameObject inputPanel;

    private bool isInputToggled = false;
    private bool isArrowB = false;

    public bool getIsArrowB() => isArrowB;
    public void setIsArrowB(bool val) => isArrowB = val;

    public GameObject getArrowA() => arrowInstanceA;
    public GameObject getArrowB() => arrowInstanceB;

    private Vector3 vector = Vector3.zero; // The current vector

    public void VecAButton() {
        if (!isInputToggled) {
            if (coordSystem == null) {
                if (FindCoordSystem()) {
                    Debug.Log("Coord system not found");
                    return;
                }
            }
            if (arrowInstanceA == null) {
                arrowInstanceA = Instantiate(arrowPrefab, coordSystem);
            }
        }
        TogglePanel();
    }

    void TogglePanel() {
        isInputToggled = !isInputToggled;
        inputPanel.SetActive(isInputToggled);
    }

    bool FindCoordSystem() {
        coordSystem = GameObject.FindGameObjectWithTag("coord-system").GetComponent<Transform>();
        return coordSystem == null;
    }


}
