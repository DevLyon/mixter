var userId = require('../../src/domain/userId');
var expect = require('chai').expect;

describe('UserId', function() {
    var email = 'user@mix-it.fr';

    it('When create UserId Then toString return email', function() {
        var id = new userId.UserId(email);

        expect(id.toString()).to.eql(('UserId:' + email));
    });

    it('When create UserId with empty email Then throw UserEmailCannotBeEmpty exception', function() {
        expect(function() {
            new userId.UserId("");
        }).to.throw(userId.UserEmailCannotBeEmpty);
    });
});