// promotion.js
describe('Example Test', function () {
    it('should load dashboard page', function () {
        browser.get(browser.params.baseUrl);
        
        expect(element(by.id('owContainer')).isPresent()).toBe(true);
    });
});