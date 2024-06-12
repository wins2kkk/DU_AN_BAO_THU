using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    public float moveSpeed = 2f;    // Tốc độ di chuyển
    public float moveDistance = 5f; // Khoảng cách di chuyển từ vị trí ban đầu

    private Vector3 initialPosition; // Vị trí ban đầu
    private bool movingRight = true;

    void Start()
    {
        // Lưu vị trí ban đầu
        initialPosition = transform.position;
    }

    void Update()
    {
        // Kiểm tra khoảng cách di chuyển từ vị trí ban đầu
        if (transform.position.x >= initialPosition.x + moveDistance)
            movingRight = false;
        else if (transform.position.x <= initialPosition.x - moveDistance)
            movingRight = true;

        // Di chuyển đối tượng theo chiều đúng
        if (movingRight)
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        else
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }
}
