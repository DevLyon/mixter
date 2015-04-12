var valueType = require('../src/valueType');
var expect = require('chai').expect;

describe('valueType', function() {
    describe('Given value type When create an instance of this type', function() {
        var id = 'idA';
        var value = 'A';
        var ValueA = function ValueA(id, value) {
            this.id = id;
            this.value = value;
        };
        valueType.extends(ValueA, function toString() {
            return 'Id:' + this.id;
        });

        var instance = new ValueA(id, value);

        it('Then call constructor', function () {
            expect(instance.id).to.equal(id);
            expect(instance.value).to.equal(value);
        });

        it('Then toString call good function', function () {
            var result = instance.toString();

            expect(result).to.equal('Id:' + id);
        });

        it('Then can compare with other instance with same representation', function () {
            var instanceWithSameData = new ValueA(id, value);
            var instanceWithoutSameData = new ValueA(id + '2', value);

            expect(instance.equals(instanceWithSameData)).to.be.true;
            expect(instance.equals(instanceWithoutSameData)).to.be.false;
        });

        it('Then can compare with null ou undefined value', function () {
            expect(instance.equals(null)).to.be.false;
            expect(instance.equals(undefined)).to.be.false;
        });
    });

    it('When extends Then return type', function () {
        var EventC = function EventC(){ };

        var result = valueType.extends(EventC, function() { return 'A'});

        expect(result).to.be.equal(EventC);
    });
});