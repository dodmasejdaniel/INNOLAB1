using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void navigateToVectorField() {
        SceneManager.LoadScene("VF-Scene");
    }

    public void navigateToVector() {
        SceneManager.LoadScene("V-Scene");
    }

    public void navigateToHome() {
        SceneManager.LoadScene("Home-Scene");
    }
}
