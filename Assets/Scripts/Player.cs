using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public float rotationSpeed;
    public float movementSpeed;
    public GameObject bullet, shootPos, thruster;
    public AudioClip shotBullet;
    public AudioSource audioSource;
    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.W))        //thruster anim :p
        {
            thruster.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            thruster.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        ScreenTeleport();
        ShipControls();
    }

    private void ShipControls()
    {
        transform.Rotate(0, 0, Input.GetAxis("Horizontal") * -rotationSpeed * Time.deltaTime);      //allows to rotate ship by pressing Horiz axis buttons
        rb.AddForce(transform.up * movementSpeed * Math.Abs(Input.GetAxis("Vertical")));       //allows to move over the map by using vertical axis button  
    }
    private void ScreenTeleport()       //method teleports asteroids to another side, then they are close to edge
    {
        float minX = -7, maxX = 7, minY = -6, maxY = 6;
        if (transform.position.x < minX) transform.position = new Vector2(maxX, transform.position.y);
        if (transform.position.x > maxX) transform.position = new Vector2(minX, transform.position.y);
        if (transform.position.y < minY) transform.position = new Vector2(transform.position.x, maxY);
        if (transform.position.y > maxY) transform.position = new Vector2(transform.position.x, minY);
    }
    public void ReSpawn()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = startPosition;    //stop all force by using zero V3
        rb.angularVelocity = 0f;       //stop all rotation forces using zero angular
    }
    void Shoot()
    {
        audioSource.PlayOneShot(shotBullet, 0.5f);
        Instantiate(bullet, shootPos.transform.position, transform.rotation);       //create bullet prefab
    }
}
