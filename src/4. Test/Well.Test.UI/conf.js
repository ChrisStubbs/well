// conf.js
exports.config = {
    framework: 'jasmine2',
    directConnect: true,
    useAllAngular2AppRoots: true,
    rootElement: "[ow-app]",
    //seleniumAddress: 'http://localhost:4444/wd/hub',
    specs: ['Specs/*.js']
}