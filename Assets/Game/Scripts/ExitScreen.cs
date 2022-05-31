using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitForRead());
    }
    IEnumerator WaitForRead()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("0");
    }
}
