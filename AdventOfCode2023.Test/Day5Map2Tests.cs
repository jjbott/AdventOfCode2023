global using Assert = NUnit.Framework.Legacy.ClassicAssert;
using NUnit.Framework;

namespace AdventOfCode2023.Test
{
    public class Day5Map2Tests
    {
        [Test]
        public void NoMap() {
            var map = new Day5.Day5Map2("a", "b", new List<Day5.Day5Map> { new Day5.Day5Map(10, 10, 1) });
            var mapped = map.Lookup2(20, 3);

            Assert.AreEqual(1, mapped.Count);
            Assert.AreEqual(20, mapped[0].Item1);
            Assert.AreEqual(3, mapped[0].Item2);
        }

        [Test]
        public void NoMap_2()
        {
            var map = new Day5.Day5Map2("a", "b", new List<Day5.Day5Map> { new Day5.Day5Map(10, 10, 1) });
            var mapped = map.Lookup2(2, 3);

            Assert.AreEqual(1, mapped.Count);
            Assert.AreEqual(2, mapped[0].Item1);
            Assert.AreEqual(3, mapped[0].Item2);
        }

        [Test]
        public void FullyCoveredMap_1()
        {
            var map = new Day5.Day5Map2("a", "b", new List<Day5.Day5Map> { new Day5.Day5Map(10, 10, 5) });
            var mapped = map.Lookup2(12, 2);

            Assert.AreEqual(1, mapped.Count);
            Assert.AreEqual(12, mapped[0].Item1);
            Assert.AreEqual(2, mapped[0].Item2);
        }

        [Test]
        public void Enclosed_1()
        {
            var map = new Day5.Day5Map2("a", "b", new List<Day5.Day5Map> { new Day5.Day5Map(10, 10, 5) });
            var mapped = map.Lookup2(5, 15);

            Assert.AreEqual(3, mapped.Count);

            Assert.AreEqual(5, mapped[0].Item1);
            Assert.AreEqual(5, mapped[0].Item2);

            Assert.AreEqual(10, mapped[1].Item1);
            Assert.AreEqual(5, mapped[1].Item2);

            Assert.AreEqual(15, mapped[2].Item1);
            Assert.AreEqual(5, mapped[2].Item2);
        }
    }
}
