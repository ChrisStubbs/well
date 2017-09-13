import { Component }                    from '@angular/core';
import { GlobalSettingsService }        from '../shared/globalSettings';
import { LocationsService }             from './locationsService';
import { BranchService }                from '../shared/branch/branchService';
import { IObservableAlive }             from '../shared/IObservableAlive';
import { Router }                       from '@angular/router';
import { GridHelpersFunctions }         from '../shared/gridHelpers/gridHelpersFunctions';
import { LocationFilter, Locations }    from './singleLocation';
import { ILookupValue }                 from '../shared/services/ILookupValue';
import { Observable }                   from 'rxjs/Rx';
import { LookupService, LookupsEnum }   from '../shared/services/services';
import * as _                           from 'lodash';
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
    public jobIssueType: Array<ILookupValue>;

    private filters: LocationFilter = new LocationFilter();
    private source: Array<Locations> = [];
    private gridSource: Array<Locations> = [];
    private inputFilterTimer: any;
    private selectedBranhId: number;

    constructor(private locationsService: LocationsService,
                private branchService: BranchService,
                private globalSettingsService: GlobalSettingsService,
                private router: Router,
                private lookupService: LookupService) {}

    public ngOnDestroy(): void {
        this.isAlive = false;
    }

    public ngOnInit(): void
    {
        Observable.forkJoin(
            this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName),
            this.lookupService.get(LookupsEnum.JobIssueType)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(res => {
                this.branches = res[0];
                if (this.branches.length === 0)
                {
                    // no branches set up
                    this.router.navigateByUrl('/branch');
                    return;
                }

                this.loadData(+this.branches[0][0]);
                this.jobIssueType = res[1];
                this.filters.jobIssueType = 0;
            });
    }

    private loadData(branchId: number): void
    {
        if (branchId)
        {
            this.selectedBranhId = branchId;
        }

        this.locationsService.getLocations(this.selectedBranhId)
            .takeWhile(() => this.isAlive)
            .subscribe(res => {
                this.source = res;
                this.fillGrid();
            });
    }

    private fillGrid(): void
    {
        this.gridSource = GridHelpersFunctions.applyGridFilter<Locations, LocationFilter>(this.source, this.filters);
    }

    public clearFilters(): void
    {
        this.filters = new LocationFilter();
        this.fillGrid();
    }

    public filterFreeText(): void
    {
        GridHelpersFunctions.filterFreeText(this.inputFilterTimer)
            .then(() => this.fillGrid())
            .catch(() => this.inputFilterTimer = undefined);
    }

    public getJobIssueTypeDescription()
    {
        const result = _.filter(
            this.jobIssueType,
            (current: ILookupValue) => +current.key == this.filters.jobIssueType);

        if (!_.isEmpty(result))
        {
            return result[0].value;
        }

        return '';
    }
}