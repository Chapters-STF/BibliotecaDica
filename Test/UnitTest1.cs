using AutoFixture;
using System.Dynamic;

namespace Test
{
    public class UnitTest1
    {
        private readonly Fixture _fixture;

        public UnitTest1() { _fixture = new Fixture(); }

        [Fact]
        public void FxitureShouldCreate()
        {
            _fixture.Should().NotBeNull();
        }

        [Fact]
        public void MutiplicationIdentity()
        {
            int number = _fixture.Create<int>();

            (number * 1).Should().Be(number);
        }

        [Fact]
        public void AdditionIdentity()
        {
            int number = _fixture.Create<int>();

            (number + 0).Should().Be(number);
        }

        public class Foo { public int Number { get; set; } public string? String { get; set; } }

        [Fact]
        public void EquivalentIsNotEqual()
        {
            var a = _fixture.Create<Foo>();
            var b = new Foo() { Number = a.Number, String = a.String };

            //Assert.Equal(a, b); // False
            b.Should().BeEquivalentTo(a); // True
        }

        [Fact]
        public void WhenWeNeddEqual()
        {
            var a = new Foo() { Number = 1, String = "2" };
            var b = new Foo() { Number = 1, String = "2" };

            b.Should().NotBeSameAs(a);
        }

        public class Animal { public string? Species { get; set; } }
        public class Human : Animal { public string? Name { get; set; } }

        public Animal GetAnimal() => new Human { Name = "Jonh", Species = "Homo Sapiens" };

        [Fact]
        public void SometimesWeMaybeNeedCast()
        {
            var a = GetAnimal();

            a.As<Human>().Name.Should().Be("Jonh");
        }

        [Fact]
        public void NeverUsedButIsPossible()
        {
            var a = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            _fixture.Customizations.Add(new ElementsBuilder<int>(a));
            var b = _fixture.Create<int>();
            b.Should().BeOneOf(a);
        }

        [Fact]
        public void BeCreative()
        {
            var obj = new Object();

            obj.Should().Match(d => (d.ToString() == "System.Object"));
        }
    }
}