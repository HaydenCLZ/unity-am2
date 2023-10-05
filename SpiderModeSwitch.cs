using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A simple class which allows switching between a player input spider controller 
 * and a spider randomly controlled.
 * A switch of controller and camera is performed.
 * We need to make sure two cameras for both mode is settled.
 * */

public class SpiderModeSwitch : MonoBehaviour {

    public SpiderController spiderController;
    public SpiderNPCController spiderNPC;
    public Camera controllerCam;
    public Camera npcCam;

    void Start() {
        //Start with spider camera enabled
        if (controllerCam.enabled && npcCam.enabled) npcCam.enabled = false;
        if (!controllerCam.enabled && !npcCam.enabled) controllerCam.enabled = true;

        // Start with spider controlled by us enabled
        if (spiderController.enabled && spiderNPC.enabled) spiderNPC.enabled = false;
        if (!spiderController.enabled && !spiderNPC.enabled) spiderController.enabled = true;
    }
    void Update() { //if we decide to press tab an d change the way the spider is working.

        if (Input.GetKeyDown(KeyCode.Tab)) {
            controllerCam.enabled = !controllerCam.enabled;
            npcCam.enabled = !npcCam.enabled;
            spiderNPC.enabled = !spiderNPC.enabled;
            spiderController.enabled = !spiderController.enabled;
        }
    }
}
