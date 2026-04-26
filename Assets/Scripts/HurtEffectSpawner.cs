using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// attach this to an enemy (should have the <Enemy> script):
// it will spawn hurt effects 
public class HurtEffectSpawner : MonoBehaviour
{
    [SerializeField] GameObject effect;
    private Effect script;

    private void Start()
    {
        script = effect.GetComponent<Effect>();
    }

    // spawn effects that randomly fire outwards in a circle from a point
    public void SpawnEffect()
    {
        GameObject obj = Instantiate(effect);
        obj.transform.position = transform.position;
    }
}
