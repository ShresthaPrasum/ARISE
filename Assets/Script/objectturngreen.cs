using UnityEngine;
using System.Collections;

public class objectturngreen : MonoBehaviour {

    public SpriteRenderer renderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Troops"))
        {
                renderer.color = Color.green;
        }
    }
}