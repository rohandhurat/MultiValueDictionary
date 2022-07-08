using Moq;
using MultiValueDictionary;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestProject1
{
    public class Tests
    {
        private IMultiValueDictionary<string, string>? _mvdictionary;

        [SetUp]
        public void Setup()
        {
            IoC.Register<IMultiValueDictionary<string, string>, MultiValueDictionary<string, string>>();
            IoC.Register<IDictionary<string, ICollection<string>>, FakeBaseDictionary<string, ICollection<string>>>();

            _mvdictionary = IoC.Get<IMultiValueDictionary<string, string>>();

        }

        [Test]
        public void Should_Return_Null_For_No_Values()
        {
            //Arrange
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>();
            mockBaseDict.Setup(x => x.Keys).Returns(keys);

            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);
            //Act
            var result = _mvdictionary.Keys();
            //Assert
            Assert.AreEqual(null, result);

            _mvdictionary = IoC.Get<IMultiValueDictionary<string, string>>();

            var result1 = _mvdictionary.Keys();
            Assert.AreEqual(null, result1);
        }

        [Test]
        public void Should_Return_Keys_For_Added_Value()
        {
            //Arrange
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);

            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);
            //Act
            var result = _mvdictionary.Keys();
            //Assert
            Assert.AreEqual(keys, result);
        }

        [Test]
        [TestCase("foo")]
        public void Should_Return_Members_For_Valid_Key_Value(string key)
        {
            //Arrange
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);


            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);
            //ACT
            var result = _mvdictionary.Members(key);
            //ASSERT
            CollectionAssert.AreEquivalent(values, result);

        }
        [Test]
        [TestCase(null)]
        public void Should_Not_Return_Members_For_NULL_Key_Value(string key)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);

            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            var ex = Assert.Throws<ArgumentNullException>(() => _mvdictionary.Members(key));

            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'key')"));

        }
        [Test]
        [TestCase("foo", "bar")]
        public void Should_Not_Add_Duplicate_Member_For_Key(string key, string value)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);

            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            var ex = Assert.Throws<Exception>(() => _mvdictionary.Add(key, value));

            Assert.That(ex?.Message, Is.EqualTo("member already exists for key"));

        }
        [Test]
        [TestCase(null, "bar")]
        public void Should_Not_Add_Null_Key(string key, string value)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            mockBaseDict.Setup(x => x[key].Add(value)).Verifiable();
            mockBaseDict.Setup(x => x.Add(key, new List<string>() { value })).Verifiable();
            var ex = Assert.Throws<ArgumentNullException>(() => _mvdictionary?.Add(key, value));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'key')"));
        }

        [Test]
        [TestCase("foo", null)]
        public void Should_Add_Null_Value(string key, string value)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            mockBaseDict.Setup(x => x[key].Add(value)).Verifiable();
            mockBaseDict.Setup(x => x.Add(key, new List<string>() { value })).Verifiable();
            Assert.DoesNotThrow(() => _mvdictionary?.Add(key, value));
        }

        [Test]
        [TestCase("foo", "bar")]
        public void Should_Add_Not_Null_Value(string key, string value)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            mockBaseDict.Setup(x => x[key].Add(value)).Verifiable();
            mockBaseDict.Setup(x => x.Add(key, new List<string>() { value })).Verifiable();
            Assert.DoesNotThrow(() => _mvdictionary?.Add(key, value));
        }

        [Test]
        [TestCase("foo", "bar")]
        public void Should_Remove_Key_Value(string key, string value)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);

            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            Assert.DoesNotThrow(() => _mvdictionary.Remove(key, value));
        }

        [Test]
        [TestCase("foo", "bad")]
        public void Should_Return_Message_Member_Not_Exists(string key, string value)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);

            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            var ex = Assert.Throws<Exception>(() => _mvdictionary.Remove(key, value));

            Assert.That(ex?.Message, Is.EqualTo("member does not exist"));
        }

        [Test]
        [TestCase("foo", "bar")]
        public void Should_Remove_Value(string key, string value)
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            Assert.DoesNotThrow(() => _mvdictionary?.Remove(key, value));
        }

        
        [Test]
        public void Should_Return_Message_If_Invalid_Key_Remove()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            var ex = Assert.Throws<Exception>(() => _mvdictionary?.Remove("boom", "pow"));

            Assert.That(ex?.Message, Is.EqualTo("key does not exist"));
        }
        [Test]
        public void Should_Remove_All()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            Assert.DoesNotThrow(() => _mvdictionary?.RemoveAll("foo"));
            
        }
        [Test]
        public void Should_Return_Message_If_Invalid_Key_RemoveAll()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            var ex = Assert.Throws<Exception>(() => _mvdictionary?.RemoveAll("bang"));

            Assert.That(ex?.Message, Is.EqualTo("key does not exist"));
        }
        [Test]
        public void Should_Clear_Sucess()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            Assert.DoesNotThrow(() => _mvdictionary?.Clear());
            
        }
        [Test]
        public void Should_Return_True_For_Key_Exists()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            var result = _mvdictionary?.KeyExistes("foo");
            Assert.That(result, Is.True);

        }

        [Test]
        public void Should_Return_False_For_Key_Not_Exists()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);

            var result = _mvdictionary?.KeyExistes("bang");
            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_Return_True_For_Member_Exists()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);


            var result = _mvdictionary?.MemberExistes("foo", "bar");
            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_Return_False_For_Member_Not_Exists()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);


            var result = _mvdictionary?.MemberExistes("foo", "bang");
            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_return_All_Members()
        {
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            ICollection<string> values = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            mockBaseDict.Setup(x => x["foo"]).Returns(values);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);


            var result = _mvdictionary?.AllMembers();

            CollectionAssert.AreEquivalent(values, result);

        }
        [Test]
        public void Should_Return_All_Items()
        {
            //Arrange
            var mockBaseDict = new Mock<IDictionary<string, ICollection<string>>>();
            ICollection<string> keys = new List<string>() { "foo" };
            mockBaseDict.Setup(x => x.Keys).Returns(keys);
            ICollection<string> expected = new List<string>() { "bar", "baz" };
            mockBaseDict.Setup(_=>_.TryGetValue("foo", out expected)).Returns(true);
            _mvdictionary = new MultiValueDictionary<string, string>(mockBaseDict.Object);


            //Act
            ICollection<string> actual = null;
            var result = _mvdictionary?.Items().TryGetValue("foo",out actual);


            // Assert
            Assert.AreEqual(expected, actual);

        }
    }
}