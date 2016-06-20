// promotion.js
describe('Example Test', function () {
    it('should load dashboard page', function () {
        browser.get('http://localhost/Well/Dashboard/');
        
        expect(element(by.id('owContainer')).isPresent()).toBe(true);
    });
});