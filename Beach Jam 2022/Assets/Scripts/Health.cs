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
    public GameObject explosionPrefab;

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

        if(healthBar != null)
        {

            healthBar.value = (float)currentHealth;

        }


        if (isDead()){
            if(gameObject.tag == "Player")
            {
                //Player death sequence
            }
            else
            {
                Death();
            }
        }

        print(currentHealth);

        StartCoroutine(BecomeTemporarilyInvincible());
    }

    public void Death()
    {
        //yield return new WaitForSeconds(time);
        var explosionObject = Instantiate(explosionPrefab, transform.position, this.transform.rotation);
        //var explosion = explosionObject.GetComponent<ParticleSystem>();
        Destroy(this.gameObject);
        //yield return new WaitUntil(() => explosion.isPlaying == false);
        Debug.Log("explosion done");
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
