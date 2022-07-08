using MultiValueDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiValueDictionary.TestProject
{
    public class FakeMultiValueDictionary<TKey, TValue> : IMultiValueDictionary<TKey, TValue> where TKey : notnull
    {
        private IDictionary<TKey, ICollection<TValue>> _dictionary;

        public FakeMultiValueDictionary()
        {
            _dictionary = new Dictionary<TKey, ICollection<TValue>>();
            //_dictionary = IoC.Get<IDictionary<TKey, ICollection<TValue>>>();
            //_dictionary =IoC.GetService<IMultiValueDictionary<string, string>>();
        }

        public void Add(TKey key, TValue member)
        {

        }

        public ICollection<TValue> AllMembers()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IDictionary<TKey, ICollection<TValue>> Items()
        {
            throw new NotImplementedException();
        }

        public bool KeyExistes(TKey key)
        {
            throw new NotImplementedException();
        }

        public ICollection<TKey> Keys()
        {
            if (_dictionary == null || _dictionary.Keys.Count() == 0)
                return null;
            return _dictionary.Keys;
        }

        public bool MemberExistes(TKey key, TValue member)
        {
            throw new NotImplementedException();
        }

        public ICollection<TValue> Members(TKey key)
        {
            throw new NotImplementedException();
        }

        public void Remove(TKey key, TValue member)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
