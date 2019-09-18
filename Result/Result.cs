namespace Result
{
    public class Result<T> : Try<Failure, T>
    {
        public Result(Failure b) : base(b)
        {
            
        }

        public Result(T item): base(item)
        {
            
        }
    }
}