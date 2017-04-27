import { OnInit, OnDestroy } from '@angular/core';
import { NavigateQueryParametersService } from './NavigateQueryParametersService';
import { NavigateQueryParameters } from './NavigateQueryParameters';
import { IOptionFilter, INavigationPager } from './IOptionFilter';
import { FilterOption } from './filterOption';
import { DropDownItem, SecurityService, GlobalSettingsService } from './shared';
import { Response } from '@angular/http';

export abstract class BaseComponent implements OnInit, IOptionFilter, OnDestroy, INavigationPager
{
    public options: DropDownItem[];
    public filterOption: FilterOption = new FilterOption();
    public selectedOption: DropDownItem;
    public selectedFilter: string;
    public currentPage: number;
    public sortDirection: string;
    public sortField: string;
    private navigationSubscriber: any;
    public readonly rowCount: number = 10;
    public isReadOnlyUser: boolean = false;

    constructor(
        private navigateQueryParametersService: NavigateQueryParametersService,
        protected globalSettingsService: GlobalSettingsService,
        protected securityService: SecurityService)
    {
        this.sortField = '';
        this.sortDirection = 'asc';
    }

    public ngOnInit(): void
    {
        const that = this;

        //TODO: Move this to the correct pages
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);

        this.navigateQueryParametersService.Navigate(that);

        this.navigationSubscriber = this.navigateQueryParametersService.BrowserNavigation
            .subscribe(p => this.navigateQueryParametersService.Navigate(this));

    }

    public onSortDirectionChanged(isDesc: boolean): void
    {
        this.sortDirection = isDesc ? 'desc' : 'asc';
        const item = new NavigateQueryParameters(undefined, 1, this.sortDirection);
        NavigateQueryParametersService.SaveSort(item);
        this.navigateQueryParametersService.Navigate(this);
    }

    public ngOnDestroy(): void
    {
        this.navigationSubscriber.unsubscribe();
    }

    public onFilterClicked(filterOption: FilterOption)
    {
        this.filterOption = filterOption;
        this.navigateQueryParametersService.Navigate(this);
    };

    public SetCurrentPage(pageNumber: number): void
    {
        this.currentPage = pageNumber;
        const item = new NavigateQueryParameters(undefined, this.currentPage);
        NavigateQueryParametersService.SavePageNumber(item);
    }

    public getSort(): string
    {
        return this.sortDirection;
    }
}