using System;

namespace Aptacode.TaskPlex.Tests.OldTests.Utilites
{
    public class ValueUpdateArgs<T> : EventArgs
    {
        public ValueUpdateArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue { get; set; }
        public T NewValue { get; set; }
    }
}