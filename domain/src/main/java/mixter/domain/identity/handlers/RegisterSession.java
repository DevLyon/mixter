package mixter.domain.identity.handlers;

import mixter.domain.identity.SessionProjection;
import mixter.domain.identity.SessionStatus;
import mixter.domain.identity.events.UserConnected;
import mixter.domain.identity.events.UserDisconnected;

public class RegisterSession {
    private SessionProjectionRepository sessionProjectionRepository;

    public RegisterSession(SessionProjectionRepository sessionProjectionRepository) {
        this.sessionProjectionRepository = sessionProjectionRepository;
    }

    public void apply(UserConnected event) {
        sessionProjectionRepository.save(new SessionProjection(event.getSessionId(), event.getUserId(), SessionStatus.CONNECTED));
    }

    public void apply(UserDisconnected event) {
        sessionProjectionRepository.replaceBy(new SessionProjection(event.getSessionId(), event.getUserId(), SessionStatus.DISCONNECTED));
    }
}
