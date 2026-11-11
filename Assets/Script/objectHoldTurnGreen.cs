using UnityEngine;

using System.Collections;

public class objectHoldTurnGreen : MonoBehaviour
{
    public SpriteRenderer renderer;

    public SpriteRenderer outlineRenderer;

    private Color startColor;

    private Color outlineStartColor;

    public float objectHolding = 0f;

    private void Start()
    {
        outlineStartColor = outlineRenderer.color;
        startColor = renderer.color;
    }

    private void Update()
    {
        if(objectHolding > 0)
        {
            outlineRenderer.color = Color.green;
            renderer.color = Color.green;
        }
        else
        {
            outlineRenderer.color = startColor;
            renderer.color = startColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")|| other.CompareTag("Troops"))
        {
            objectHolding ++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Troops"))
        {
            objectHolding --;
            
        }
    }
}
