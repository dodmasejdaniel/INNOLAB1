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

enum Product {
    DOT,
    CROSS,
    NONE
}

public class VectorNav : MonoBehaviour
{
    public GameObject arrowPrefab;
    private GameObject arrowInstanceA;
    private GameObject arrowInstanceB;
    private Transform coordSystem;

    public GameObject inputPanel;
    public GameObject infoBackground;
    public GameObject[] menus;
    public GameObject productDisplay;

    public TMP_Text menuText;

    private Mode selectedMenu = Mode.POLAR;
    private readonly String[] menuNames = { "Kartesisch", "Polar", "" };
    private Product selectedProduct = Product.NONE;
    private readonly String[] products = { "Skalar", "Kreuz", "" };

    public TMP_Text productTypeText;
    public TMP_Text productResultText;

    public GameObject vectorImage;
    public TMP_Text vectorText;

    private bool isInputToggled = false;
    private bool isArrowB = false;

    public bool GetIsArrowB() => isArrowB;
    public bool GetIsPolar() => selectedMenu == Mode.POLAR;

    public GameObject GetActualArrow() => isArrowB ? arrowInstanceB : arrowInstanceA;

    private readonly int totalEnumValues = Enum.GetNames(typeof(Mode)).Length;

    // BUTTONCLICK: Vector A button is clicked
    public void HandleVecAButtonClick() {
        HandleVectorButton(false, AClick);
    }

    // BUTTONCLICK: Vector B button is clicked
    public void HandleVecBButtonClick() {
        HandleVectorButton(true, BClick);
    }

    // Common logic for both Vector A and B button clicks
    void HandleVectorButton(bool isB, Action clickAction) {
        // Check if coord system exists
        if (coordSystem == null) {
            if (FindCoordSystem()) {
                Debug.Log("Coord system not found");
                return;
            }
        }

        // Conditional execution based on current toggle state
        if (isInputToggled) {
            if (isArrowB != isB) {
                clickAction();
                ReloadInputs();
            } else {
                TogglePanel();
            }
        } else {
            clickAction();
            TogglePanel();
        }
    }

    // Reload UI Input Fields to the actual arrow position
    void ReloadInputs() {
        switch (selectedMenu) {
            case Mode.CARTESIAN: menus[(int)selectedMenu].GetComponent<CartesianMenu>().SetInputBasedOnArrow(); break;
            case Mode.POLAR: menus[(int)selectedMenu].GetComponent<PolarMenu>().SetInputBasedOnArrow(); break;
            default: Debug.Log("No Mode found."); return;
        }
    }

    // Logic for Vector A click
    void AClick() {
        isArrowB = false;
        SetArrow(ref arrowInstanceA, "A", new Color(0.1215686f, 0.003921569f, 0.3882353f));
    }

    // Logic for Vector B click
    void BClick() {
        isArrowB = true;
        SetArrow(ref arrowInstanceB, "B", new Color(0.569f, 1.0f, 0.191f));
    }

    // Sets up the arrow instance
    void SetArrow(ref GameObject arrowInstance, string text, Color color) {
        // Update UI elements
        vectorText.text = text;
        vectorImage.GetComponent<Image>().color = color;

        // Instantiate arrow if it doesn't exist
        if (arrowInstance == null) {
            // Your instantiation logic here
            arrowInstance = Instantiate(arrowPrefab, coordSystem);
            Renderer rend = arrowInstance.transform.Find("arrow/3D-Arrow").GetComponent<Renderer>();
            rend.material = new Material(rend.material) {
                color = color
            };
        }
    }

    // Toggles input panel
    void TogglePanel() {
        isInputToggled = !isInputToggled;
        if (isInputToggled) {
            ChangeMenu();
            productDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1228);
        } else {
            productDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 413);
        }
        inputPanel.SetActive(isInputToggled);
        
    }

    // Finds the coordinate system, returns true if not found
    bool FindCoordSystem() {
        coordSystem = GameObject.FindGameObjectWithTag("coord-system").GetComponent<Transform>();
        return coordSystem == null;
    }

    // BUTTONCLICK: Moves to the next menu
    public void NextMenu() {
        selectedMenu = (Mode)(((int)selectedMenu + 1) % totalEnumValues);
        ChangeMenu();
    }

    // BUTTONCLICK: Moves to the previous menu
    public void PrevMenu() {
        selectedMenu = (Mode)(((int)selectedMenu - 1 + totalEnumValues) % totalEnumValues);
        ChangeMenu();
    }

    // BUTTONCLICK: Change to DOT-Product
    public void DotProductButton() {
        ChangeProduct(Product.DOT);
    }

    // BUTTONCLICK: Change to CROSS-Product
    public void CrossProductButton() {
        ChangeProduct(Product.CROSS);
    }

    // Disables all menus
    void DisableAllMenus() {
        foreach (GameObject menu in menus) {
            menu.SetActive(false);
        }
    }

    // Changes to the selected menu
    void ChangeMenu() {
        DisableAllMenus();
        // Enable the selected menu
        menus[(int)selectedMenu].SetActive(true);
        menuText.text = menuNames[(int)selectedMenu];
    }

    // Changes to the selected product
    void ChangeProduct(Product newProduct) {

        // Stop, if the vectors are not set.
        if (arrowInstanceA == null || arrowInstanceB == null) {
            infoBackground.SetActive(true);
            infoBackground.GetComponentInChildren<TMP_Text>().text = "Platziere zuerst 2 Vektoren.";
            return;
        } else {
            infoBackground.SetActive(false);
        }

        // Doube-click disables the menu and returns (the new product is only the same, if the user clicked on the same product-button)
        if (selectedProduct == newProduct) {
            productDisplay.SetActive(false);
            selectedProduct = Product.NONE;
            return;
        }
        selectedProduct = newProduct;
        gameObject.GetComponent<DotCrossProductCalculator>().SetDotCrossProduct((int)selectedProduct);
        productTypeText.text = products[(int)selectedProduct] + "produkt:";
        productDisplay.SetActive(true);
    }

}
