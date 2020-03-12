using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject gameMaster;
    private AudioSource audioSource;

    public int generation = 3;
    public int killPoints = 100;
    public float rotationSpeed, movementSpeed;
    public GameObject[] roks;
    public Vector2 movementDirection;
    public AudioClip explodeAsteroid;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        audioSource = gameMaster.GetComponent<AudioSource>();

        rotationSpeed = Random.Range(-100, 100);
        movementSpeed = Random.Range(15, 80);
        movementDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));

        rb.AddForce(movementDirection * movementSpeed);
    }
    private void Update()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        ScreenTeleport();
    }
    private void ScreenTeleport()       //method teleports asteroids to another side, then they are close to edge
    {
        float minX = -7, maxX = 7, minY = -6, maxY = 6;
        if (transform.position.x < minX) transform.position = new Vector2(maxX, transform.position.y);
        if (transform.position.x > maxX) transform.position = new Vector2(minX, transform.position.y);
        if (transform.position.y < minY) transform.position = new Vector2(transform.position.x, maxY);
        if (transform.position.y > maxY) transform.position = new Vector2(transform.position.x, minY);
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            gameMaster.GetComponent<GameMaster>().score += killPoints;      //increase score 
            if (gameMaster.GetComponent<GameMaster>().score % 1000 == 0)
            {
                gameMaster.GetComponent<GameMaster>().SpawnUfo();
            }
            Destroy(collision.gameObject);      //destoy bullet
            gameMaster.GetComponent<GameMaster>().ExplodeEffect(collision.gameObject);      //spawn effect on collision point

            generation--;       //decrease generetion
            Split(generation);      //create new asteroids
            Destroy(gameObject);        //destroy existing asteroid
            audioSource.PlayOneShot(explodeAsteroid, 0.8f);     //*boom* snd
        }
        if (collision.gameObject.tag == "Player")
        {
            gameMaster.GetComponent<GameMaster>().Dead();
            generation--;       //decrease generetion
            Split(generation);      //create new asteroids
            Destroy(gameObject);        //destroy existing asteroid
            audioSource.PlayOneShot(explodeAsteroid, 0.8f);     //*boom* snd

        }
    }

    private void Split(int currentGeneretion)
    {
        if(currentGeneretion > 0) 
        {
            int randomNum = Random.Range(1, 3);
            killPoints += 50;       // smaller asteroid -> more score
            while (randomNum > 0)       //make random numer of small asteroids
            {
                randomNum--;
                int randomId = Random.Range(0, roks.Length);        //choose random type
                float scaleNum = 0.5f;      //how much new asteroid will be smaller
                GameObject rock = Instantiate(roks[randomId], transform.position, Quaternion.identity);     //spawn asteroid
                rock.transform.localScale = new Vector3(rock.transform.localScale.x * scaleNum, rock.transform.localScale.y * scaleNum, rock.transform.localScale.z * scaleNum);        //make spawned asteroid smaller
                rock.GetComponent<Asteroid>().generation = currentGeneretion;       //send generation info
            }
        }
    }
}
