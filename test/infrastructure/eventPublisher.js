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
        publisher.on(EventA, function(){
            called = true;
        });

        publisher.publish(new EventA());

        expect(called).to.be.true;
    });

    it('Given different handlers When publish Then call right handler', function() {
        publisher.on(EventA, function(){
            throw new Error('Publish EventB, not EventA');
        });
        var eventBReceived = false;
        publisher.on(EventB, function(){
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

    it('Given handler on all events When publish Then handler is called for all events', function() {
        var calledNb = 0;
        publisher.onAny(function(){
            calledNb++;
        });

        publisher.publish(new EventA());
        publisher.publish(new EventB());

        expect(calledNb).to.equal(2);
    });

    it('Given several global handlers When publish Then all handlers are called', function() {
        var handler1Called = false;
        publisher.onAny(function(){
            handler1Called = true;
        });
        var handler2Called = false;
        publisher.onAny(function(){
            handler2Called = true;
        });

        publisher.publish(new EventA());

        expect(handler1Called).to.be.true;
        expect(handler2Called).to.be.true;
    });
});