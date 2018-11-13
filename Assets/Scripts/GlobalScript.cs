using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {

    public GameObject circle1;
    public GameObject circle2;
    public GameObject unionPrefab;
    public GameObject enemyPrefab;
    private bool bothHeld = false;
    public bool debug = false;
    private Vector3 size = new Vector3(10f,10f,10f);


    // Use this for initialization
    void Start () {

        //We create an instance of the cylinder that will be our line
        unionPrefab = Instantiate(unionPrefab, new Vector3(), new Quaternion());

        Invoke("SpawnEnemyMoving", 1f);

    }

    void SpawnEnemyMoving() {

        //Random location to spawn the enemies
        Vector3 randomPositionOffscreen = Camera.main.ViewportToWorldPoint(new Vector3(0f, Random.Range(0.1f, 0.9f), 0f));

        randomPositionOffscreen.x = randomPositionOffscreen.x - Screen.width/4;

        randomPositionOffscreen.z = 10f;
        //Instantation of the enemy
        GameObject enemy = Instantiate(enemyPrefab, randomPositionOffscreen, Quaternion.identity);

        //Size of the enemies
        enemy.transform.localScale = size;

        //Adding movement
        enemy.GetComponent<Rigidbody>().AddForce(10000f, 0f, 0f);

        enemy.transform.rotation = Quaternion.Euler(0, 90, 0);

        //Destroy after x time
        Destroy(enemy, 10f);


        //In order to keep spawning enemies, we call again the same method
        Invoke("SpawnEnemyMoving", 1f);
    }



    void SpawnEnemyRandom() {

        //Random location to spawn the enemies
        Vector3 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value,0f));
        randomPositionOnScreen.z = 10f;
        //Instantation of the enemy
        GameObject enemy = Instantiate(enemyPrefab, randomPositionOnScreen, Quaternion.identity);
 
        //Size of the enemies
        enemy.transform.localScale = size;

        //In order to keep spawning enemies, we call again the same method
        Invoke("SpawnEnemyRandom", 1f);
    }

    void UpdateUnionLine() {

        //The line's position is in the middle of the interpolation between the 2 vectors 
        Vector3 c1Pos = circle1.transform.position;
        Vector3 c2Pos = circle2.transform.position;

        //In order to keep the line as the last object in view, we add 10 to the z Axis. 
        c1Pos.z += 10;
        c2Pos.z += 10;


        //Interpolation(Lerp)
        unionPrefab.transform.position = Vector3.Lerp(c1Pos, c2Pos, 0.5f);
        //unionObject.transform.position = c1Pos;


        //The cylinder has to face one of the circles
        unionPrefab.transform.LookAt(c2Pos);

        //The lenght of the lane has to be equal to the distance between the circles
        Vector3 scale = unionPrefab.transform.localScale;
        scale.z = Vector3.Distance(circle1.transform.position, circle2.transform.position);
        unionPrefab.transform.localScale = scale;

        //RayCast collision detection
        CollisionRaycast(c1Pos, c2Pos - c1Pos , scale.z);

    }

    // Function that creates a raycast and detects if a collision is produced, if it is, destroys the enemy that has collided with
    private void CollisionRaycast(Vector3 origin, Vector3 direction, float maxDistance) {
        //Layermask in order to avoid collisions with layer 8
        int layerMask = 1 << 8;
        //Because the enemies are in layer 8, we invert the layermask so we only collide with them
        layerMask = ~layerMask;

        //We create a RaycastHit to be able to work with the collision when is produced
        RaycastHit hit;

        //Creation of the ray
        bool ray = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

        //Detection of collision, destroys the enemy that collides with the ray
        if (hit.collider != null) {
            GameObject.Destroy(hit.collider.gameObject);

        }
    }

  

    // Update is called once per frame
    void Update() {

        
        bool circle1Held = circle1.GetComponent<Player>().isPressed() || debug;
        bool circle2Held = circle2.GetComponent<Player>().isPressed() || debug;

        // Checks if both circles are held
        bothHeld = circle1Held && circle2Held;  
        
        if (bothHeld) {

            //We render and update the union line
            unionPrefab.GetComponent<Renderer>().enabled = true;
            UpdateUnionLine();

        } else {
            //IF we stop holding the circles, the line has to be removed from view
            unionPrefab.GetComponent<Renderer>().enabled = false;
                }
		
	}
}
