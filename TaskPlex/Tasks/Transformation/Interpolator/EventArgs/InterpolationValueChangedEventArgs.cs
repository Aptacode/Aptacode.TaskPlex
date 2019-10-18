namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.EventArgs
{
    public class InterpolationValueChangedEventArgs<T> : InterpolationEventArgs
    {
        public InterpolationValueChangedEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}