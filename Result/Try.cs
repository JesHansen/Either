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

        public F Resolve<F>(Func<T, F> onSuccess, Func<E, F> onError)
        {
            return Failure != null ? onError(Failure) : onSuccess(Ok);
        }

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
        
        #region QuerySyntax Providers

        public Try<E, A> Select<A>(Func<T, A> map)
        {
            return Map(map);
        }

        public Try<E, A> SelectMany<A>(Func<T, Try<E, A>> mapToTry)
        {
            return FlatMap(mapToTry);
        }

        public Try<E, A> SelectMany<A, B>(Func<T, Try<E, B>> mapToTry, Func<T, B, A> reduce)
        {
            return FlatMap(mapToTry).FlatMap(t => new Try<E, A>(reduce(Ok, t)));
        }

        #endregion

        private Try<E, A> Map<A>(Func<T, A> mapSucceeded)
        {
            return Failure != null ? new Try<E, A>(Failure) : new Try<E, A>(mapSucceeded(Ok));
        }

        public Try<E, A> FlatMap<A>(Func<T, Try<E, A>> mapSucceeded)
        {
            return Failure != null ? new Try<E, A>(Failure) : mapSucceeded(Ok);
        }
    }
}
