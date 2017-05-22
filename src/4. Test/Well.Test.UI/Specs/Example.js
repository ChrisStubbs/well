// Example.js

describe('Example Test',
    function() {
        it('runs a sample test',
            function() {
                expect(1).toBe(1);
            });

        /*
        //Not working with signalr on pages
        it('should load dashboard page',
            function() {
                browser.get(browser.params.baseUrl + "widgetstats");
                browser.waitForAngular();
                element.all(by.css('.widget-container'))
                    .then(function(items) {
                        expect(items.length).toBe(1);
                    });
            });
        */
    });
