using UnityEngine;

public class TestHealthInput : MonoBehaviour
{
    public SectionHealthFuck violinSection;   

    void Update()
    {
        // fuck it over
        if (Input.GetKey(KeyCode.Alpha1))
            violinSection.SetHealth(violinSection.Health - (1f/3f) * Time.deltaTime);

        // heal
        if (Input.GetKey(KeyCode.Alpha2))
            violinSection.SetHealth(violinSection.Health + (1f/3f) * Time.deltaTime);
    }
}