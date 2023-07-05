using UnityEngine;
namespace Yes.Game.Chicken
{
    public class ItemManager : SingletonPatternBase<ItemManager>
    {
        public void AddItem(PropFunType itemType, int amount = 1)
        {
            string key = GetKeyForItemType(itemType);
            int currentCount = PlayerPrefs.GetInt(key, 0);
            PlayerPrefs.SetInt(key, currentCount + amount);
        }

        // 减少道具数量的函数
        public void RemoveItem(PropFunType itemType, int amount = 1)
        {
            string key = GetKeyForItemType(itemType);
            int currentCount = PlayerPrefs.GetInt(key, 0);

            if (currentCount < amount)
            {
                return; 
            }

            PlayerPrefs.SetInt(key, currentCount - amount);
            return;
        }

        // 获取道具数量的函数
        public int GetItemCount(PropFunType itemType)
        {
            string key = GetKeyForItemType(itemType);
            return PlayerPrefs.GetInt(key, 0); // 如果道具不存在，返回0
        }

        // 根据道具类型获取对应的键名
        private string GetKeyForItemType(PropFunType itemType)
        {
            return itemType.ToString();
        }
    }
    public enum PropFunType
    {
        BACKCARD=1,
        REARRANGE=2,
        BACKTHREE=3,
        RELIFE=4
    }
}