public interface ILootable
{
    void OnStartLook(); // 当玩家开始查看物品时调用
    void OnInteract(); // 当玩家与物品进行交互时调用
    void OnEndLook(); // 当玩家停止查看物品时调用
}