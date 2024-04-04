namespace Tulip.Services.Implementations
{
    [Serializable]
    public class SAPException : Exception 
    {
        public SAPException() { }

        public SAPException(string message)
            : base(message) { }

        public SAPException(string message, Exception inner)
            : base(message, inner) { }
    }
}