using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Common.Cache
{
    public class Cache
    {
        private static Dictionary<string, CacheItemInfo> collection = new Dictionary<string, CacheItemInfo>(GlobalConfig.CacheCapacity);
        private static int capacity = GlobalConfig.CacheCapacity;
        private static bool locked = false;

        public static bool Add(string key, object value)
        {
            if (locked)
            {
                return false;
            }
            else
            {
                if (collection.ContainsKey(key))
                {
                    collection[key].Value = value;
                    collection[key].CreateTime = DateTime.Now;
                    return true;
                }
                else
                {
                    if (collection.Count == capacity)
                    {
                        Thread threadCleanUp = new Thread(CleanUp);
                        threadCleanUp.IsBackground = true;
                        threadCleanUp.Start();
                        return false;
                    }
                    else
                    {
                        CacheItemInfo item = new CacheItemInfo();
                        item.Value = value;
                        item.CreateTime = DateTime.Now;
                        collection.Add(key, item);
                        if (collection.Count == capacity)
                        {
                            Thread threadCleanUp = new Thread(CleanUp);
                            threadCleanUp.IsBackground = true;
                            threadCleanUp.Start();
                        }
                        return true;
                    }
                }
            }
        }

        public static bool ContainsKey(string key)
        {
            if (locked)
            {
                return false;
            }
            else
            {
                return collection.ContainsKey(key);
            }
        }

        public static object Get(string key)
        {
            if (locked)
            {
                return null;
            }
            else
            {
                if (!collection.ContainsKey(key))
                {
                    return null;
                }
                else
                {
                    collection[key].Hits++;
                    return collection[key].Value;
                }
            }
        }

        //方法名需要修改，当缓存量达到一定程度时进行清理
        private static void CleanUp()
        {
            locked = true;
            SortedDictionary<int, int> dic = new SortedDictionary<int, int>();
            foreach (KeyValuePair<string,CacheItemInfo>  item in collection)
            {
                if (dic.ContainsKey(item.Value.Hits))
                {
                    dic[item.Value.Hits]++;
                }
                else
                {
                    dic.Add(item.Value.Hits, 1);
                }
            }
            int toBeRemovedNumber = capacity / 10;
            int count=0;
            int maxHits=0;

            //看情况
            DateTime latest = DateTime.Now.AddHours(-1);
            foreach (int key in dic.Keys)
            {
                count += dic[key];
                if (count > toBeRemovedNumber)
                {
                    maxHits = key;
                    break;
                }
            }
            List<string> keys = new List<string>(count);
            foreach (string key in collection.Keys)
            {
                if (collection[key].Hits <= maxHits && collection[key].CreateTime < latest)
                {
                    keys.Add(key);
                }
            }
            for (int i = 0; i < count; i++)
            {
                collection.Remove(keys[i]);
            }
            locked = false;
        }
    }
}
