using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider healthBar;
    public double initialHealth;
    private double currentHealth;
    [SerializeField]
    private Animator anim;
    public AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        currentHealth = initialHealth;
        healthBar.maxValue = (float)initialHealth;
        healthBar.value = (float)currentHealth;
        print(currentHealth);
    }

    //the enemy will be able to access the Health of this so it can be changed.
    public void changeHealth(double change){
        if (isInvincible) return;
        if(gameObject.tag == "Player"){
            am.Play("Player_Damage");
        }
        

        currentHealth += change;
        healthBar.value = (float)currentHealth;

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
        Debug.Log(this.name +"turned invincible!");
        isInvincible = true;

        if(anim != null)
        {
            anim.SetBool("isInvincible", true);
        }
        yield return new WaitForSeconds(invincibilityDurationSeconds);

        if(anim != null)
        {
            anim.SetBool("isInvincible", false);
        }
        isInvincible = false;
        Debug.Log(this.name +"is no longer invincible!");
    
    }
}
