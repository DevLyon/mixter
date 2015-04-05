var idGenerator = require('../src/idGenerator');
var expect = require('chai').expect;

describe('idGenerator', function() {
    it('When generate several id Then return always different id', function() {
        var id1 = idGenerator.generate();
        var id2 = idGenerator.generate();

        expect(id1).not.to.equal(id2);
    });
});