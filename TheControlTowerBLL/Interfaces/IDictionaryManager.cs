﻿namespace TheControlTowerBLL.Interfaces
{
    public interface IDictionaryManager<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        bool Remove(TKey key);
        bool Update(TKey key, TValue value);
        TValue Get(TKey key);
        List<string> ToStringList();
        int Count { get; }
        List<TValue> GetAll();
    }
}
