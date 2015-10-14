using Mixter.Domain;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.Extensions;
using Nancy.Responses.Negotiation;

namespace Mixter.Web
{
    public class ExceptionStatusCodeHandler : IStatusCodeHandler
    {
        private readonly IResponseNegotiator _responseNegotiator;

        public ExceptionStatusCodeHandler(IResponseNegotiator responseNegotiator)
        {
            _responseNegotiator = responseNegotiator;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            var exception = context.GetException();
            return exception != null;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            context.NegotiationContext = new NegotiationContext();

            var negotiator = new Negotiator(context);

            var exception = context.GetException();
            if (exception.InnerException is DomainException)
            {
                var domainException = exception.InnerException;

                negotiator = negotiator
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(new
                    {
                        errorName = domainException.GetType().Name,
                        error = domainException.Message
                    });
            }
            else
            {
                negotiator = negotiator
                    .WithStatusCode(HttpStatusCode.InternalServerError)
                    .WithModel(new
                    {
                        errorName = "InternalServerError",
                        error = "InternalServerError"
                    });
            }

            context.Response = _responseNegotiator.NegotiateResponse(negotiator, context);
        }
    }
}