using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ButtonScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeScene(){
        SceneManager.LoadScene("Level");

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
