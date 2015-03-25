namespace Mixter.Domain
{
    public class MessageDecisionProjection
    {
        public MessageDecisionProjection(MessagePublished evt)
        {
            Mute(evt);
        }

        public MessageId Id { get; private set; }

        private void Mute(MessagePublished evt)
        {
            Id = evt.Id;
        }
    }
}