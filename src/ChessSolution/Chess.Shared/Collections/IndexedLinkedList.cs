using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Shared.Collections
{
    public class IndexedLinkedList<TKey, TItem> : IEnumerable<TItem>
    {
        private readonly Func<TItem, TKey> keyProvider;

        private readonly Dictionary<TKey, LinkedListNode<TItem>> nodes;
        private readonly LinkedList<TItem> items;

        public IndexedLinkedList(Func<TItem, TKey> keyProvider) :
            this(keyProvider, Enumerable.Empty<TItem>())
        {
           
        }

        public IndexedLinkedList(Func<TItem, TKey> keyProvider, IEnumerable<TItem> initialItems)
        {
            this.keyProvider = keyProvider;

            nodes = new Dictionary<TKey, LinkedListNode<TItem>>();
            items = new LinkedList<TItem>();

            AddRange(initialItems);
        }

        public int Count => items.Count;

        public TItem Last => items.Last.Value;

        public void AddRange(IEnumerable<TItem> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Add(TItem item)
        {
            var newNode = items.AddLast(item);

            var key = keyProvider(item);
            nodes.Add(key, newNode);
        }

        public TItem GetValue(TKey key)
        {
            return nodes[key].Value;
        }

        public bool TryGetValue(TKey key, out TItem value)
        {
            value = default;

            var foundValue = nodes.TryGetValue(key, out LinkedListNode<TItem> node);

            if (foundValue)
                value = node.Value;

            return foundValue;
        }

        public void Remove(TKey key)
        {
            var nodeToRemvoe = nodes[key];

            items.Remove(nodeToRemvoe);
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
