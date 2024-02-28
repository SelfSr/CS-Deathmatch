using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKey(KeyCode.I))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
