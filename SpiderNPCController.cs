using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This code could potentially be used for other GameObject which could be helpful with a bit of change
 * but would still polyvalently work.
 */
[DefaultExecutionOrder(-1)] // Make sure the input movement is applied before the spider itself will do a ground check and possibly add gravity
public class SpiderNPCController : MonoBehaviour {


    [Header("Debug")]
    public bool showDebug; //bool that can draw debug lines for better understanding of the NPC spider.

    [Header("Spider Reference")]
    public Spider spider; //reference to spider class

    private float perlinDirectionStep = 0.07f;
    private float perlinSpeedStep = 0.5f;
    private float startValue;

    private Vector3 Z;
    private Vector3 X;
    private Vector3 Y;

    private void Awake() {
        Random.InitState(System.DateTime.Now.Millisecond) //initialize a number generator based on current millisecond
        startValue = Random.value;

        //Initialize Coordinate System based on the spider's orientation.
        Z = transform.forward;
        X = transform.right;
        Y = transform.up;
    }

    private void FixedUpdate() { //this function allows to update physics-related factors.
        updateCoordinateSystem(); //update based on the spider's ground.

        Vector3 input = getRandomDirection() * getRandomBinaryValue(0, 1, 0.4f); //a random direction vector and a random binary value get generated. (for the stop and go value)
        //with perlin.
        spider.walk(input);
        spider.turn(input);

        if (showDebug) Debug.DrawLine(spider.transform.position, spider.transform.position + input * 0.1f *spider.getScale(), Color.cyan,Time.fixedDeltaTime);
    }

    private void Update() { //called once per frame for non-physics related updates.
        if (showDebug) {
            Debug.DrawLine(spider.transform.position, spider.transform.position + X * 0.1f * spider.getScale(), Color.red);
            Debug.DrawLine(spider.transform.position, spider.transform.position + Z * 0.1f * spider.getScale(), Color.blue);
        }
    }
    private void updateCoordinateSystem() { //update the local coordinate system of the NPC spider.
        Vector3 newY = spider.getGroundNormal();
        Quaternion fromTo = Quaternion.FromToRotation(Y, newY);
        X = Vector3.ProjectOnPlane(fromTo * X, newY).normalized;
        Z = Vector3.ProjectOnPlane(fromTo * Z, newY).normalized;
        Y = newY;
    }

    private Vector3 getRandomDirection() //get random values between [-1,1] using perlin noise
        float vertical = 2.0f * (Mathf.PerlinNoise(Time.time * perlinDirectionStep, startValue) - 0.5f);
        float horizontal = 2.0f * (Mathf.PerlinNoise(Time.time * perlinDirectionStep, startValue + 0.3f) - 0.5f);
        return (X * horizontal + Z * vertical).normalized;
    }

    // Threshold is between 0 and 1 and applies a threshold filter to the perlin noise. Min is the lower value and Max the higher value.
    private float getRandomBinaryValue(float min, float max, float threshold) {
        float value = Mathf.PerlinNoise(Time.time * perlinSpeedStep, startValue + 0.6f);
        if (value >= threshold) value = 1;
        else value = 0;
        return min + value * (max - min);// Range [min,max]
    }
}

/* What is Perlin noise? (more details)
 * A procedural animation generation algorithm,
 * its mostly used to control randomness and smoothness of movements.
 * It allows in this situation for the spider to move in a random yet realistic and coherent way
 * even if everything is randomized.
 *
 *
 * What is quaternion use?
 * a four-component vector (x,y,z,w) is considered a quaternion.
 * It is used for a pure rotation in a 3-dimensional space.
 * W represents the amount of rotation around that specific axes.
 * => we are using it here to handle in an easier way rotations.
 *
 * 
*/
