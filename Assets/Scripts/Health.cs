using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private int health = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function to damage the player
    public void Damage()
    {
        health--;
        Debug.Log("health decreased");
        if(health<=0)
        {
            //death animation then switch scene to death menu
            //put animation here
            SceneManager.LoadScene(sceneName:"Assets/Scenes/DeathMenu.unity");
        }
    }
}
