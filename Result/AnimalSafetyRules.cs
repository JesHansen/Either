namespace Result
{
    public class AnimalSafetyRules
    {

        public AnimalSafetyRules(string safetyRules, DangerLevel level)
        {
            SafetyRules = safetyRules;
            CautionLevel = level;
        }

        public string SafetyRules { get; set; }

        public DangerLevel CautionLevel { get; set; }
    }
}
