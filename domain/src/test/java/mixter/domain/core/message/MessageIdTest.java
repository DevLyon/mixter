package mixter.domain.core.message;

import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageIdTest {
    public static final String VALUE = "Value";

    @Test
    public void Given2GeneratedMessageIdsWhenComparedWithEqualThenTheyShouldBeDifferent() {
        MessageId messageId = MessageId.generate();
        MessageId otherMessageId = MessageId.generate();
        assertThat(messageId).isNotEqualTo(otherMessageId);
    }

    @Test
    public void Given2MessageIdCreatedFromTheSameStringWhenComparedWithEqualsThenTheyShouldBeEqual() {
        MessageId messageId = new MessageId(VALUE);
        MessageId otherMessageId = new MessageId(VALUE);
        assertThat(messageId).isEqualTo(otherMessageId);
    }

}
