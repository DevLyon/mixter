module.exports = function () {
    return {
        files: [
            "src/**/*.js"
        ],

        "tests": [
            "test/**/*.js"
        ],

        env: {
            type: 'node'
        },

        testFramework: 'mocha@2.1.0'
    };
};