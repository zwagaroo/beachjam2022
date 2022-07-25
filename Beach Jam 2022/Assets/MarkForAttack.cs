using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkForAttack : MonoBehaviour
{
    public List<Collider> inRangeEnemies = new List<Collider>();


    //Enemies within range are marked as attackable
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            inRangeEnemies.Add(other);
        }
    }

    //Enemies that leave the range are unmarked
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            inRangeEnemies.Remove(other);
        }
    }
}
