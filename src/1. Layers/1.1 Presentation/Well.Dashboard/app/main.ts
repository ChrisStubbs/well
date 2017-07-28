//declare var require;
import core_js from 'core-js/client/shim.min.js';

import 'reflect-metadata';
require('zone.js/dist/zone');
require('zone.js/dist/long-stack-trace-zone'); // for development only - not needed for prod deployment

// Angular 2
import '@angular/platform-browser';
import '@angular/platform-browser-dynamic';
import '@angular/core';
import '@angular/common';
import '@angular/http';
import '@angular/router';

// RxJS
import 'rxjs';

// Other vendors for example jQuery, Lodash or Bootstrap
import * as _ from 'lodash';

// require('../Content/Site.less');
// require('../Content/css/6-chasing-dots.css');
// require('../Content/css/primeng.css');
// require('../Content/css/ngprime-theme.css');

// let style = require('style-loader/useable!css-loader!./shared/primeng/resources/primeng.css?singleton&insertAt=top');
// style.use();

/* tslint:disable */
// style = require('style-loader/useable!css-loader!./Content/css/theme.css?singleton&insertAt=top');
// style.use();
/* tslint:enable */

// style = require('style-loader/useable!css-loader!../Content/css/bootstrap.min.css?singleton&insertAt=top');
// style.use();

// You can import js, ts, css, sass, ...
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './appModule';

platformBrowserDynamic().bootstrapModule(AppModule);
