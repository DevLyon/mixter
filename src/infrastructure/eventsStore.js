var _ = require('lodash');

var EventDontContainsAggregateId = exports.EventDontContainsAggregateId = function EventDontContainsAggregateId(eventName){
    this.eventName = eventName;
};

var EventsStore = function EventsStore(){
    var events = [];

    this.store = function store(event) {
        if(!event.getAggregateId){
            throw new EventDontContainsAggregateId(event.constructor.name);
        }

        events.push(event);
    };

    this.getEventsOfAggregate = function getEventsOfAggregate(aggregateId) {
        return _.filter(events, function(event) {
            return event.getAggregateId().equals(aggregateId);
        });
    };
};

exports.create = function create(){
    return new EventsStore();
};