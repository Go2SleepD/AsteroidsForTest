using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    private int playerHealth = 3;
    private Vector3 randomPos;
    private GameObject player;
    private AudioSource audioSource;

    public GameObject explodeEffect, ufo;
    public GameObject[] roks;
    public AudioClip explodeShip;
    public Text scoreText;
    public int score, everyScore;
    public float checkRadius;

    private void Start()
    {
        score = 0;

        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        scoreText.text = score.ToString();
        
        if (GameObject.FindGameObjectWithTag("Asteroid") == null)       //when player destroyed all asteroids -> make new
        {
            SpawnAsteroids();
        }
    }
    private void SpawnAsteroids()
    {
        for (int spawnNum = 0; spawnNum < 4; spawnNum ++)       //choose value of asteroids
        {
            int randomNum = Random.Range(0, roks.Length);       //choose random asteroid type  
            randomPos = new Vector3(Random.Range(-6, 6), Random.Range(-5, 5), 0);       //choose random pos to spawn

            while ((randomPos.x < (player.transform.position.x + checkRadius) & randomPos.x > (player.transform.position.x - checkRadius)) ||(randomPos.y < (player.transform.position.y + checkRadius) & randomPos.y > (player.transform.position.y - checkRadius)))
            // this shit up just check if randomPoint for asteroid is in radius of checkRadius to not to spawn asteroid close to player. Yeah i can do it another way, more simple and read-frindly, but...who will eaven read this? No one ever read code. 
            {
                randomPos = new Vector3(Random.Range(-6, 6), Random.Range(-5, 5), 0);       //choose random pos to spawn again
            }
            GameObject rock = Instantiate(roks[randomNum], randomPos, Quaternion.identity);     //spawn asteroids with uppedr params
        }
    }
    public void Dead()
    {
        ExplodeEffect(player);      //create explode
        audioSource.PlayOneShot(explodeShip);       //*boom* snd of ship
        playerHealth--;     //decrease lifes
        GameObject life = GameObject.Find("Life" + playerHealth.ToString());        //choose correct life image
        Destroy(life);      //destroy it

        player.gameObject.SetActive(false);     //death "anim"
        if (playerHealth > 0)
        {
            player.gameObject.SetActive(true);
            player.GetComponent<Player>().ReSpawn();        //respawn, if have lifes
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    public void SpawnUfo()
    {
        randomPos = new Vector3(Random.Range(-6, 6), Random.Range(-5, 5), 0);       //choose random pos to spawn
        while ((randomPos.x < (player.transform.position.x + checkRadius) & randomPos.x > (player.transform.position.x - checkRadius)) || (randomPos.y < (player.transform.position.y + checkRadius) & randomPos.y > (player.transform.position.y - checkRadius)))
        // this shit up just check if randomPoint for asteroid is in radius of checkRadius to not to spawn asteroid close to player. Yeah i can do it another way, more simple and read-frindly, but...who will eaven read this? No one ever read code. 
        {
            randomPos = new Vector3(Random.Range(-6, 6), Random.Range(-5, 5), 0);       //choose random pos to spawn again
        }
        GameObject rock = Instantiate(ufo, randomPos, Quaternion.identity);
    }
    public void ExplodeEffect(GameObject point)     //method to spawn effect and destroy it to direct point
    {
        GameObject effect = Instantiate(explodeEffect, point.transform.position, Quaternion.identity);     //create explode effect
        Destroy(effect, 1f);        //destoy effcet on finish
    }
}
