var eventsStoreModule = require('../../src/infrastructure/eventsStore');
var valueType = require('../../src/valueType');
var _ = require('lodash');
var expect = require('chai').expect;

describe('EventsStore', function() {
    var eventsStore;
    beforeEach(function(){
        eventsStore = eventsStoreModule.create();
    });

    var AggregateId = valueType.extends(function AggregateId(id){
        this.id = id;
    }, function() {
        return 'Id:' + this.id;
    });
    var Event = function Event(aggregateId, num){
        this.aggregateId = aggregateId;
        this.num = num;
    };
    Event.prototype.getAggregateId = function() {
        return this.aggregateId;
    };
    var BadEvent = function BadEvent(){ };

    it('When store event of aggregate Then can get this event of aggregate', function() {
        var aggregateId = new AggregateId('AggregateA');
        eventsStore.store(new Event(aggregateId));

        var result = eventsStore.getEventsOfAggregate(aggregateId);

        expect(result).to.contain(new Event(aggregateId));
    });

    it('When get this event of aggregate Then use equals and not operator', function() {
        var id = 'AggregateA';
        eventsStore.store(new Event(new AggregateId(id)));

        var result = eventsStore.getEventsOfAggregate(new AggregateId(id));

        expect(result).to.contain(new Event(new AggregateId(id)));
    });

    it('Given events of several aggregates When getEventsOfAggregate Then return events of only this aggregate', function() {
        var aggregateId1 = new AggregateId('AggregateA');
        var aggregateId2 = new AggregateId('AggregateB');
        eventsStore.store(new Event(aggregateId1));
        eventsStore.store(new Event(aggregateId2));
        eventsStore.store(new Event(aggregateId1));

        var result = eventsStore.getEventsOfAggregate(aggregateId1);

        expect(_.map(result, 'aggregateId')).to.contain(aggregateId1).and.not.contain(aggregateId2);
        expect(result).to.have.length(2);
    });

    it('When store event without aggregateId Then throw exception', function() {
        expect(function () {
            eventsStore.store(new BadEvent());
        }).to.throw(eventsStoreModule.EventDontContainsAggregateId);
    });

    it('Given several events When GetEventsOfAggregate Then return events and preserve order', function() {
        var aggregateId1 = new AggregateId('AggregateA');
        var aggregateId2 = new AggregateId('AggregateB');
        eventsStore.store(new Event(aggregateId1, 1));
        eventsStore.store(new Event(aggregateId1, 2));
        eventsStore.store(new Event(aggregateId1, 3));

        var result = eventsStore.getEventsOfAggregate(aggregateId1);

        expect(result).to.have.length(3);
        expect(_.sortBy(result, 'num')).to.deep.equals(result);
    });
});