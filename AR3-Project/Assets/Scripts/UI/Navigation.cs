using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void navigateToMain() {
        SceneManager.LoadScene("AR-Scene");
    }
}
