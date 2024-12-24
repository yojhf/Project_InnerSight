using InnerSight_Kys;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public string sound;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("asd");
            AudioManager.Instance.Play(sound);
        }

    }
}
