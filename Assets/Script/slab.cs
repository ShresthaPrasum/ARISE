using UnityEngine;
using System.Collections;

public class slab : MonoBehaviour
{
    public Sprite newSprite;

    private Sprite oldSprite;

    void Start()
    {
        oldSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Troops"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite =  newSprite;
        }
        
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Troops"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite  = oldSprite;
        }
    }
}