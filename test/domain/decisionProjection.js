var decisionProjection = require('../../src/domain/decisionProjection');
var expect = require('chai').expect;

describe('DecisionProjection', function() {
    var EventA = function EventA(){
        this.userId = 'UserA';
    };

    var EventB = function EventB(){
        this.valueB = 'ValueB';
    };

    it('When register Event Then call action on apply of this event', function() {
        var projection = decisionProjection.create().register(EventA, function(event){
            this.isCalled = true;
        }).apply(new EventA());

        expect(projection.isCalled).is.true;
    });

    it('Given several event registered When apply Then call good handler for each event', function() {
        var projection = decisionProjection.create().register(EventA, function(event){
            this.userId = event.userId;
        }).register(EventB, function(event){
            this.valueB = event.valueB;
        }).apply([new EventA(), new EventB()]);

        expect(projection.userId).to.equal('UserA');
        expect(projection.valueB).to.equal('ValueB');
    });

    it('When apply an event not registered Then nothing', function() {
        var projection = decisionProjection.create().apply(new EventA());

        expect(projection.userId).to.be.undefined;
    });
});