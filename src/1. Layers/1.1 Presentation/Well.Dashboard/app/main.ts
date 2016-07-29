import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from "@angular/core";

import {AppComponent} from './appComponent';
import { appRouterProviders } from './appRoutes';

bootstrap(AppComponent, [appRouterProviders]);