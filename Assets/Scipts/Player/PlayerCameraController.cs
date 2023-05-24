using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private GameObject player; // 玩家对象
    [SerializeField] private float lookSensitivity; // 鼠标敏感度
    [SerializeField] private float smoothing; // 平滑移动值
    [SerializeField] private int maxLookRotation;// 最大垂直旋转角度

    private Vector2 smoothedVelocity; // 平滑速度
    private Vector2 currentLookingPos; // 当前观察位置
    private float recoilRecoveryDelay = 0.1f; // 控制恢复后坐力的延迟时间（单位：秒）
    private Vector2 initialLookingPos; // 记录产生后坐力前摄像机的初始位置

    private Vector3 lastPosition; // 记录玩家上一帧的位置
    private bool hasMoved = false; // 记录玩家是否已经移动过


    public TutorialPanelController tutorialPanelController; // 你的TutorialPanelController对象
    public Vector2 RecoilOffset { get; set; }
    public float RecoilRecoverySpeed { get; set; } // 后座力恢复速度

    public void ApplyRecoilWithRecovery(Vector2 recoil, float recoverySpeed, Vector2 initialPos)
    {
        initialLookingPos = initialPos; // 记录产生后坐力前摄像机的初始位置
        RecoilOffset += recoil;
        RecoilRecoverySpeed = recoverySpeed;
        StartCoroutine(RecoverRecoilAfterDelay(recoilRecoveryDelay));
    }

    // 添加一个新方法，用于从外部获取当前摄像机位置
    public Vector2 GetCurrentLookingPos()
    {
        return currentLookingPos;
    }

    private IEnumerator RecoverRecoilAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (RecoilOffset != Vector2.zero)
        {
            RecoilOffset = Vector2.MoveTowards(RecoilOffset, Vector2.zero, RecoilRecoverySpeed * Time.deltaTime);
            currentLookingPos = Vector2.Lerp(currentLookingPos, initialLookingPos, RecoilRecoverySpeed * Time.deltaTime); // 向记录的摄像机状态恢复
            yield return null;
        }
    }

    private void Start()
    {
        player = transform.parent.gameObject != null ? transform.parent.gameObject : gameObject;
        lastPosition = transform.position; // 初始化lastPosition
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标光标
        Cursor.visible = false; // 隐藏鼠标光标
    }

    private void Update()
    {
        RotateCamera(); // 每帧旋转相机

        // 检查玩家是否移动
        if (transform.position != lastPosition && !hasMoved)
        {
            hasMoved = true; // 标记玩家已经移动过
            tutorialPanelController.ShowTutorial(); // 显示教程
        }

        // 检查玩家是否按下了"H"键
        if (Input.GetKeyDown(KeyCode.H))
        {
            tutorialPanelController.ShowTutorial(); // 显示教程
        }

        lastPosition = transform.position; // 更新玩家位置
    }

    private void LateUpdate()
    {
        // 更新摄像机位置
        transform.position = new Vector3(player.transform.position.x,
            player.transform.position.y + 0.48f,
            player.transform.position.z);
    }

    private void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")); // 获取鼠标输入值

        inputValues = Vector2.Scale(inputValues,
            new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));
        // 调整输入值与敏感度和平滑值的关系

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);
        // 平滑处理输入值

        currentLookingPos += smoothedVelocity + RecoilOffset; // 更新当前观察位置

        // 计算恢复后坐力的速度
        Vector2 recoverySpeed = RecoilOffset * RecoilRecoverySpeed * Time.deltaTime;

        // 将摄像机的后坐力逐渐恢复
        RecoilOffset -= recoverySpeed;

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -maxLookRotation, maxLookRotation); // 限制垂直旋转角度

        transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x, player.transform.up);
        // 旋转相机和玩家

        //Debug.Log($"Mouse Input: {inputValues}, Smoothed Velocity: {smoothedVelocity}, Current Looking Position: {currentLookingPos}");
    }
}