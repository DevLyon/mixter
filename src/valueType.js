var equals = function equals(other){
    if(!other){
        return false;
    }

    return this.toString() === other.toString();
};

exports.extends = function create(valueType, toString){
    valueType.prototype.toString = toString;

    valueType.prototype.equals = equals;

    return valueType;
};