using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KickPower : MonoBehaviour
{
    public game_variables_so game_variables;

    [SerializeField] public Slider slider;

    public float value = 0;

    float t;
   
    private void Update()
    {
        t += Time.deltaTime;
        if (t>1.1f)
        {
            if (value > 0)
            {
                value -= 0.2f;
            }
            t = 0;
        }
        game_variables.kick_force = value;
        slider.value = Mathf.Lerp(slider.value, value,Time.deltaTime*5);
    }


    public  void OnTap()
    {
        if (value < 1)
        {
            value += 0.1f;
        }
    }
}
