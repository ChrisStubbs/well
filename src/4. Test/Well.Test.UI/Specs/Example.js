// Example.js
describe('Example Test', function () {
    it('should load dashboard page', function () {
        browser.get(browser.params.baseUrl);
        //Always add this after a browser.get() for now. Shouldn't be needed once protractor is updated to work with angular 2 fully
        browser.waitForAngular();   
        
        expect(element(by.id('owContainer')).isPresent()).toBe(true);
    });
});