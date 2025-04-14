using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float strafeSpeed = 5f;

    [Header("Настройки наклона")]
    [SerializeField] private float tiltAngle = 30f;
    [SerializeField] private float tiltReturnSpeed = 5f;

    [Header("Ограничения")]
    [SerializeField] private float cameraEdgeOffset = 0.2f;      // отступ от краёв камеры
    [SerializeField] private float roadBorderOffset = 0.02f;     // ~2 пикселя, если 1 юнит = 100 пикселей

    private float _horizontalInput;
    private float _verticalInput;
    private Rigidbody2D _rb;
    private float _currentTilt;
    private Camera _mainCamera;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D или стрелки
        _verticalInput = Input.GetAxisRaw("Vertical");     // W/S или стрелки
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleTilt();
    }

    private void HandleMovement()
    {
        // Предполагаемая новая позиция
        Vector2 movement =
            transform.up * (_verticalInput * moveSpeed * Time.fixedDeltaTime) +
            transform.right * (_horizontalInput * strafeSpeed * Time.fixedDeltaTime);

        Vector2 newPosition = _rb.position + movement;

        // Ограничим позицию
        newPosition = ClampPositionToCamera(newPosition);

        // Перемещаем
        _rb.MovePosition(newPosition);
    }


    private void HandleTilt()
    {
        float targetTilt = 0f;

        if (Mathf.Abs(_horizontalInput) > 0.1f || Mathf.Abs(_verticalInput) > 0.1f)
        {
            // Поворачиваем в зависимости от направления: если назад - наклон в другую сторону
            float direction = Mathf.Sign(_verticalInput != 0 ? _verticalInput : 1f);
            targetTilt = -_horizontalInput * tiltAngle * direction;
        }

        _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, tiltReturnSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, _currentTilt);
    }

    private Vector2 ClampPositionToCamera(Vector2 position)
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));

        float minX = bottomLeft.x + cameraEdgeOffset + roadBorderOffset;
        float maxX = topRight.x - cameraEdgeOffset - roadBorderOffset;
        float minY = bottomLeft.y + cameraEdgeOffset;
        float maxY = topRight.y - cameraEdgeOffset;

        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector2(clampedX, clampedY);
    }

}
