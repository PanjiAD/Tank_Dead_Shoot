using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health = 5;
    public float stoppingDistance;
    public float retreatDistance;
    public float startShots;
    private float timeShots;
    
    private Transform player;
    public GameObject projectTile;
    public GameObject destroyEffect;

    SpriteRenderer spriteRenderer;
    // private UnityEngine.Object enemyRef;
    private Material matDefault;

    // Start is called before the first frame update
    void Start()
    {
        // enemyRef = ;
        spriteRenderer = GetComponent<SpriteRenderer>();
        matDefault = spriteRenderer.material;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeShots = startShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        if (timeShots <= 0)
        {
            Instantiate(projectTile, transform.position, Quaternion.identity);
            timeShots = startShots;
        }
        else
        {
            timeShots -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            health--;

            if (health <= 0)
            {
                kill();
            }
            else{
                Invoke("ResetMaterial", 0.1f);
            }
        }
    }

    void ResetMaterial(){
        spriteRenderer.material = matDefault;
    }

    private void kill(){
        GameObject e = Instantiate(destroyEffect) as GameObject;
        e.transform.position = transform.position;
        // Destroy(other.gameObject);
        
        spriteRenderer.enabled = false;
        
        Invoke("Respawn", 1);
    }

    void Respawn(){
        GameObject clone = Instantiate(Resources.Load("enemy"), transform.position, Quaternion.identity) as GameObject;
        // clone.transform.position = ;
        // Destroy(gameObject);
        Destroy(gameObject);
    }
}
