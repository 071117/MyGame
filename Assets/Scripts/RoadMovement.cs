using UnityEngine;

public class RoadMovement : MonoBehaviour 
{
    public float scrollSpeed = 5f;
    public float roadHeight = 20f;

    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        if (transform.position.y < -roadHeight)
        {
            Vector3 newPos = transform.position;
            newPos.y += roadHeight * 2;
            transform.position = newPos;
        }
    }
}