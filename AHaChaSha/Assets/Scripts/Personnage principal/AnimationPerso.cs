using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPerso : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        InputsVerification();
    }

    private void InputsVerification() //V�rifie si le perso marche ou coure
    {
        //Bool marche 
        bool marche = animator.GetBool("marche");

        //Bool court
        bool court = animator.GetBool("court");

        //Bool tire 
        bool tire = animator.GetBool("tire");

        //Stock un bool d'appuie de touche
        bool forwardPress = Input.GetKey("w");
        bool leftPress = Input.GetKey("a");
        bool rightPress = Input.GetKey("d");
        //bool backwardPress = Input.GetKey("s");

        bool runPress = Input.GetKey("left shift");

        bool firepress = Input.GetKey("o");

        //V�rifie qu'au moins une touche est appuy�e 
        if (!marche && (forwardPress || leftPress || rightPress /*|| backwardPress */))
        {
            animator.SetBool("marche", true);
           
        }

        //V�rifie qu'aucune touche n'est appuy�e
        if (marche && !forwardPress && !leftPress && !rightPress /*&& !backwardPress */)
        {
            animator.SetBool("marche", false);
        }

        //V�rifie si le perso court
        if (!court && marche && runPress)
        {
            animator.SetBool("court", true);
        }

        if (court && (!marche || !runPress))
        {
            animator.SetBool("court", false);
        }

        //V�rifie si le perso tire.

        if (!tire && marche && firepress)
        {
            animator.SetBool("tire", true);
        }

        if (tire && !marche  && !firepress)
        {
            animator.SetBool("tire", false);
        }


    }
}

