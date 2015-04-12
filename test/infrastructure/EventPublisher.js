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
        eventPublisher.on(EventA, function(){
            called = true;
        });

        eventPublisher.publish(new EventA());

        expect(called).to.be.true;
    });

    it('Given different handlers When publish Then call right handler', function() {
        eventPublisher.on(EventA, function(){
            throw new Error('Publish EventB, not EventA');
        });
        var eventBReceived = false;
        eventPublisher.on(EventB, function(){
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

    it('Given handler on all events When publish Then handler is called for all events', function() {
        var calledNb = 0;
        eventPublisher.onAny(function(){
            calledNb++;
        });

        eventPublisher.publish(new EventA());
        eventPublisher.publish(new EventB());

        expect(calledNb).to.equal(2);
    });

    it('Given several global handlers When publish Then all handlers are called', function() {
        var handler1Called = false;
        eventPublisher.onAny(function(){
            handler1Called = true;
        });
        var handler2Called = false;
        eventPublisher.onAny(function(){
            handler2Called = true;
        });

        eventPublisher.publish(new EventA());

        expect(handler1Called).to.be.true;
        expect(handler2Called).to.be.true;
    });
});