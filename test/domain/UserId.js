var UserId = require('../../src/domain/UserId');
var expect = require('chai').expect;

describe('UserId', function() {
    var email = 'user@mix-it.fr';

    it('When create UserId Then toString return email', function() {
        var id = new UserId.UserId(email);

        expect(id.toString()).to.eql(('UserId:' + email));
    });

    it('When create UserId with empty email Then throw UserEmailCannotBeEmpty exception', function() {
        expect(function() {
            new UserId.UserId("");
        }).to.throw(UserId.UserEmailCannotBeEmpty);
    });
});