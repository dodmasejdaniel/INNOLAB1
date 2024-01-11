using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Scene Navigation
public class Navigation : MonoBehaviour
{
    public void navigateToPreVF() {
        SceneManager.LoadScene("Pre-VF");
    }

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
