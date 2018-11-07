using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class Player : MonoBehaviour {
    public Camera camera;
    public float movementRatio;
    public float threshold;
    private float screenRatio;
    private bool pressed = false;
    public float holdTime = .1f;
    private float currentTime = 0;


    // Use this for initialization
    void Start() {
        screenRatio = camera.aspect;
        //this.transform.localScale = new Vector3(6f  6f * screenRatio);
    }

    public bool isPressed() {
        return pressed;
    }

    // Update is called once per frame
    void Update() {


        Vector2 minDistanceVector = this.transform.position;
        int minDistIndex = 0;

        // If theres a touch
        if (Input.touchCount > 0) {

            float minDist = threshold;


            for (int i = 0; i < Input.touchCount; i++) {
                // We map the position of the touch with ScreenToWorldPoint().
                Vector2 touchPosMapped = camera.ScreenToWorldPoint(Input.GetTouch(i).position);

                // We obtain the position of the circle 
                Vector2 circlePosMapped = this.transform.position;

                // Calculus of the distance
                float dist = Vector2.Distance(touchPosMapped, circlePosMapped);


                //We find the closest touch
                if (minDist > dist) {
                    minDist = dist;
                    minDistanceVector = touchPosMapped;
                    minDistIndex = i;
                }


            }

            // If it's closer than the threshold
            if (minDist < threshold) {
                currentTime += Input.GetTouch(minDistIndex).deltaTime;
                // We move the position of the circle to the position.
                this.GetComponent<Rigidbody>().MovePosition(minDistanceVector * movementRatio);
                if (currentTime >= holdTime) {
                    pressed = true;
                }

                if (Input.GetTouch(minDistIndex).phase == TouchPhase.Ended) {
                    {
                        currentTime = 0;
                        pressed = false;
                    }

                

                }


            }

        }

    }
}

        

	


