package mixter.domain.message;

import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageIdTest {
    @Test
    public void whenInstanciatingTwoMessageIdThenTheyHaveDifferentIds() {
        MessageId messageId1 = new MessageId();
        MessageId messageId2 = new MessageId();

        assertThat(messageId1).isNotEqualTo(messageId2);
    }

    @Test
    public void whenDisplayingMessageAsStringThenItDisplaysId() {
        MessageId messageId = new MessageId();

        assertThat(messageId.toString()).isEqualTo(messageId.getId());
    }
}
