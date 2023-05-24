using UnityEngine;

namespace HCID274._Weapons
{
    public class WeaponHandler : MonoBehaviour // 武器处理类
    {
        public static WeaponHandler instance; // 单例实例
        public Gun primaryGun;
        public Gun secondaryGun;

        private Gun currentGun; // 当前使用的枪支
        private GameObject currentGunPrefab; // 当前枪支预制件的实例
        

        private void Awake()
        {
            if (instance == null)
            {
                instance = this; // 如果单例实例为空，将当前实例设为单例
            }
            else if (instance != this)
            {
                Destroy(this); // 如果已存在其他实例，销毁当前实例
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToPrimary(); // 当玩家按下1键时，切换到主枪
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchToSecondary(); // 当玩家按下2键时，切换到次枪   
            }
        }

        public void PickUpGun(Gun gun)
        {
            if (primaryGun == null)
            {
                primaryGun = gun; // 如果主枪为空，则设拾取的枪为主枪
                SwitchToPrimary(); // 切换到主枪
            }
            else if (secondaryGun == null)
            {
                secondaryGun = gun; // 如果次枪为空，则设拾取的枪为次枪
                SwitchToSecondary(); // 切换到次枪
            }
            else
            {
                // 如果已经有两把枪，替换当前装备的枪
                if (currentGun == primaryGun)
                {
                    // 实例化掉落的枪支预制体
                    Instantiate(primaryGun.gunPickup, transform.position + transform.forward, Quaternion.identity);
                    primaryGun = gun; // 将拾取的枪设为主枪
                    SwitchToPrimary(); // 切换到主枪
                }
                else
                {
                    // 实例化掉落的枪支预制体
                    Instantiate(secondaryGun.gunPickup, transform.position + transform.forward, Quaternion.identity);
                    secondaryGun = gun; // 将拾取的枪设为次枪
                    SwitchToSecondary(); // 切换到次枪
                }
            }
        }

        private void SwitchToPrimary()
        {
            if (primaryGun != null)
            {
                EquipGun(primaryGun); // 如果主枪不为空，则装备主枪
            }
        }

        private void SwitchToSecondary()
        {
            if (secondaryGun != null)
            {
                EquipGun(secondaryGun); // 如果次枪不为空，则装备次枪
            }
        }


        private void EquipGun(Gun gun)
        {
            currentGun = gun; // 设置当前枪为装备的枪
            if (currentGunPrefab != null)
            {
                Destroy(currentGunPrefab); // 如果当前枪预制件不为空，则销毁
            }

            // 将选择的枪支实例化，并将实例化后的预制件赋值给currentGunPrefab变量
            currentGunPrefab = Instantiate(gun.gameObject, transform);
            // 使用AmmunitionManager的单例，调用其AmmunitionUI组件，更新弹药类型为当前装备的枪支
            AmmunitionManager.instance.ammunitionUI.UpdateAmmunitionType(currentGun);
        }
    }
}
