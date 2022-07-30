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
    //07/30: Heart sprite images initialized
    public SpriteRenderer spriteRenderer;
    public Sprite halfheart;
    public Sprite emptyheart;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        healthBar.maxValue = (float)initialHealth;
        healthBar.value = (float)currentHealth;
        print(currentHealth);
        am = FindObjectOfType<AudioManager>();
    }

    // Displays the hearts differently based on amount of health
    public void ChangeSprite()
    {
        if (currentHealth > 45 && currentHealth < 60)
        {
            spriteRenderer.sprite = halfheart;
            Debug.Log("Half-heart");
        }
        else if (currentHealth < 45)
        {
            spriteRenderer.sprite = emptyheart;
            Debug.Log("Empty heart");
        }
    }

    void Update()
    {
        ChangeSprite();
    }

    //the enemy will be able to access the Health of this so it can be changed.
    public void changeHealth(double change)
    {
        if (isInvincible) return;
        if(gameObject.tag == "Player"){
            am.Play("Player_Damage");
        }
        currentHealth += change;
        healthBar.value = (float)currentHealth;

        print(currentHealth);

        StartCoroutine(BecomeTemporarilyInvincible());
    }

    public bool isDead()
    {
        return (currentHealth <= 0);
    }

    //Invincibility
    public float invincibilityDurationSeconds;
    public bool isInvincible = false;

    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log(this.name + "turned invincible!");
        isInvincible = true;

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