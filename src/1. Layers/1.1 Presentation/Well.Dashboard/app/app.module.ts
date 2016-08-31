import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
//import { provide } from '@angular/core';
import { AppComponent } from './appComponent';
import { appRouterProviders } from './appRoutes';

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, FormsModule, appRouterProviders],
    bootstrap: [AppComponent]
})
export class AppModule {
}