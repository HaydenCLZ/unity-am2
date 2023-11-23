using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;  // Le préfabriqué du missile
    [SerializeField] private float cooldownTime = 5f;  // Temps de recharge
    [SerializeField] private Transform spellcaster;

    private bool canFire = true;

    void Update()
    {
        // Vérifiez si le sort peut être tiré
        if (Input.GetKeyDown(KeyCode.Q) && canFire)
        {
            FireMissile();
            StartCoroutine(Cooldown());
        }
    }

    void FireMissile()
    {
        // Crée et déplace le missile
        GameObject missile = Instantiate(missilePrefab, spellcaster.position, missilePrefab.transform.rotation);
        missile.transform.up = transform.forward;
        missile.SetActive(true);
    }


    IEnumerator Cooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
    }
}
