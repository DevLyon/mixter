var eventPublisher = require('../../src/infrastructure/eventPublisher');
var expect = require('chai').expect;

describe('EventPublisher', function() {
    var publisher;
    beforeEach(function(){
        publisher = eventPublisher.create();
    });

    var EventA = function EventA(){
        this.value = 5;
    };
    var EventB = function EventB(){ };

    it('Given handler When publish Then call handler', function() {
        var called = false;
        publisher.on(EventA, function(event){
            called = true;
        });

        publisher.publish(new EventA());

        expect(called).to.be.true;
    });

    it('Given different handlers When publish Then call right handler', function() {
        publisher.on(EventA, function(event){
            throw new Error('Publish EventB, not EventA');
        });
        var eventBReceived = false;
        publisher.on(EventB, function(event){
            eventBReceived = true;
        });

        publisher.publish(new EventB());

        expect(eventBReceived).to.be.true;
    });

    it('Given handler When publish Then pass event to action', function() {
        var eventReceived;
        publisher.on(EventA, function(event){
            eventReceived = event;
        });

        publisher.publish(new EventA());

        expect(eventReceived.value).to.equal(5);
    });
});