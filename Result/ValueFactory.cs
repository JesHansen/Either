using System;

namespace Result
{
    public static class ValueFactory
    {
        public static Result<AnimalSafetyRules> TryGetWarning(string warningText, bool fail)
        {
            return fail
                ? new Result<AnimalSafetyRules>(new Failure { Reason = "The database field was null", Type = FailureType.NoDataPresent })
                : new Result<AnimalSafetyRules>(new AnimalSafetyRules(warningText, DangerLevel.High));
        }

        public static Result<Animal> TryGetTiger(string name, bool fail)
        {
            return fail
                ? new Result<Animal>(new Failure { Reason = $"Unfortunately {name} has passed away.", Type = FailureType.AnimalWasFoundDead })
                : new Result<Animal>(new Animal{Dangerous = Animal.DangerClass.Deadly, Name = name, PresentInCopenhagenZoo = true, WeightInKilograms = 300});
        }

        public static Result<bool> TryGetBoolean(bool b, bool fail)
        {
            return fail
                ? new Result<bool>(new Failure {Reason = "The value '¤$!½§' read from the database could not be parsed as a boolean value.", Type = FailureType.DataCorruption})
                : new Result<bool>(b);
        }

        public static Result<decimal> TryGetNumber(decimal d, bool fail)
        {
            return fail
                ? new Result<decimal>(new Failure { Reason = "The number is NaN", Type = FailureType.ArithmericalError})
                : new Result<decimal>(d);
        }

        public static Result<Guid> TryGetId(Guid id, bool fail)
        {
            return fail
                ? new Result<Guid>(new Failure { Reason = "The given user id could not be mapped to a user in the company database" , Type = FailureType.UserIsUnknown})
                : new Result<Guid>(id);
        }
    }
}
