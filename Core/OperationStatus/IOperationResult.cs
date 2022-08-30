namespace Core
{
    public interface IOperationResult<T>
    {
        public T? Result { get; set; }
        public Status Status { get; set; }
        public string Message { get; }
    }
}
