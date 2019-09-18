namespace Result
{
    public class Failure
    {
        public string Reason { get; set; }
        public FailureType Type { get; set; }
    }

    public enum FailureType
    {
        StringNotFound,
        BooleanNotFound,
        DecimalNotFound,
        GuidNotFound,
    }
}