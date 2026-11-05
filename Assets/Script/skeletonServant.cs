using System.Collections;
using UnityEngine;

public class SkeletonServant : MonoBehaviour
{
    public int groundLayerIndex = 6; 
    
    public float riseDepth = 1.5f; 
    public float riseSpeed = 4f;   

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        
        gameObject.layer = groundLayerIndex;
        
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        
        StartCoroutine(StartFromGround());
    }

    private IEnumerator StartFromGround()
    {
        
        if (boxCollider != null) boxCollider.enabled = false;
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
        
        Vector3 targetPos = transform.position;
        transform.position = targetPos + (Vector3.down * riseDepth);
        
        while (transform.position.y < targetPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, riseSpeed * Time.deltaTime);
            yield return null; 
        }

        transform.position = targetPos;

        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
        if (boxCollider != null) boxCollider.enabled = true;
    }
    public void SetDirection(float dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }
}