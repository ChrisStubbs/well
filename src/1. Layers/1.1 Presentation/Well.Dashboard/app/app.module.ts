import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './appComponent';
import { appRouterProviders } from './appRoutes';

import {PaginatePipe, PaginationControlsCmp} from 'ng2-pagination';
import {OptionFilterComponent} from './shared/optionfilter.component';
import {OptionFilterPipe} from './shared/optionFilterPipe';
import {OrderBy} from "./shared/orderBy";

@NgModule({
    declarations: [AppComponent, OptionFilterComponent, OptionFilterPipe, PaginationControlsCmp, PaginatePipe, OrderBy],
    imports: [BrowserModule, FormsModule, appRouterProviders],
    bootstrap: [AppComponent]
})
export class AppModule {
}