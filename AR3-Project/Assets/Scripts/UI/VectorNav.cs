using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

enum Mode {
    CARTESIAN,
    POLAR
}



public class VectorNav : MonoBehaviour
{
    public GameObject arrowPrefab;
    private GameObject arrowInstanceA;
    private GameObject arrowInstanceB;
    private Transform coordSystem;

    public GameObject inputPanel;
    public GameObject[] menus;

    public TMP_Text menuText;

    private Mode selectedMenu = Mode.POLAR;
    private String[] menuNames = { "Kartesisch", "Polar" };

    public GameObject VectorImage;
    public TMP_Text VectorText;

    private bool isInputToggled = false;
    private bool isArrowB = false;

    public bool getIsArrowB() => isArrowB;
    public void setIsArrowB(bool val) => isArrowB = val;

    public GameObject getArrowA() => arrowInstanceA;
    public GameObject getArrowB() => arrowInstanceB;

    public GameObject getActualArrow() => isArrowB ? arrowInstanceB : arrowInstanceA;

    private Vector3 vector = Vector3.zero; // The current vector

    int totalEnumValues = Enum.GetNames(typeof(Mode)).Length;


    public void VecAButton() {
        if (coordSystem == null) {
            if (FindCoordSystem()) {
                Debug.Log("Coord system not found");
                return;
            }
        }
        if (isInputToggled) {
            if (isArrowB) {
                AClick();
            } else {
                TogglePanel();
            }
        } else {
            AClick();
            TogglePanel();
        }
    }

    void AClick() {
        isArrowB = false;
        VectorImage.GetComponent<Image>().color = new Color(0.1215686f, 0.003921569f, 0.3882353f);
        VectorText.text = "A";
        if (arrowInstanceA == null) {
            arrowInstanceA = Instantiate(arrowPrefab, coordSystem);
            Renderer rend = arrowInstanceA.transform.Find("arrow/3D-Arrow").GetComponent<Renderer>();
            rend.material = new Material(rend.material) {
                color = new Color(0.1215686f, 0.003921569f, 0.3882353f)
            };
        }
    }

    public void VecBButton() {
        if (coordSystem == null) {
            if (FindCoordSystem()) {
                Debug.Log("Coord system not found");
                return;
            }
        }
        if (isInputToggled) {
            if (!isArrowB) {
                BClick();
            } else {
                TogglePanel();
            }
        } else {
            BClick();
            TogglePanel();
        }
    }

    void BClick() {
        isArrowB = true;
        VectorImage.GetComponent<Image>().color = new Color(0.569f, 1.0f, 0.191f);
        VectorText.text = "B";
        if (arrowInstanceB == null) {
            arrowInstanceB = Instantiate(arrowPrefab, coordSystem);
            Renderer rend = arrowInstanceB.transform.Find("arrow/3D-Arrow").GetComponent<Renderer>();
            rend.material = new Material(rend.material) {
                color = new Color(0.569f, 1.0f, 0.191f)
            };

        }
    }

    void TogglePanel() {
        isInputToggled = !isInputToggled;
        if (isInputToggled) {
            ChangeMenu();
        }
        inputPanel.SetActive(isInputToggled);
        
    }

    bool FindCoordSystem() {
        coordSystem = GameObject.FindGameObjectWithTag("coord-system").GetComponent<Transform>();
        return coordSystem == null;
    }

    public void NextMenu() {
        selectedMenu = (Mode)(((int)selectedMenu + 1) % totalEnumValues);
        ChangeMenu();
    }

    public void PrevMenu() {
        selectedMenu = (Mode)(((int)selectedMenu - 1 + totalEnumValues) % totalEnumValues);
        ChangeMenu();
    }

    void DisableAll() {
        foreach (GameObject menu in menus) {
            menu.SetActive(false);
        }
    }

    void ChangeMenu() {
        DisableAll();

        // Enable the selected menu
        menus[(int)selectedMenu].SetActive(true);
        menuText.text = menuNames[(int)selectedMenu];
    }



}
