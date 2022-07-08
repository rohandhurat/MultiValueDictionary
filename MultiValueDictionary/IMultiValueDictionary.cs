using System.Collections.Generic;

namespace MultiValueDictionary
{
    public interface IMultiValueDictionary<TKey, TValue> 
        where TKey : class
    {
        void Add(TKey key, TValue member);
        ICollection<TValue> AllMembers();
        void Clear();
        IDictionary<TKey, ICollection<TValue>> Items();
        bool KeyExistes(TKey key);
        ICollection<TKey> Keys();
        bool MemberExistes(TKey key, TValue member);
        ICollection<TValue> Members(TKey key);
        void Remove(TKey key, TValue member);
        void RemoveAll(TKey key);
    }
}