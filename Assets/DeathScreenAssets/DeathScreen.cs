using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathScreen : MonoBehaviour
{
    private bool keyHeld;
    // Start is called before the first frame update
    void Start()
    {
        keyHeld = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Input.anyKey){
            keyHeld = false;
        }
        if(Input.anyKey && !keyHeld){
            SceneManager.LoadScene("MainMenu");
        }
    }
}
