using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tirer : MonoBehaviour
{
    // Update is called once per frame
    public Transform armeTransform;
    public GameObject projectilePrefab;
    public float cadence = 0.1f;
    private float tempsDernierTir;


    void Update()
    {
        // Capture de la position du curseur
        Vector3 positionCurseur = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

        // Orientation de l'arme vers la position du curseur
        Vector3 direction = armeTransform.position - positionCurseur;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        armeTransform.rotation = Quaternion.LookRotation(direction, direction*angle);

        // Gestion du tir
        if (Input.GetMouseButton(0) && Time.time > tempsDernierTir + cadence)
        {
            Shoot();
            tempsDernierTir = Time.time;
        }
    }

    void Shoot()
    {
        // Instanciation du projectile depuis l'arme
        GameObject balle = Instantiate(projectilePrefab, armeTransform.position, armeTransform.rotation);
        balle.transform.Rotate(0f, 180f, 0f);
        balle.transform.up = balle.transform.forward;
        balle.SetActive(true);
        // Ajoutez ici la logique de gestion du tir (son, effet, etc.)
    }
}

