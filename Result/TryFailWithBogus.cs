namespace Result
{
    class TryFailWithBogus<T> : Try<Bogus, T>
    {
        public TryFailWithBogus(Bogus b) : base(b)
        {
            
        }

        public TryFailWithBogus(T item): base(item)
        {
            
        }
    }
}