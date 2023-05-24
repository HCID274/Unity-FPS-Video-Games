using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float speed; // 玩家移动速度
    [SerializeField] private float jumpForce; // 玩家跳跃力
    [SerializeField] private float jumpRaycastDistance; // 用于检测地面的射线距离
    [SerializeField] private Transform spawnPoint; // 玩家出生点

    private Rigidbody rb; // 玩家的刚体组件

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // 获取刚体组件
        transform.position = spawnPoint.position; // 将玩家位置设置为出生点位置
    }

    private void Update()
    {
        Jump(); // 每帧执行跳跃
    }

    private void FixedUpdate()
    {
        Move(); // 每个固定更新执行移动
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal"); // 获取水平输入
        float vAxis = Input.GetAxisRaw("Vertical"); // 获取垂直输入

        // 计算移动方向和速度
        Vector3 movement = new Vector3(hAxis, 0, vAxis) * speed * Time.fixedDeltaTime;

        // 计算新的玩家位置
        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement);

        rb.MovePosition(newPosition); // 更新玩家位置
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 按下空格键时
        {
            if (IsGrounded()) // 如果玩家在地面上
            {
                // 添加向上的跳跃力
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded()
    {
        // 使用射线检测玩家是否在地面上
        return Physics.Raycast(transform.position, Vector3.down, jumpRaycastDistance);
    }
}
