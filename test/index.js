var index = require('../src/index');
var expect = require('chai').expect;

describe('Runner', function() {
    it('When run Then return ok', function() {
        var result = index.run();

        expect(result).to.equal('ok');
    });
});
