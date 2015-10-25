package mixter.domain.core.message;

import mixter.doc.Repository;

@Repository
public interface MessageRepository {
    Message getById(MessageId messageId);
}
