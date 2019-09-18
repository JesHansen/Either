using System;

namespace Result
{
    public class Try<E, T> where E: class
    {
        private E Failure { get; }
        private T Ok { get; }

        protected Try(T ok)
        {
            Ok = ok;
        }

        protected Try(E failure)
        {
            Failure = failure;
        }

        public F Resolve<F>(Func<E, F> onError, Func<T, F> onSuccess)
        {
            return Failure != null ? onError(Failure) : onSuccess(Ok);
        }

        #region Extra query methods

        public void Resolve(Action<E> onError, Action<T> onSuccess)
        {
            if (Failure != null)
            {
                onError(Failure);
            }
            else
            {
                onSuccess(Ok);
            }
        }
        
        private Try<E, A> Map<A>(Func<T, A> mapSucceeded)
        {
            return Failure != null ? new Try<E, A>(Failure) : new Try<E, A>(mapSucceeded(Ok));
        }
        
        public Try<E, A> Select<A>(Func<T, A> map)
        {
            return Map(map);
        }

        public Try<E, A> SelectMany<A>(Func<T, Try<E, A>> mapToTry)
        {
            return FlatMap(mapToTry);
        }
        #endregion

        public Try<E, A> SelectMany<A, B>(Func<T, Try<E, B>> mapToTry, Func<T, B, A> reduce)
        {
            return FlatMap(mapToTry).FlatMap(t => new Try<E, A>(reduce(Ok, t)));
        }

        private Try<E, A> FlatMap<A>(Func<T, Try<E, A>> mapSucceeded)
        {
            return Failure != null ? new Try<E, A>(Failure) : mapSucceeded(Ok);
        }
    }
}
