using UnityEngine;

using System.Collections;

public class objectHoldTurnGreen : MonoBehaviour
{
    public SpriteRenderer renderer;

    private Color startColor;

    private void Start()
    {
        startColor = renderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")|| other.CompareTag("Troops"))
        {
            renderer.color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Troops"))
        {
            renderer.color = startColor;
        }
    }
}
