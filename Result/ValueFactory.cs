using System;

namespace Result
{
    public static class ValueFactory
    {
        public static TryFailWithBogus<string> TryGetString(string s, bool fail)
        {
            return fail
                ? new TryFailWithBogus<string>(new Bogus {BogusReason = "No string was generated"})
                : new TryFailWithBogus<string>(s);
        }

        public static TryFailWithBogus<bool> TryGetBoolean(bool b, bool fail)
        {
            return fail
                ? new TryFailWithBogus<bool>(new Bogus {BogusReason = "The boolean value is missing"})
                : new TryFailWithBogus<bool>(b);
        }

        public static TryFailWithBogus<decimal> TryGetNumber(decimal d, bool fail)
        {
            return fail
                ? new TryFailWithBogus<decimal>(new Bogus { BogusReason = "The number was not present." })
                : new TryFailWithBogus<decimal>(d);
        }

        public static TryFailWithBogus<Guid> TryGetId(Guid id, bool fail)
        {
            return fail
                ? new TryFailWithBogus<Guid>(new Bogus { BogusReason = "The ID was not found." })
                : new TryFailWithBogus<Guid>(id);
        }
    }
}
