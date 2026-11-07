using UnityEngine;

public class Killer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Kill(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Kill(collision.gameObject);
        }
    }

    private void Kill(GameObject playerObj)
    {
        PlayerMovement player = playerObj.GetComponent<PlayerMovement>();

        if (!player.isDead)
        {
            player.Die();
        }
    }
}
