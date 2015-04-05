var EventPublisher = require('../../src/infrastructure/EventPublisher');
var expect = require('chai').expect;

describe('EventPublisher', function() {
    var eventPublisher;
    beforeEach(function(){
        eventPublisher = EventPublisher.create();
    });

    var EventA = function EventA(){
        this.value = 5;
    };
    var EventB = function EventB(){ };

    it('Given handler When publish Then call handler', function() {
        var called = false;
        eventPublisher.on(EventA, function(event){
            called = true;
        });

        eventPublisher.publish(new EventA());

        expect(called).to.be.true;
    });

    it('Given different handlers When publish Then call right handler', function() {
        eventPublisher.on(EventA, function(event){
            throw new Error('Publish EventB, not EventA');
        });
        var eventBReceived = false;
        eventPublisher.on(EventB, function(event){
            eventBReceived = true;
        });

        eventPublisher.publish(new EventB());

        expect(eventBReceived).to.be.true;
    });

    it('Given handler When publish Then pass event to action', function() {
        var eventReceived;
        eventPublisher.on(EventA, function(event){
            eventReceived = event;
        });

        eventPublisher.publish(new EventA());

        expect(eventReceived.value).to.equal(5);
    });
});