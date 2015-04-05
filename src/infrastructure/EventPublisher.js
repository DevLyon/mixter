var events = require('events');

var EventPublisher = function EventPublisher(){
    var self = this;

    var eventEmitter = new events.EventEmitter();

    self.on = function on(eventType, action){
        eventEmitter.on(eventType.name, action);
    };

    self.publish = function publish(event){
        var eventName = event.constructor.name;
        eventEmitter.emit(eventName, event);
    };
};

exports.create = function create(){
    return new EventPublisher();
};