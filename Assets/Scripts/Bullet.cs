using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1);
    }
    private void Update()
    {
        transform.Translate(Vector2.up * 20 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
            gameMaster.GetComponent<GameMaster>().Dead();
        }
    }
}
