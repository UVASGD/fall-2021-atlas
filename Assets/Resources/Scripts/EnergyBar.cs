using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public int maxEnergy = 100;
    public int currentEnergy;




    void Start()
    {
        currentEnergy = maxEnergy;
        setMaxEnergy(maxEnergy);
    }

    void Update()
    {
        //energy bar test
        //use "]" to manually lower energy
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            loseEngery(20);
        }
    }


    // set max value of energy
    public void setMaxEnergy(int energy)
    {
        slider.maxValue = energy;
        slider.value = energy;
    }

    //update value of energy
    public void setEnergy(int energy)
    {
        slider.value = energy;
    }

    //energy bar test
    void loseEngery(int energy)
    {
        currentEnergy -= energy;
        setEnergy(currentEnergy);
    }
}
