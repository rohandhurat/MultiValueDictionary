using Microsoft.Extensions.DependencyInjection;
using MultiValueDictionary;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MultiValueDictionary.TestProject
{
    public class Tests
    {
        //private ServiceProvider serviceProvider = new ServiceCollection()
        //        .AddSingleton<IDictionary<string, ICollection<string>>, Dictionary<string, ICollection<string>>>()
        //        .BuildServiceProvider();


        private IMultiValueDictionary<string, string> _dictionary;

        [SetUp]
        public void Setup()
        {
            IoC.Register<IMultiValueDictionary<string, string>, FakeMultiValueDictionary<string, string>>();

            _dictionary = IoC.Get<IMultiValueDictionary<string, string>>();
            _dictionary.Add("foo", "bar");
            _dictionary.Add("baz", "bang");
        }

        [Test]
        public void TestKeys()
        {
            //_dictionary.Add("foo", "bar");
            //_dictionary.Add("baz", "bang");
            var result = _dictionary.Keys();
            var expected = new string[] { "baz", "foo" };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void TestMembersValidKey()
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            var result = _dictionary.Members("foo");
            var expected = new string[] { "bar", "baz" };
            CollectionAssert.AreEquivalent(expected, result);

        }
        [Test]
        public void TestMembersInvalidKey()
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            var ex = Assert.Throws<Exception>(() => _dictionary.Members("bad"));

            Assert.That(ex.Message, Is.EqualTo("key does not exist"));

        }
        [Test]
        [TestCase("foo", "bar")]
        [TestCase("foo", "baz")]
        public void TestAddDuplicate(string key, string value)
        {
            _dictionary.Add(key, value);
            var ex = Assert.Throws<Exception>(() => _dictionary.Add(key, value));

            Assert.That(ex.Message, Is.EqualTo("member already exists for key"));

        }
        [TestCase("foo", "bar")]
        public void TestAdd(string key, string value)
        {
            Assert.DoesNotThrow(() => _dictionary.Add(key, value));

        }



        [TestCase("foo", "bar")]
        public void TestRemoveSucess(string key, string value)
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            Assert.DoesNotThrow(() => _dictionary.Remove(key, value));
        }

        [TestCase("foo", "bar")]
        public void TestRemoveMemberNotExists(string key, string value)
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            Assert.DoesNotThrow(() => _dictionary.Remove(key, value));
            var ex = Assert.Throws<Exception>(() => _dictionary.Remove(key, value));

            Assert.That(ex.Message, Is.EqualTo("member does not exist"));
        }

        [TestCase("foo", "bar")]
        public void TestRemoveCheckKeys(string key, string value)
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            Assert.DoesNotThrow(() => _dictionary.Remove(key, value));
            var result = _dictionary.Keys();
            var expected = new string[] { "foo" };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void TestRemoveCheckKeysEmptySet()
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            Assert.DoesNotThrow(() => _dictionary.Remove("foo", "bar"));
            Assert.DoesNotThrow(() => _dictionary.Remove("foo", "baz"));
            var result = _dictionary.Keys();
            Assert.IsNull(result);
        }
        [Test]
        public void TestRemoveInvalidKey()
        {
            var ex = Assert.Throws<Exception>(() => _dictionary.Remove("boom", "pow"));

            Assert.That(ex.Message, Is.EqualTo("key does not exist"));
        }
        [Test]
        public void TestRemoveAllSucess()
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            var result = _dictionary.Keys();
            var expected = new string[] { "foo" };
            CollectionAssert.AreEquivalent(expected, result);
            Assert.DoesNotThrow(() => _dictionary.RemoveAll("foo"));
            result = _dictionary.Keys();
            Assert.IsNull(result);
        }
        [Test]
        public void TestRemoveAllInvalidKey()
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("foo", "baz");
            var result = _dictionary.Keys();
            var expected = new string[] { "foo" };
            CollectionAssert.AreEquivalent(expected, result);
            Assert.DoesNotThrow(() => _dictionary.RemoveAll("foo"));
            var ex = Assert.Throws<Exception>(() => _dictionary.RemoveAll("foo"));

            Assert.That(ex.Message, Is.EqualTo("key does not exist"));
        }
        [Test]
        public void TestClear()
        {
            _dictionary.Add("foo", "bar");
            _dictionary.Add("bang", "zip");
            var result = _dictionary.Keys();
            var expected = new string[] { "foo", "bang" };
            CollectionAssert.AreEquivalent(expected, result);
            Assert.DoesNotThrow(() => _dictionary.Clear());
            result = _dictionary.Keys();
            Assert.IsNull(result);

            Assert.DoesNotThrow(() => _dictionary.Clear());
            result = _dictionary.Keys();
            Assert.IsNull(result);
        }
        [Test]
        public void KeyExistesSucess()
        {

            var result = _dictionary.KeyExistes("foo");
            Assert.That(result, Is.False);


            _dictionary.Add("foo", "bar");
            result = _dictionary.KeyExistes("foo");
            Assert.That(result, Is.True);
        }
        [Test]
        public void MemberExistesSucess()
        {

            var result = _dictionary.MemberExistes("foo", "bar");
            Assert.That(result, Is.False);


            _dictionary.Add("foo", "bar");
            result = _dictionary.MemberExistes("foo", "bar");
            Assert.That(result, Is.True);

            result = _dictionary.MemberExistes("foo", "baz");
            Assert.That(result, Is.False);
        }
        [Test]
        public void AllMembersSucess()
        {

            var result = _dictionary.AllMembers();
            Assert.IsNull(result);

            Assert.DoesNotThrow(() => _dictionary.Add("foo", "bar"));
            Assert.DoesNotThrow(() => _dictionary.Add("foo", "baz"));

            result = _dictionary.AllMembers();

            var expected = new string[] { "bar", "baz" };
            CollectionAssert.AreEquivalent(expected, result);

            Assert.DoesNotThrow(() => _dictionary.Add("bang", "bar"));
            Assert.DoesNotThrow(() => _dictionary.Add("bang", "baz"));

            result = _dictionary.AllMembers();

            expected = new string[] { "bar", "baz", "bar", "baz" };
            CollectionAssert.AreEquivalent(expected, result);

        }
        [Test]
        public void ItemsSucess()
        {
            var result = _dictionary.Items();
            Assert.IsNull(result);

            Assert.DoesNotThrow(() => _dictionary.Add("foo", "bar"));
            Assert.DoesNotThrow(() => _dictionary.Add("foo", "baz"));

            result = _dictionary.Items();

            Dictionary<string, ICollection<string>> expected = new Dictionary<string, ICollection<string>>();
            expected.Add("foo", new string[] { "bar", "baz" });

            CollectionAssert.AreEquivalent(expected, result);

            Assert.DoesNotThrow(() => _dictionary.Add("bang", "bar"));
            Assert.DoesNotThrow(() => _dictionary.Add("bang", "baz"));

            result = _dictionary.Items();

            expected.Add("bang", new string[] { "bar", "baz" });
            CollectionAssert.AreEquivalent(expected, result);

        }
    }
}