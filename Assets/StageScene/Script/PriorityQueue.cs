using System;
using System.Collections.Generic;

// Unity 6で標準が見つからない場合、このクラスが代わりを務めます
public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement Element, TPriority Priority)> nodes = new();

    public int Count => nodes.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        nodes.Add((element, priority));
        int i = nodes.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (nodes[i].Priority.CompareTo(nodes[parent].Priority) >= 0) break;
            (nodes[i], nodes[parent]) = (nodes[parent], nodes[i]);
            i = parent;
        }
    }

    public TElement Dequeue() => TryDequeue(out TElement element, out _) ? element : throw new InvalidOperationException("Queue is empty");

    public bool TryDequeue(out TElement element, out TPriority priority)
    {
        if (nodes.Count == 0)
        {
            element = default;
            priority = default;
            return false;
        }

        element = nodes[0].Element;
        priority = nodes[0].Priority;
        nodes[0] = nodes[nodes.Count - 1];
        nodes.RemoveAt(nodes.Count - 1);

        int i = 0;
        while (true)
        {
            int left = i * 2 + 1;
            int right = i * 2 + 2;
            int smallest = i;

            if (left < nodes.Count && nodes[left].Priority.CompareTo(nodes[smallest].Priority) < 0) smallest = left;
            if (right < nodes.Count && nodes[right].Priority.CompareTo(nodes[smallest].Priority) < 0) smallest = right;

            if (smallest == i) break;
            (nodes[i], nodes[smallest]) = (nodes[smallest], nodes[i]);
            i = smallest;
        }
        return true;
    }

    public void Clear() => nodes.Clear();
}