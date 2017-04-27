import { Component, ViewChild}  from '@angular/core';
import { Router}                from '@angular/router';
import { AppSearch }            from './appSearchComponent'
import 'rxjs/add/operator/takeWhile';

@Component({
    selector: 'ow-menubarappSearch',
    templateUrl: './app/shared/appSearch/menuBarAppSearchComponent.html',
    styles: ['.modal-dialog { width: 782px}']
})
export class MenuBarAppSearchComponent {
    private alive: boolean = true;
    @ViewChild(AppSearch) public search: AppSearch;

    constructor(private router: Router)
    {
        router.events
            .takeWhile(() => this.alive)
            .subscribe((val) =>
            {
                this.search.resetSearch();
            });
    }
}