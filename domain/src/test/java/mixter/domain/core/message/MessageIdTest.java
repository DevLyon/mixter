package mixter.domain.core.message;

import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageIdTest {
    @Test
    public void Given2GeneratedMessageIdsWhenComparedWithEqualThenTheyShouldBeDifferent() {
        MessageId messageId = MessageId.generate();
        MessageId otherMessageId = MessageId.generate();
        assertThat(messageId).isNotEqualTo(otherMessageId);
    }

}
