using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
   
    IEnumerator waitToTravel()
    {
        //play whoosh magical audio here!
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Bedroom");
    }

    public void LoadBedroomScene()
    {
        StartCoroutine(waitToTravel());
    }
}
