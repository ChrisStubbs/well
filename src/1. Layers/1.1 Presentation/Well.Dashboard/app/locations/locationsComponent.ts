import { ActivatedRoute }                               from '@angular/router';
import { Component }                                    from '@angular/core';
import { GlobalSettingsService }                        from '../shared/globalSettings';
import { Route }                                        from './route';
import { RouteFilter }                                  from './routeFilter';
import { LocationsService }                             from './locationsService';
import { BranchService }                                from '../shared/branch/branchService';
import { AppSearchParameters }                          from '../shared/appSearch/appSearch';
import { AssignModel, AssignModalResult }               from '../shared/components/components';
import { Branch }                                       from '../shared/branch/branch';
import { IObservableAlive }                             from '../shared/IObservableAlive';
import { LookupService, LookupsEnum, ILookupValue }     from '../shared/services/services';
import { Router }                                       from '@angular/router';
import * as _                                           from 'lodash';
import { Observable }                                   from 'rxjs/Observable';
import { GridHelpersFunctions }                         from '../shared/gridHelpers/gridHelpersFunctions';
import 'rxjs/Rx';

@Component({
    selector: 'ow-location',
    templateUrl: './app/locations/locationsComponent.html',
    providers: [LocationsService, BranchService]
})
export class LocationsComponent implements IObservableAlive
{
    public isAlive: boolean = true;

    public branches: Array<[string, string]>;

    constructor(private locationsService: LocationsService,
                private branchService: BranchService,
                private globalSettingsService: GlobalSettingsService,
                private router: Router) {}

    public ngOnDestroy(): void {
        this.isAlive = false;
    }

    public ngOnInit(): void
    {
        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
        .takeWhile(() => this.isAlive)
        .subscribe(res =>
        {
            this.branches = res;
            if (this.branches.length === 0)
            {
                // no branches set up
                this.router.navigateByUrl('/branch');
                return;
            }
        });
    }

}