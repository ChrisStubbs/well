// conf.js
exports.config = {
    framework: 'jasmine2',
    directConnect: true,
    useAllAngular2AppRoots: true,
    //seleniumAddress: 'http://localhost:4444/wd/hub',
    specs: ['Specs/*.js'],
    onPrepare: function () {
        browser.ignoreSynchronization = true;
        //If running on TeamCity then use the TeamCity test results reporter
        if (process.env.TEAMCITY_VERSION) { 
            var jasmineReporters = require('jasmine-reporters');
            jasmine.getEnv().addReporter(new jasmineReporters.TeamCityReporter());
        }
    }
}