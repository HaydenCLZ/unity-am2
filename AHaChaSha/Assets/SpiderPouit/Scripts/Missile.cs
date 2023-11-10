using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;  // Le préfabriqué du missile
    [SerializeField] private float speed = 5f;  // Vitesse du missile
    [SerializeField] private float explosionTime = 3f;  // Temps avant l'explosion
    [SerializeField] private float cooldownTime = 5f;  // Temps de recharge
    [SerializeField] private Transform spellcaster;

    private bool canFire = true;

    void Update()
    {
        // Vérifiez si le sort peut être tiré
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireMissile();
            StartCoroutine(Cooldown());
        }
    }

    void FireMissile()
    {
        // Crée et déplace le missile
        GameObject missile = Instantiate(missilePrefab, spellcaster.position, transform.rotation);
        missile.transform.Rotate(Vector3.right, 90f);
        missile.SetActive(true);
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        // Détruisez le missile après un certain temps
        Destroy(missile, explosionTime);
    }

    IEnumerator Cooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
    }
}
