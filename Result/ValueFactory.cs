using System;

namespace Result
{
    public static class ValueFactory
    {
        public static Result<string> TryGetString(string s, bool fail)
        {
            return fail
                ? new Result<string>(new Failure {Reason = "No string was generated", Type = FailureType.StringNotFound})
                : new Result<string>(s);
        }

        public static Result<bool> TryGetBoolean(bool b, bool fail)
        {
            return fail
                ? new Result<bool>(new Failure {Reason = "The boolean value is missing", Type = FailureType.BooleanNotFound})
                : new Result<bool>(b);
        }

        public static Result<decimal> TryGetNumber(decimal d, bool fail)
        {
            return fail
                ? new Result<decimal>(new Failure { Reason = "The number was not present.", Type = FailureType.DecimalNotFound})
                : new Result<decimal>(d);
        }

        public static Result<Guid> TryGetId(Guid id, bool fail)
        {
            return fail
                ? new Result<Guid>(new Failure { Reason = "The ID was not found." , Type = FailureType.GuidNotFound})
                : new Result<Guid>(id);
        }
    }
}
