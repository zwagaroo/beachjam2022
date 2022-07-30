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
    //07/30: Heart sprite images initialized
    public Image heartImage;
    public Sprite halfheart;
    public Sprite emptyheart;
    public AudioManager am;
    public GameObject explosionPrefab;
    public GameObject tryAgainPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        if(healthBar != null)
        {
            healthBar.maxValue = (float)initialHealth;
            healthBar.value = (float)currentHealth;
        }
        print(currentHealth);
        am = FindObjectOfType<AudioManager>();
    }

    // Displays the hearts differently based on amount of health
    public void ChangeSprite()
    {
        if (currentHealth > 45 && currentHealth < 60)
        {
            heartImage.sprite = halfheart;
            Debug.Log("Half-heart");
        }
        else if (currentHealth < 45)
        {
            heartImage.sprite = emptyheart;
            Debug.Log("Empty heart");
        }
    }

    void Update()
    {
        if(gameObject.tag == "Player")
        {
            ChangeSprite();
        }
    }

    //the enemy will be able to access the Health of this so it can be changed.
    public void changeHealth(double change)
    {
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
                StartCoroutine(PlayerDeath());
                //Player death sequence
            }
            else
            {
                Death();
            }
        }

        print(currentHealth);

        //StartCoroutine(BecomeTemporarilyInvincible());
    }

    IEnumerator PlayerDeath()
    {
        var explosionObject = Instantiate(explosionPrefab, transform.position, this.transform.rotation);
        GetComponent<PlayerController>().inputDisabled = true;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        Instantiate(tryAgainPrefab);
        Destroy(this.gameObject);
        Debug.Log("explosion done");

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
        Debug.Log(this.name + "turned invincible!");
        isInvincible = true;
        
        if(gameObject.tag == "Player"){
            am.Play("Player_Damage");
        }

        if (anim != null)
        {
            anim.SetBool("isInvincible", true);
        }
        yield return new WaitForSeconds(invincibilityDurationSeconds);

        if (anim != null)
        {
            anim.SetBool("isInvincible", false);
        }
        isInvincible = false;
        Debug.Log(this.name + "is no longer invincible!");

    }
}