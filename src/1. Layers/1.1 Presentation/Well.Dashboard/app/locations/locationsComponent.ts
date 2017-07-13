import { Component }                    from '@angular/core';
import { GlobalSettingsService }        from '../shared/globalSettings';
import { LocationsService }             from './locationsService';
import { BranchService }                from '../shared/branch/branchService';
import { IObservableAlive }             from '../shared/IObservableAlive';
import { Router }                       from '@angular/router';
import { GridHelpersFunctions }         from '../shared/gridHelpers/gridHelpersFunctions';
import { LocationFilter, Locations }    from './singleLocation';
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

    private filters: LocationFilter = new LocationFilter();
    private source: Array<Locations> = [];
    private gridSource: Array<Locations> = [];
    private inputFilterTimer: any;

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

                this.loadData(+this.branches[0][0]);
            });
    }

    private loadData(branchId: number): void
    {
        this.locationsService.getLocations(branchId)
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
}