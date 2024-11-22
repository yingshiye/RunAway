using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ButtonScene : MonoBehaviour
{
    private float timer;
    void Start()
    {
        timer = -1;
    }

    public void ChangeScene(){
        GameObject.Find("Black").GetComponent<Animator>().Play("FadeIn");
        GameObject.Find("Button").GetComponent<Animator>().Play("FadeOut");
        timer = 0.75F;
    }


    // Update is called once per frame
    void Update()
    {
        if(timer > 0){
            timer -= Time.deltaTime;
            if(timer <= 0){
                SceneManager.LoadScene("Level");
            }
        }
    }
}
