using ListRand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ListRandTest
{
    [TestClass]
    public class ListRandomTest
    {
        private readonly Random _random = new Random();

        // этот тест, можно было бы разбить на несколько, для большей локализации потенциальной проблемы
        [TestMethod]
        public void SerializeDeserializeTest()
        {
            ListRandom[] toSerialize =
            {
                GetListRandom(0),
                GetListRandom(1),
                GetListRandom(2),
                GetListRandom(3),
                GetListRandom(25),
                GetListRandom(50),
                GetListRandom(255),
                GetListRandom(1000),
            };
            
            foreach(var list in toSerialize)
            {
                Assert.IsTrue(SerializeDeserializeTest(list));
            }
        }

        public bool SerializeDeserializeTest(ListRandom list)
        {
            var buffSizeOfElement = 255;
            var streamBuffSize = buffSizeOfElement * list.Count;
            var stream = new MemoryStream(streamBuffSize);
            list.Serialize(stream);

            stream.Position = 0;

            var list2 = new ListRandom();
            list2.Deserialize(stream);

            stream.Close();
            return list.Equals(list2);
        }

        private ListRandom GetListRandom(int countNodes)
        {
            var list = new ListRandom();
            for(int i = 0; i< countNodes; i++)
            {
                list.AddWithoutRandom(i.ToString());
            }

            list.ForEachStoppable((node, stop) =>
            {
                node.Random = list.Get(_random.Next(0, list.Count - 1));
            });

            return list;
        }
    }
}
