using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider slider;
    public int maxHealth = 100;
    public int currentHealth;




    void Start()
    {
        currentHealth = maxHealth;
        setMaxHealth(maxHealth);
    }

    void Update()
    {
        //health bar test
        //use "[" to manually lower health
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            TakeDamage(20);
        }
    }


    // set max value of health
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    //update value of health
    public void setHealth(int health)
    {
        slider.value = health;
    }

    //health bar test
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        setHealth(currentHealth);
    }


}
