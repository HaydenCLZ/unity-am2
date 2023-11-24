using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;  // Le préfabriqué du missile
    [SerializeField] private float cooldownTime;  // Temps de recharge
    [SerializeField] private Transform caster;

    private bool canFire = true;

    void Update()
    {
        // Vérifiez si le sort peut être tiré
        if (Input.GetKeyDown(KeyCode.Q) && canFire)
        {
            Fire();
            StartCoroutine(Cooldown());
        }
    }

    void Fire()
    {
        // Crée et déplace le missile
        GameObject projectile = Instantiate(projectilePrefab, caster.position, projectilePrefab.transform.rotation);
        projectile.transform.up = transform.forward;
        projectile.SetActive(true);
    }


    IEnumerator Cooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
    }
}
