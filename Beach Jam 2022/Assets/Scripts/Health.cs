using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public double initialHealth;
    private double currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        print(currentHealth);
    }

    //the enemy will be able to access the Health of this so it can be changed.
    public void changeHealth(double change){
        if (isInvincible) return;

        currentHealth += change;
        
        print(currentHealth);
        StartCoroutine(BecomeTemporarilyInvincible());
    }

    public bool isDead(){
        return (currentHealth <= 0);
    }

        //Invincibility
    public float invincibilityDurationSeconds;
    public bool isInvincible = false;

    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log("Player turned invincible!");
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
        Debug.Log("Player is no longer invincible!");
    
    }
}
