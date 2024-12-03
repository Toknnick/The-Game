using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость перемещения
    public float smoothSpeed = 0.1f; // Коэффициент сглаживания

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position; // Запоминаем начальную позицию камеры
    }

    void Update()
    {
        // Получаем ввод пользователя
        float moveX = Input.GetAxis("Horizontal"); // A и D
        float moveY = Input.GetAxis("Vertical");   // W и S

        // Вычисляем целевую позицию
        targetPosition += new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

        // Плавное перемещение камеры
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
