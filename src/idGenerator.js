var uuid = require('node-uuid');

exports.generate = function generate(){
    return uuid.v4();
};