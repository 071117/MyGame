using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private void Update()
    {
        // Двигаем вниз
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        // Удаляем объект, если он вышел за нижнюю границу экрана
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}

