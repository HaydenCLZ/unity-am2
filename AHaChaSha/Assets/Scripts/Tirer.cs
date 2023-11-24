using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tirer : MonoBehaviour
{
    // Update is called once per frame
    public Transform armeTransform;
    public GameObject projectilePrefab;
    public Camera mainCamera;


    void Update()
    {
        // Capture de la position du curseur
        Vector3 positionCurseur = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z));

        // Orientation de l'arme vers la position du curseur
        Vector3 direction = positionCurseur - armeTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        armeTransform.rotation = Quaternion.LookRotation(-direction, direction*angle);

        // Gestion du tir
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instanciation du projectile depuis l'arme
        GameObject balle = Instantiate(projectilePrefab, armeTransform.position, armeTransform.rotation);
        balle.transform.up = balle.transform.forward;
        balle.SetActive(true);
        // Ajoutez ici la logique de gestion du tir (son, effet, etc.)
    }
}

