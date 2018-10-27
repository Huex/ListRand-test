using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ListRand
{
    public class ListRandom : IEquatable<ListRandom>
    {
        public ListNode Head = null;
        public ListNode Tail = null;
        public int Count = 0;

        public void Serialize(Stream stream)
        {
            ListNode[] nodes = ToArray();
            var writer = new StreamWriter(stream);
            foreach (var node in nodes)
            {
                int indexOfRandom = Array.IndexOf(nodes, node.Random);
                writer.WriteLine(node.Data);
                writer.WriteLine(indexOfRandom.ToString());
            }
            writer.Flush();
        }

        public void Deserialize(Stream stream)
        {
            var line = string.Empty;
            var IndexesOfRandom = new List<int>();

            var reader = new StreamReader(stream);

            while ((line = reader.ReadLine()) != null)
            {
                AddWithoutRandom(line);
                IndexesOfRandom.Add(int.Parse(reader.ReadLine()));
            }

            int index = 0;
            ForEachStoppable((node) =>
            {
                node.Random = Get(IndexesOfRandom[index]);
                index++;
                return false;
            });
        }

        public ListNode[] ToArray()
        {
            ListNode[] nodes = new ListNode[Count];
            int nodeIndex = 0;

            ForEachStoppable((node) =>
            {
                nodes[nodeIndex] = node;
                nodeIndex++;
                return false;
            });

            return nodes;
        }

        public void ForEachStoppable(Func<ListNode, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var current = Head;
            while (current != null)
            {
                if (action.Invoke(current))
                {
                    return;
                }
                current = current.Next;
            }
        }

        public void AddWithoutRandom(string data)
        {
            var node = new ListNode()
            {
                Data = data,
                Next = null
            };

            if (Count == 0)
            {
                Head = node;
            }
            else
            {
                Tail.Next = node;
            }
        
            node.Previous = Tail;
            Tail = node;
            Count++;
        }

        public ListNode Get(int index)
        {
            if(index > Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("Index was outside the bounds of the ListRandom");
            }

            ListNode resNode = null;
            int nodeIndex = 0;

            ForEachStoppable((node) =>
            {
                bool stop = false;
                if(nodeIndex == index)
                {
                    resNode = node;
                    stop = true;
                }
                nodeIndex++;
                return stop;
            });

            return resNode;
        }

        public bool Equals(ListRandom other)
        {
            if(Count != other.Count)
            {
                return false;
            }

            int index = 0;
            bool result = true;
            ForEachStoppable((node) =>
            {
                bool stop = false;
                var otherNode = other.Get(index);
                if (otherNode.Data != node.Data || 
                    otherNode.Random.Data != node.Random.Data)
                {
                    stop = true;
                    result = false;
                }
                index++;
                return stop;
            });

            return result;
        }
    }
}
