using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Galaxus.Functional.Tests
{
    [TestFixture]
    public class EitherTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement", Justification = "The 'new' throws anyways - at least it should")]
        public void CtorThrowsWhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Either<string, object>(default(string)));
            Assert.Throws<ArgumentNullException>(() => new Either<string, object>(default(object)));
        }

        [Test]
        public void UnionEqualityAndHashCodeWithValueTypeVariant()
        {
            var union1 = new Either<string, int>(117);
            var union2 = new Either<string, int>(666);
            var union3 = new Either<string, int>(117);
            Assert.AreNotEqual(union1, union2);
            Assert.AreEqual(union1, union3);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
            Assert.AreEqual(union1.GetHashCode(), union3.GetHashCode());
        }

        [Test]
        public void UnionEqualityAndHashCodeWithReferenceTypeVariant()
        {
            var union1 = new Either<string, int>("hello");
            var union2 = new Either<string, int>("bye");
            var union3 = new Either<string, int>("hello");
            Assert.AreNotEqual(union1, union2);
            Assert.AreEqual(union1, union3);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
            Assert.AreEqual(union1.GetHashCode(), union3.GetHashCode());
        }

        [Test]
        public void UnionEqualityAndHashCodeWithMixedTypeVariants()
        {
            var union1 = new Either<string, int>(117);
            var union2 = new Either<string, int>("hello");
            Assert.AreNotEqual(union1, union2);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
        }

        [Test]
        public void UnionEqualityWithMixedValueTypeVariantsBothZero()
        {
            var union1 = new Either<byte, int>(default(byte));
            var union2 = new Either<byte, int>(default(int));
            Assert.AreNotEqual(union1, union2);
        }

        [Test]
        [TestCaseSource(nameof(IEitherReturnsCorrectTypeTestCases))]
        public void IEitherReturnsCorrectType(IEither givenInterface, Type expectedType)
        {
            Assert.AreEqual(expectedType, givenInterface.ToObject().GetType());
        }

        [Test]
        [TestCase(3)]
        [TestCase("Foo")]
        [TestCase(default(byte))]
        public void ToStringDelegates<T>(T arg)
        {
            var eitherLeft = new Either<T, Unit>(arg);
            var toStringLeftResult = eitherLeft.ToString();
            Assert.IsNotNull(toStringLeftResult);
            Assert.AreEqual(arg.ToString(), toStringLeftResult);

            var eitherRight = new Either<Unit, T>(arg);
            var toStringRightResult = eitherRight.ToString();
            Assert.IsNotNull(toStringRightResult);
            Assert.AreEqual(arg.ToString(), toStringRightResult);
        }

        private static IEnumerable<object[]> IEitherReturnsCorrectTypeTestCases()
        {
            return IEitherReturnsCorrectTypeTestCasesExplicit()
                .Select(testObj => new object[] {testObj.GivenInterface, testObj.ExpectedType});
        }

        private static IEnumerable<(IEither GivenInterface, Type ExpectedType)>
            IEitherReturnsCorrectTypeTestCasesExplicit()
        {
            yield return (new Either<string, char>("hello"), typeof(string));
            yield return (new Either<string, char>('B'), typeof(char));
            yield return (new Either<string, char, bool>("World"), typeof(string));
            yield return (new Either<string, char, bool>('B'), typeof(char));
            yield return (new Either<string, char, bool>(false), typeof(bool));
        }

        [Test]
        [TestCaseSource(nameof(IEitherReturnsCorrectObjectTestCases))]
        public void IEitherReturnsCorrectObject(IEither givenInterface, object expectedObject)
        {
            Assert.AreEqual(expectedObject, givenInterface.ToObject());
        }

        private static IEnumerable<object[]> IEitherReturnsCorrectObjectTestCases()
        {
            return IEitherReturnsCorrectObjectTestCasesExplicit()
                .Select(testObj => new [] {testObj.GivenInterface, testObj.ExpectedObject});
        }

        private static IEnumerable<(IEither GivenInterface, object ExpectedObject)>
            IEitherReturnsCorrectObjectTestCasesExplicit()
        {
            yield return (new Either<string, char>("Digitec"), "Digitec");
            yield return (new Either<string, char>('X'), 'X');
            yield return (new Either<string, char, bool>("Galaxus"), "Galaxus");
            yield return (new Either<string, char, bool>('Y'), 'Y');
            yield return (new Either<string, char, bool>(true), true);
        }
    }
}
