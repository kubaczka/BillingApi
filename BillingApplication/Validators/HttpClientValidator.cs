using FluentValidation;
using System.Runtime.Loader;

namespace BillingApplication.Validators
{
    public class HttpClientValidator : AbstractValidator<HttpClient>
    {
        public HttpClientValidator()
        {
            RuleFor(client => client.BaseAddress).NotNull().NotEmpty().Must(x => IsValidUri(x.AbsoluteUri));
            
        }
        Boolean IsValidUri(String uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.Absolute);
        }       
    }
}
