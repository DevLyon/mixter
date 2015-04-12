var DecisionProjection = function DecisionProjection(){
    var self = this;

    var handlers = {};

    self.register = function register(eventType, action) {
        handlers[eventType.name] = action;

        return self;
    };

    self.apply = function apply(events) {
        if(events instanceof Array) {
            events.forEach(self.apply);
            return self;
        }

        var event = events;
        var typeName = event.constructor.name;

        var handler = handlers[typeName];
        if(handler){
            handler.call(self, event);
        }

        return self;
    };
};

exports.create = function create() {
    return new DecisionProjection();
};