package mixter.domain.identity.handlers;

import mixter.domain.identity.SessionProjection;
import mixter.domain.identity.SessionStatus;
import mixter.domain.identity.events.UserConnected;

public class RegisterSession {
    private SessionProjectionRepository sessionProjectionRepository;

    public RegisterSession(SessionProjectionRepository sessionProjectionRepository) {
        this.sessionProjectionRepository = sessionProjectionRepository;
    }

    public void apply(UserConnected event) {
        sessionProjectionRepository.save(new SessionProjection(event.getSessionId(), event.getUserId(), SessionStatus.CONNECTED));
    }
}
