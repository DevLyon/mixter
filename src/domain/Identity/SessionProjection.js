exports.SessionEnabled = true;
exports.SessionDisabled = false;

var SessionProjection = function SessionProjection(sessionId, userId, isEnabled){
    this.sessionId = sessionId;
    this.userId = userId;
    this.isEnabled = isEnabled;
};

exports.create = function create(sessionId, userId, isEnabled){
    return new SessionProjection(sessionId, userId, isEnabled);
};