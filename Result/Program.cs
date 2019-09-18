using System;
using System.Text;

namespace Result
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            ResultValues();

            // Se også https://fsharpforfunandprofit.com/rop/
        }

        private static void ResultValues()
        {
            Result<string> animal = ValueFactory.TryGetString("Lama, lama, lama, lama", false);
            Result<string> warning = ValueFactory.TryGetString("Alarm, a Lama!", false);
            Result<bool> toggle = ValueFactory.TryGetBoolean(false, false);
            Result<Guid> userId = ValueFactory.TryGetId(Guid.NewGuid(), true);
            Result<decimal> constant = ValueFactory.TryGetNumber(3.14159m, false);

            CombineResultValues(animal, warning, toggle, userId, constant);
        }

        static string BusinessLogic(bool printStringLengths, string animalDescription, string animalWarning, Guid userId, decimal number)
        {
            if (printStringLengths)
            {
                return (animalDescription.Length + animalWarning.Length).ToString();
            }
            return userId + $" {number}";
        }

        private static void CombineResultValues(Result<string> animal, Result<string> warning, Result<bool> toggle, Result<Guid> userId, Result<decimal> constant)
        {
            var (somethingFailed, result) = ForceOutValues(animal, warning, toggle, userId, constant);

            var outcome = somethingFailed ? "Could not complete the calculation" : "All went well";
            string summary = $"{outcome}: {result}";
            Console.WriteLine(summary);

            var (failed, answer) = UseQuerySyntax(animal, warning, toggle, userId, constant);
            outcome = failed ? "Could not complete the calculation" : "All went well";
            summary = $"{outcome}: {answer}";
            Console.WriteLine(summary);
        }

        private static (bool failed, string result) ForceOutValues(Result<string> animal, Result<string> warning, Result<bool> toggle, Result<Guid> userId, Result<decimal> constant)
        {
            bool failed = false;
            string a = animal.Resolve(e => { failed = true; return e.Reason; }, x => x);
            if (failed)
                return (true, a);

            string w = warning.Resolve(e => { failed = true; return e.Reason; }, x => x);
            if (failed)
                return (true, w);

            bool t = toggle.Resolve(e => { failed = true; return false; }, x => x);
            if (failed)
                return (true, "No value for toggle.");

            Guid g = userId.Resolve(e => { failed = true; return Guid.Empty; }, x => x);
            if (failed)
                return (true, "No value for userId.");

            decimal num = constant.Resolve(e => { failed = true; return -1m; }, x => x);
            if (failed)
                return (true, "No value for constant.");

            var result = BusinessLogic(t, a, w, g, num);
            Console.WriteLine(result);
            return (false, result);
        }

        private static (bool failed, string result) UseQuerySyntax(Result<string> animal, Result<string> warning, Result<bool> toggle, Result<Guid> userId, Result<decimal> constant)
        {
            var combinedValuesOrError =
                from a in animal
                from w in warning
                from t in toggle
                from u in userId
                from c in constant
                select BusinessLogic(t, a, w, u, c);

            return combinedValuesOrError.Resolve(ParseError, s => (false, message: s));
        }

        private static (bool failed, string message) ParseError(Failure error)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"An error of type '{error.Type}' occured.");
            sb.AppendLine();
            sb.AppendLine($"The reason for the error was given as: '{error.Reason}'");
            return (true, sb.ToString());
        }
    }
}
