using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses.Negotiation;

namespace Mixter.Web
{
    public class NotFoundStatusCodeHandler : IStatusCodeHandler
    {
        private readonly IResponseNegotiator _responseNegotiator;

        public NotFoundStatusCodeHandler(IResponseNegotiator responseNegotiator)
        {
            _responseNegotiator = responseNegotiator;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            context.NegotiationContext = new NegotiationContext();

            var negotiator = new Negotiator(context)
                .WithStatusCode(HttpStatusCode.NotFound);

            context.Response = _responseNegotiator.NegotiateResponse(negotiator, context);
        }
    }
}