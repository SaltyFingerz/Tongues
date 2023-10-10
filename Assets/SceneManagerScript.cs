using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
   
    IEnumerator waitToTravel()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Bedroom");
    }

    public void LoadBedroomScene()
    {
        StartCoroutine(waitToTravel());
    }
}
