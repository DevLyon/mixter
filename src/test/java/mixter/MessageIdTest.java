package mixter;

import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class MessageIdTest {
    @Test
    public void whenInstanciatingTwoMessageIdThenTheyHaveDifferentIds() {
        Message.MessageId messageId1 = new Message.MessageId();
        Message.MessageId messageId2 = new Message.MessageId();

        assertThat(messageId1).isNotEqualTo(messageId2);
    }

    @Test
    public void whenDisplayingMessageAsStringThenItDisplaysId() {
        Message.MessageId messageId = new Message.MessageId();

        assertThat(messageId.toString()).isEqualTo(messageId.getId());
    }
}
