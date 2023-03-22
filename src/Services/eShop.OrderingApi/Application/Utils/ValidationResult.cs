namespace eShop.OrderingApi.Application.Utils;

public class ValidationResult
{
    public ValidationResult()
    {
        Validations = new List<Validation>();
    }

    public bool IsValid => Validations.Count == 0;
    public bool IsInvalid => !IsValid;
    public List<Validation> Validations { get; set; }

    public void AddValidation(string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

        Validations.Add(new Validation()
        {
            Message = message
        });
    }

    public void AddValidation(string property, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

        Validations.Add(new Validation()
        {
            Property = property,
            Message = message
        });
    }

    public class Validation
    {
        public string? Property { get; set; }
        public string Message { get; set; }
    }
}
