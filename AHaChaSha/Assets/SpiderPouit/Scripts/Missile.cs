using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;  // Le pr�fabriqu� du missile
    [SerializeField] private float speed = 5f;  // Vitesse du missile
    [SerializeField] private float explosionTime = 3f;  // Temps avant l'explosion
    [SerializeField] private float cooldownTime = 5f;  // Temps de recharge
    [SerializeField] private Transform spellcaster;

    private bool canFire = true;

    void Update()
    {
        // V�rifiez si le sort peut �tre tir�
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireMissile();
            StartCoroutine(Cooldown());
        }
    }

    void FireMissile()
    {
        // Cr�e et d�place le missile
        GameObject missile = Instantiate(missilePrefab, spellcaster.position, transform.rotation);
        missile.transform.Rotate(Vector3.right, 90f);
        missile.SetActive(true);
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        // D�truisez le missile apr�s un certain temps
        Destroy(missile, explosionTime);
    }

    IEnumerator Cooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
    }
}
