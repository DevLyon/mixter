var UpdateFollowers = function UpdateFollowers(followersRepository){
    var self = this;

    self.register = function register(eventPublisher) {
    };
};

exports.create = function create(followersRepository){
    return new UpdateFollowers(followersRepository);
};