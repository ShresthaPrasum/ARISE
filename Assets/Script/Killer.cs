using UnityEngine;

public class Killer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip deathSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
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

        if (player != null && !player.isDead)
        {
            // Tells the player to die instantly cause the player is dead and the game is over breh

            player.Die();

            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
        }
    }
}