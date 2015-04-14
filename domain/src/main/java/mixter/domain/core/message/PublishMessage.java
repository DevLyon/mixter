package mixter.domain.core.message;


import mixter.domain.identity.UserId;

public class PublishMessage {
    private final String message;
    private final UserId authorId;

    public PublishMessage(String message, UserId authorId) {
        this.message = message;
        this.authorId = authorId;
    }

    public String getMessage() {
        return message;
    }

    public UserId getAuthorId() {
        return authorId;
    }
}
