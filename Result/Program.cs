using System;
using System.Runtime.InteropServices;

namespace Result
{
    class Program
    {
        static void Main(string[] args)
        {
            TryFailWithBogus<string>    animal = ValueFactory.TryGetString("Lama lama lama lama", false);
            TryFailWithBogus<string>   warning = ValueFactory.TryGetString("alarm, a Lama", false);
            TryFailWithBogus<bool>    doSwitch = ValueFactory.TryGetBoolean(false, false);
            TryFailWithBogus<Guid>      userId = ValueFactory.TryGetId(Guid.NewGuid(), false);
            TryFailWithBogus<decimal> constant = ValueFactory.TryGetNumber(3.14159m, false);

            var combine =
                from song in animal
                from text in warning
                from shouldToggle in doSwitch
                from user in userId
                from pi in constant
                select CombineValues(shouldToggle, song, text, user, pi);
            var total = combine.Resolve(b => b.BogusReason, s => s);
            combine.Resolve(b => Console.WriteLine(b.BogusReason), Console.WriteLine);
            Console.WriteLine(total);
        }

        static string CombineValues(bool printStringLengths, string lamaString, string alarmString, Guid id, decimal pi)
        {
            if (printStringLengths)
            {
                return (lamaString.Length + alarmString.Length).ToString();
            }
            return id + $" {pi}";
        }
    }
}
