using UnityEngine;

public class UFO : MonoBehaviour
{
    private Rigidbody2D rb;
    private float time;
    private int killPoints = 200;
    private GameObject player, gameMaster;
    private AudioSource audioSource;

    public GameObject bullet;
    public float timeBtwSpawn;
    public AudioClip explodeUFO, shot;
    public Transform firePosTop, firePosDown;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        audioSource = gameMaster.GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        ScreenTeleport();
        if (time <=0)       //true every timeBtwSpawn seconds
        {
            Patrol();
            Shoot();
            time = timeBtwSpawn;
        }
        else
        {
            time -= Time.deltaTime;
        }
    }
    private void Patrol()
    {
        rb.AddForce(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * Random.Range(15, 30));      //move in random direction with ranmdom speed
    }
    private void Shoot()
    {
        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 
                                        player.transform.position.y - transform.position.y);        //get direction to player's position
        Vector3 firePos;
        if (direction.y > 0)
        {
            firePos = firePosTop.position;
        }
        else
        {
            firePos = firePosDown.position;
        }
        audioSource.PlayOneShot(shot);
        GameObject firedBullet = Instantiate(bullet, firePos, Quaternion.identity);
        firedBullet.transform.up = direction;       //set fire dir to player's position
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            gameMaster.GetComponent<GameMaster>().score += killPoints;      //increase score 
            Destroy(collision.gameObject);      //destoy bullet
            gameMaster.GetComponent<GameMaster>().ExplodeEffect(collision.gameObject);      //spawn effect on collision point
            Destroy(gameObject);        //destroy UFO
            audioSource.PlayOneShot(explodeUFO, 0.8f);     //*boom* snd
        }
        if (collision.gameObject.tag == "Player")
        {
            gameMaster.GetComponent<GameMaster>().Dead();
            Destroy(gameObject);        //destroy UFO
            audioSource.PlayOneShot(explodeUFO, 0.8f);     //*boom* snd

        }
    }
    private void ScreenTeleport()       //method teleports asteroids to another side, then they are close to edge
    {
        float minX = -7, maxX = 7, minY = -6, maxY = 6;
        if (transform.position.x < minX) transform.position = new Vector2(maxX, transform.position.y);
        if (transform.position.x > maxX) transform.position = new Vector2(minX, transform.position.y);
        if (transform.position.y < minY) transform.position = new Vector2(transform.position.x, maxY);
        if (transform.position.y > maxY) transform.position = new Vector2(transform.position.x, minY);
    }
}
