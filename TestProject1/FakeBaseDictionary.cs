using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class FakeBaseDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
    {
        public FakeBaseDictionary()
        {

        }
    }
}
