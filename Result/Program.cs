using System;

namespace Result
{
    class Program
    {
        static void Main(string[] args)
        {
            var gbs1 = new TryFailWithBogus<string>("Lama lama lama lama");
            var gbs2 = new TryFailWithBogus<string>("alarm, a Lama");
            var gbb = new TryFailWithBogus<bool>(false);
            var gbguid = new TryFailWithBogus<Guid>(Guid.NewGuid());
            var jes = new TryFailWithBogus<decimal>(3.14159m);

            var qss =
                from s1 in gbs1
                from s2 in gbs2
                from b in gbb
                from id in gbguid
                from mig in jes
                select CombineValues(b, s1, s2, id, mig);
            var result = qss.Resolve(b => b.BogusReason, x => x);
            Console.WriteLine(result);
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
