using UnityEngine;

namespace HCID274._UI
{
    public class BillBoardUI : MonoBehaviour
    {
        private Camera playerCamera; // 玩家摄像机

        private void Start()
        {
            playerCamera = Camera.main; // 获取主摄像机
        }

        private void LateUpdate()
        {
            LookAtPlayer(); // 每帧在LateUpdate中让UI面向玩家
        }

        private void LookAtPlayer()
        {
            // 计算UI应该面向的方向，使其面向摄像机
            Vector3 directionToFace = transform.position - playerCamera.transform.position;
            directionToFace.y = 0; // 这将确保UI只在X和Z轴上旋转，而不会在Y轴上旋转
            Quaternion targetRotation = Quaternion.LookRotation(directionToFace); // 计算目标旋转
            transform.rotation = targetRotation; // 应用目标旋转
        }
    }
}
