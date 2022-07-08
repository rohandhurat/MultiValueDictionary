using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiValueDictionary
{
    public class MultiValueDictionary<TKey, TValue> : IMultiValueDictionary<TKey, TValue>
        where TKey : class
    {
        //private dictionary to go all the operations
        private IDictionary<TKey, ICollection<TValue>> _dictionary;

        //DI using constuctor
        public MultiValueDictionary(IDictionary<TKey, ICollection<TValue>> dictionary)
        {
            _dictionary = dictionary;
        }


        public ICollection<TKey> Keys()
        {
            if (_dictionary == null || _dictionary.Keys.Count() == 0)
                return null;
            return _dictionary.Keys;

        }

        public ICollection<TValue> Members(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (_dictionary == null || !_dictionary.Keys.Contains(key))
                throw new Exception("key does not exist");
            else
                return _dictionary[key];

        }
        // ADD foo bar
        // ADD foo baz
        public void Add(TKey key, TValue member)
        {
            if(key == null)
                throw new ArgumentNullException("key");
            if (_dictionary.Keys.Contains(key) && _dictionary[key].Contains(member))
                throw new Exception("member already exists for key");
            if (_dictionary.Keys.Contains(key))
                _dictionary[key].Add(member);
            else
                _dictionary.Add(key, new List<TValue>() { member });
        }
        //remove foo bar
        public void Remove(TKey key, TValue member)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (!_dictionary.Keys.Contains(key))
                throw new Exception("key does not exist");
            else if (!_dictionary[key].Contains(member))
                throw new Exception("member does not exist");

            if (_dictionary[key].Count > 1)
                _dictionary[key].Remove(member);
            else
                _dictionary.Remove(key);
        }
        //removeall foo
        public void RemoveAll(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (!_dictionary.Keys.Contains(key))
                throw new Exception("key does not exist");
            else
                _dictionary.Remove(key);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool KeyExistes(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (_dictionary == null || _dictionary.Keys.Count == 0)
                return false;
            return _dictionary.Keys.Contains(key);
        }

        public bool MemberExistes(TKey key, TValue member)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (_dictionary == null || _dictionary.Keys.Count == 0 || !_dictionary.Keys.Contains(key))
                return false;
            return _dictionary[key].Contains(member);
        }

        public ICollection<TValue> AllMembers()
        {
            if (_dictionary == null || _dictionary.Keys.Count == 0)
                return null;

            List<TValue> result = new List<TValue>();
            foreach (TKey key in _dictionary.Keys)
                result.AddRange(_dictionary[key]);
            return result;
        }

        public IDictionary<TKey, ICollection<TValue>> Items()
        {
            if (_dictionary == null || _dictionary.Keys.Count == 0)
                return null;

            return _dictionary;
        }
    }
}
