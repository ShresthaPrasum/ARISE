using UnityEngine;
using System.Collections;

public class slab : MonoBehaviour
{
    public Sprite newSprite;

    private Sprite oldSprite;
    private int touchingObj = 0;

    void Start()
    {
        oldSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {
        if (touchingObj > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = oldSprite;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Troops"))
        {
           touchingObj++;
        }
        
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Troops"))
        {
            touchingObj--;
        }
    }


}