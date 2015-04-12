var events = require('events');

var EventPublisher = function EventPublisher(){
    var self = this;

    var eventEmitter = new events.EventEmitter();

    self.on = function on(eventType, action){
        eventEmitter.on(eventType.name, action);

        return self;
    };

    self.onAny = function onAny(action){
        eventEmitter.on('*', action);

        return self;
    };

    self.publish = function publish(event){
        eventEmitter.emit('*', event);

        var eventName = event.constructor.name;
        eventEmitter.emit(eventName, event);
    };
};

exports.create = function create(){
    return new EventPublisher();
};