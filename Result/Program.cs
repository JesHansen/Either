using System;
using System.Text;

namespace Result
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Result<Animal> animal = ValueFactory.TryGetTiger("Cuddles", false);
            Result<AnimalSafetyRules> safetyRules = ValueFactory.TryGetWarning("it is an Asian Tiger", false);
            Result<bool> eatsMeat = ValueFactory.TryGetBoolean(true, false);
            Result<Guid> userId = ValueFactory.TryGetId(Guid.NewGuid(), false);
            Result<decimal> admissionPrice = ValueFactory.TryGetNumber(31.41m, false);

            BruteForceValues(animal, safetyRules, eatsMeat, userId, admissionPrice);
            RefactorBruteForceValues(animal, safetyRules, eatsMeat, userId, admissionPrice);
            SafelyAccessValues(animal, safetyRules, eatsMeat, userId, admissionPrice);
        }

        static string BusinessLogic(Animal animal, AnimalSafetyRules animalAnimalSafetyRules, bool carnivore, Guid userId, decimal ticketPrice)
        {
            var sb = new StringBuilder();
            var glue = carnivore ? "is" : "is not";

            sb.AppendLine($"*** Animal Facts about {animal.Name} ***");
            sb.AppendLine($"A word of caution. This animal is classed as Danger Level '{animalAnimalSafetyRules.CautionLevel}', because {animalAnimalSafetyRules.SafetyRules}.");
            sb.AppendLine($"It {glue} a carnivorous animal.");
            sb.AppendLine($"Information about {animal.Name}, such as its wight, {animal.WeightInKilograms} kg, was last updated by user id {userId}.");
            sb.AppendLine($"Ticket price to see {animal.Name}: kr. {ticketPrice}");

            return sb.ToString();
        }

        private static void BruteForceValues(
            Result<Animal> probablyAnAnimal,
            Result<AnimalSafetyRules> probablyRules,
            Result<bool> probablyDietInfo,
            Result<Guid> probablyuserId,
            Result<decimal> probablyTicketPrice)
        {
            bool somethingFailed = false;

            Animal animal = probablyAnAnimal.Resolve(x => x, e => { somethingFailed = true; return new Animal { Dangerous = Animal.DangerClass.Undefined, Name = "Dummy" }; });
            if (!somethingFailed)
            {
                AnimalSafetyRules rules = probablyRules.Resolve(x => x, e => { somethingFailed = true; return new AnimalSafetyRules("Dummy", AnimalSafetyRules.DangerLevel.Benign); });
                if (!somethingFailed)
                {
                    bool isCarnivore = probablyDietInfo.Resolve(x => x, e => { somethingFailed = true; return false; });
                    if (!somethingFailed)
                    {
                        Guid g = probablyuserId.Resolve(x => x, e => { somethingFailed = true; return Guid.Empty; });
                        if (!somethingFailed)
                        {
                            decimal price = probablyTicketPrice.Resolve(x => x, e => { somethingFailed = true; return -1m; });
                            if (!somethingFailed)
                            {
                                var result = BusinessLogic(animal, rules, isCarnivore, g, price);
                                string summary = $"All went well!\n\n{result}";
                                Console.WriteLine(summary);
                            }
                            else
                            {
                                Console.WriteLine("Calculation failed: Unable to communicate with invoice and ticket systems.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Calculation failed: User not located.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Calculation failed: Carnivore status undetermined.");
                    }
                }
                else
                {
                    Console.WriteLine("Calculation failed: Safety procedure rules not located.");
                }
            }
            else
            {
                Console.WriteLine("Calculation failed: Retrievel of the animal failed.");
            }
        }

        private static void RefactorBruteForceValues(
            Result<Animal> probablyAnAnimal,
            Result<AnimalSafetyRules> probablyRules,
            Result<bool> probablyDietInfo,
            Result<Guid> probablyuserId,
            Result<decimal> probablyTicketPrice)
        {
            bool somethingFailed = false;

            Animal animal = probablyAnAnimal.Resolve(x => x, e => { somethingFailed = true; return new Animal { Dangerous = Animal.DangerClass.Undefined, Name = "Dummy" }; });
            if (somethingFailed)
            {
                Console.WriteLine("Calculation failed: Retrievel of the animal failed.");
                return;
            }

            AnimalSafetyRules rules = probablyRules.Resolve(x => x, e => { somethingFailed = true; return new AnimalSafetyRules("Dummy", AnimalSafetyRules.DangerLevel.Benign); });
            if (somethingFailed)
            {
                Console.WriteLine("Calculation failed: Safety procedure rules not located.");
                return;
            }

            bool isCarnivore = probablyDietInfo.Resolve(x => x, e => { somethingFailed = true; return false; });
            if (somethingFailed)
            {
                Console.WriteLine("Calculation failed: Carnivore status undetermined.");
                return;
            }

            Guid g = probablyuserId.Resolve(x => x, e => { somethingFailed = true; return Guid.Empty; });
            if (somethingFailed)
            {
                Console.WriteLine("Calculation failed: User not located.");
                return;
            }

            decimal price = probablyTicketPrice.Resolve(x => x, e => { somethingFailed = true; return -1m; });
            if (somethingFailed)
            {
                Console.WriteLine("Calculation failed: Unable to communicate with invoice and ticket systems.");
                return;
            }

            var result = BusinessLogic(animal, rules, isCarnivore, g, price);
            string summary = $"All went well!\n\n{result}";
            Console.WriteLine(summary);
        }



        private static void SafelyAccessValues(
            Result<Animal> probablyAnAnimal,
            Result<AnimalSafetyRules> probablyRules,
            Result<bool> probablyDietInfo,
            Result<Guid> probablyuserId,
            Result<decimal> probablyTicketPrice)
        {
            (bool calculationFailed, string result) outcome;

            #region impl, ROP and SelectMany

            var combinedValuesOrError =
                from a in probablyAnAnimal
                from w in probablyRules
                from t in probablyDietInfo
                from u in probablyuserId
                from c in probablyTicketPrice
                select BusinessLogic(a, w, t, u, c);
            outcome = combinedValuesOrError.Resolve(x => (false, x), ParseError);

            #endregion

            #region alternative impl

            //var combinedValuesOrError =
            //  probablyAnAnimal.FlatMap(a =>
            //  probablyRules.FlatMap(w =>
            //  probablyDietInfo.FlatMap(t =>
            //  probablyuserId.FlatMap(u =>
            //  probablyTicketPrice.FlatMap(c =>
            //  new Result<string>(BusinessLogic(a, w, t, u, c)))))));
            //outcome = combinedValuesOrError.Resolve(x => (false, x), ParseError);

            #endregion

            var header = outcome.calculationFailed ? "Could not complete the calculation" : "All went well!";
            var summary = $"{header}\n\n{outcome.result}";
            Console.WriteLine(summary);
        }

        private static (bool failed, string message) ParseError(Failure error)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"An error of type '{error.Type}' occured. The reason for the error was given as: '{error.Reason}'");
            return (true, sb.ToString());
        }
    }
}
