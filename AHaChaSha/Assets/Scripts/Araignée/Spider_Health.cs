using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Spider_Health : MonoBehaviour
{
    [SerializeField] private int level;
    public int maxHealth;
    public int currentHealth;
    public bool alive = true;
    public float timer_respawn;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100 * level;
        currentHealth = maxHealth;
        alive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            alive = false;
            transform.position = GetComponent<IASpider>().spawn;
            Transform target = GetComponent<IASpider>().target;
            target.GetComponent<XP>().addXP(level);
            gameObject.SetActive(false);
            Invoke("Respawn", timer_respawn);
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Spider took " + damage + " damage");
    }
}
