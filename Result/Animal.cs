namespace Result
{
    public class Animal
    {
        public DangerClass Dangerous { get; set; }
        public string Name { get; set; }
        public bool PresentInCopenhagenZoo { get; set; }
        public double WeightInKilograms { get; set; }

        public enum DangerClass
        {
            Undefined,
            ChildrenFriendly,
            Tame,
            CautionAdvised,
            Dangerous,
            Deadly
        }
        
    }
}
