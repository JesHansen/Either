namespace Result
{
    public class Failure
    {
        public string Reason { get; set; }
        public FailureType Type { get; set; }
    }

    public enum FailureType
    {
        NoDataPresent,
        DataCorruption,
        ArithmericalError,
        UserIsUnknown,
        AnimalWasFoundDead
    }
}