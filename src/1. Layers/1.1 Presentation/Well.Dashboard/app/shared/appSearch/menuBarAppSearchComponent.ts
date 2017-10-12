import { Component, ViewChild, ElementRef}  from '@angular/core';
import { Router}                from '@angular/router';
import { AppSearch }            from './appSearchComponent';

import 'rxjs/add/operator/takeWhile';

@Component({
    selector: 'ow-menubarappSearch',
    templateUrl: './app/shared/appSearch/menuBarAppSearchComponent.html'
})
export class MenuBarAppSearchComponent {
    private alive: boolean = true;
    public display: boolean = true;
    @ViewChild(AppSearch) public search: AppSearch;
    @ViewChild('closeAppSearch') public closeBtn: ElementRef;

    constructor(private router: Router)
    {
        router.events
            .takeWhile(() => this.alive)
            .subscribe((val) =>
            {
                this.search.resetSearch();
                this.closeSearch();
            });
    }

    private closeSearch()
    {
        this.closeBtn.nativeElement.click();
    }

    public onSearch(): void
    {
        //this.closeSearch();
    }
}