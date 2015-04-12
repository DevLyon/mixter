exports.SessionEnabled = true;
exports.SessionDisabled = false;

var SessionProjection = function SessionProjection(sessionId, userId, state){
    this.sessionId = sessionId;
    this.userId = userId;
    this.state = state;
};

exports.create = function create(sessionId, userId, state){
    return new SessionProjection(sessionId, userId, state);
};