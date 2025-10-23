// ReSharper disable once CheckNamespace
namespace ClearBank.DeveloperTest.Types
{
    // Not made immutable in case it is the DTO for a web service.
    
    public class MakePaymentResult
    {
        public bool Success { get; set; }
        
        public static MakePaymentResult ForSuccess() => new() { Success = true };
        
        public static MakePaymentResult ForFailure() => new() { Success = false };
    }
}
