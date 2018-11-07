using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {

    public GameObject circle1;
    public GameObject circle2;
    public GameObject unionPrefab;
    public GameObject enemyPrefab;
    private bool bothHeld = false;


    // Use this for initialization
    void Start () {

        //We create an instance of the cylinder that will be our line
        unionPrefab = Instantiate(unionPrefab, new Vector3(), new Quaternion());

        Invoke("SpawnEnemy", 1f);

    }

    void OnTriggerEnter(Collider collision) {
        print("collision");
        enemyPrefab = collision.gameObject;
            
    }

    void SpawnEnemy() {

        //Random location to spawn the enemies
        Vector3 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value,0f));
        randomPositionOnScreen.z = 10f;
        //Instantation of the enemy
        GameObject enemy = Instantiate(enemyPrefab, randomPositionOnScreen, Quaternion.identity);
 
        //Size of the enemies
        enemy.transform.localScale = new Vector3(10f, 10f, 10f);

        //In order to keep spawning enemies, we call again the same method
        Invoke("SpawnEnemy", 1f);
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
        bool collision = CollisionRaycast(c1Pos, c2Pos - c1Pos , scale.z);
        
        if (collision) {
            print("collision");
        } 

    }

    private bool CollisionRaycast(Vector3 origin, Vector3 direction, float maxDistance) {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;

        bool ray = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

        if (hit.collider != null) {
            GameObject.Destroy(hit.collider.gameObject);

        }

        return ray;
    }

    // Update is called once per frame
    void Update() {

        
        bool circle1Held = circle1.GetComponent<Player>().pressed;
        bool circle2Held = circle2.GetComponent<Player>().pressed;

        // Checks if both circles are held
        if (circle1Held && circle2Held) {

            //We render and update the union line
            unionPrefab.GetComponent<Renderer>().enabled = true;
            UpdateUnionLine();

        } else {
            //IF we stop holding the circles, the line has to be removed from view
            unionPrefab.GetComponent<Renderer>().enabled = false;
                }
		
	}
}
