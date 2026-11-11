using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    public Transform newParent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")|| other.CompareTag("Troops"))
        {
            other.transform.SetParent(gameObject.transform);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Troops"))
        {
            other.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Troops"))
        {
            other.transform.SetParent(null);
        }
    }
}