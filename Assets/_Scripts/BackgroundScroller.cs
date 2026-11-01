using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scrolling")]
    [SerializeField] public float speed = 2f;

    private Transform copy;
    private float width;
    private float startX;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        startX = transform.position.x;
        enabled = false;
        GameObject clone = Instantiate(gameObject, transform.position + Vector3.right * width,
                                       transform.rotation, transform.parent);
        enabled = true;

        Destroy(clone.GetComponent<BackgroundScroller>());
        copy = clone.transform;
    }

    void Update()
    {
        Vector3 move = Vector3.left * speed * Time.deltaTime;
        transform.position += move;
        copy.position += move;

        if (transform.position.x <= startX - width) 
            transform.position += Vector3.right * width * 2f;

        if (copy.position.x <= startX - width) 
            copy.position += Vector3.right * width * 2f;
    }
}
