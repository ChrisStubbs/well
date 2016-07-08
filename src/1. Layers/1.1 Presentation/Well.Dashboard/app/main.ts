import { bootstrap }    from 'angular2/platform/browser';
import { GlobalSettings } from './globalSettings';
// Our main component
import { AppComponent } from './appComponent';

bootstrap(AppComponent, [GlobalSettings]);