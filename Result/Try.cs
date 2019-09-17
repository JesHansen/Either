using System;

namespace Result
{
    public class Try<E, T>
    {
        private E Failure { get; }
        private T Ok { get; }

        public Try(T ok)
        {
            Ok = ok;
        }

        public Try(E failure)
        {
            Failure = failure;
        }

        public F Resolve<F>(Func<E, F> onError, Func<T, F> onSuccess)
        {
            return Failure != null ? onError(Failure) : onSuccess(Ok);
        }

        private Try<E, A> Map<A>(Func<T, A> mapSucceeded)
        {
            return Failure != null ? new Try<E, A>(Failure) : new Try<E, A>(mapSucceeded(Ok));
        }

        private Try<E, A> FlatMap<A>(Func<T, Try<E, A>> mapSucceeded)
        {
            return Failure != null ? new Try<E, A>(Failure) : mapSucceeded(Ok);
        }

        public Try<E, A> Select<A>(Func<T, A> map)
        {
            return Map(map);
        }

        public Try<E, A> SelectMany<B, A>(Func<T, Try<E, B>> mapToTry, Func<T, B, A> reduce)
        {
            return FlatMap(mapToTry).FlatMap(t => new Try<E, A>(reduce(Ok, t)));
        }
    }
}
