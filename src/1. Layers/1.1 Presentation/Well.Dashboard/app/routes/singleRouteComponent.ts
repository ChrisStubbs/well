import { Component }                                            from '@angular/core';
import { CurrencyPipe }                                         from '@angular/common';
import { RoutesService }                                        from './routesService'
import {SingleRoute, SingleRouteSource, SingleRouteFilter}      from './singleRoute';
import { ActivatedRoute }                                       from '@angular/router';
import * as _                                                   from 'lodash';
import { AssignModel, AssignModalResult }                       from '../shared/components/components';
import { Branch }                                               from '../shared/branch/branch';
import { SecurityService }                                      from '../shared/security/securityService';
import { GlobalSettingsService }                                from '../shared/globalSettings';
import { IObservableAlive }                                     from '../shared/IObservableAlive';
import { SingleRouteItem }                                      from './singleRoute';
import { LookupService, LookupsEnum, ILookupValue }             from '../shared/services/services';
import { Observable }                                           from 'rxjs';
import { GridHelpersFunctions }                                 from '../shared/gridHelpers/gridHelpersFunctions';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/singleRouteComponent.html',
    providers: [RoutesService, LookupService, CurrencyPipe]
})
export class SingleRouteComponent implements IObservableAlive
{
    public branchId: number;
    public driver: string;
    public routeDate: Date;
    public routeNumber: string;
    public isAlive: boolean = true;
    public jobTypes: Array<ILookupValue>;
    public wellStatus: Array<ILookupValue>;
    public assignees: Array<string>;

    private routeId: number;
    private isReadOnlyUser: boolean = false;
    private source = Array<SingleRouteSource>();
    private gridSource = Array<SingleRouteSource>();
    private filters = new SingleRouteFilter();
    private inputFilterTimer: any;

    constructor(
        private lookupService: LookupService,
        private routeService: RoutesService,
        private route: ActivatedRoute,
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService) { }

    public ngOnInit()
    {
        this.route.params
            .flatMap(data =>
            {
                this.routeId = data.id;

                return this.routeService.getSingleRoute(this.routeId);
            })
            .takeWhile(() => this.isAlive)
            .subscribe((data: SingleRoute) =>
            {
                this.branchId = data.branchId;
                this.routeNumber = data.routeNumber;
                this.driver = data.driver;
                this.routeDate = data.routeDate;

                this.source = this.buildGridSource(data.items);
                this.fillGridSource();
            });

        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.JobType),
            this.lookupService.get(LookupsEnum.WellStatus)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.jobTypes = res[0];
                this.wellStatus = res[1];
            });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public getAssignModel(route: SingleRouteSource, level: string): AssignModel
    {
        const branch = { id: this.branchId } as Branch;
        const jobs = _.map(route.items, 'jobId');

        return new AssignModel(
            route.stopAssignee,
            branch,
            jobs,
            this.isReadOnlyUser,
            route);
    }

    public onAssigned(event: AssignModalResult): void
    {
        const userName = _.isNil(event.newUser) ? undefined : event.newUser.name;
        const route = _.filter(this.gridSource,
                    (value: SingleRouteSource) => value.stopId == event.source.stopId);

        _.map(route.items, (value: SingleRouteItem) =>
        {
            value.assignee = userName;
            value.stopAssignee = userName;
        });
    }

    public clearFilter(): void
    {
        this.filters = new SingleRouteFilter();
        this.fillGridSource();
    }

    public selectStops(select: boolean, stop?: SingleRouteSource): void
    {
        let collection: Array<SingleRouteItem>;

        if (!_.isNil(stop))
        {
            //if it is not null it means the user click on a group
            collection = stop.items;
        }
        else
        {
            collection = _.reduce(this.gridSource, (total: SingleRouteItem[], current: SingleRouteSource) =>
            {
                return total.concat(current.items);
            }, [])
        }

        _.map(collection, current => current.isSelected = select);
    }

    public selectedItems(): Array<SingleRouteItem>
    {
        return _.chain(this.gridSource)
                    .reduce((total: SingleRouteItem[], current: SingleRouteSource) =>
                    {
                        return total.concat(current.items);
                    }, [])
                    .filter((current: SingleRouteItem) => current.isSelected)
               .value();
    }

    public allChildrenSelected(stop?: SingleRouteSource): boolean
    {
        let collection: Array<SingleRouteItem>;

        if (!_.isNil(stop))
        {
            collection = stop.items;
        }
        else
        {
            collection = _.reduce(this.gridSource, (total: SingleRouteItem[], current: SingleRouteSource) =>
                        {
                            return total.concat(current.items);
                        }, []);
        }

        return _.every(collection, ((current: SingleRouteItem) => current.isSelected));
    }

    public getSelectedJobIds(): number[]
    {
        return _.uniq(_.map(this.selectedItems(), 'jobId'));
    }

    private calculateTotals(data: Array<SingleRouteItem>): any
    {
        let totalExceptions: number = 0;
        let totalClean: number = 0;
        let totalTBA: number = 0;

        _.forEach(data, (current: SingleRouteItem) =>
        {
            totalExceptions += current.exceptions;
            totalClean += current.clean;
            totalTBA += current.tba;
        });

        return {
            totalExceptions: totalExceptions,
            totalClean: totalClean,
            totalTBA: totalTBA
        };
    }

    private buildGridSource(data: Array<SingleRouteItem>): Array<SingleRouteSource>
    {
        const result = Array<SingleRouteSource>();
        this.assignees = [];

        _.chain(data)
            .groupBy(current => current.stop)
            .map((current: Array<SingleRouteItem>) =>
            {
                const item = new SingleRouteSource();
                const summary = this.calculateTotals(current);
                const singleItem = _.head(current);

                item.stopId = singleItem.stopId;
                item.stop = singleItem.stop;
                item.stopStatus = singleItem.stopStatus;
                item.totalExceptions = summary.totalExceptions;
                item.totalClean = summary.totalClean;
                item.totalTBA = summary.totalTBA;
                item.stopAssignee = singleItem.stopAssignee;
                item.items = current;

                this.assignees.push(singleItem.stopAssignee || 'Unallocated');

                result.push(item);
            })
            .value();

        this.assignees = _.uniq(this.assignees);

        return result;
    }

    public areAllExpanded(): boolean
    {
        let value: boolean = true;

        _.forEach(this.source, current => value = value && current.isExpanded);

        return value;
    }

    public expandGroup(event: any): void
    {
        const action: boolean = !this.areAllExpanded();

        _.forEach(this.gridSource, (current: SingleRouteSource) =>
        {
            this.expand(event, current, action);
        });
    }

    private fillGridSource()
    {
        const values = Array<SingleRouteSource>();

        _.map(this.source, (current: SingleRouteSource) =>
        {
            const filteredValues =
                GridHelpersFunctions.applyGridFilter<SingleRouteItem, SingleRouteFilter>(current.items, this.filters);

            if (!_.isEmpty(filteredValues))
            {
                const newItem: SingleRouteSource = _.clone(current);

                newItem.items = _.clone(filteredValues);
                values.push(newItem);
            }
        });

        this.gridSource = values;
    }

    public filterFreeText(): void
    {
        GridHelpersFunctions.filterFreeText(this.inputFilterTimer)
            .then(() => this.fillGridSource())
            .catch(() => this.inputFilterTimer = undefined);
    }

    public expand(event: any, item: SingleRouteSource, expand?: boolean)
    {
        item.isExpanded = expand || !item.isExpanded;
        _.find(this.source, (current: SingleRouteSource) =>
            current.stopId == item.stopId).isExpanded = item.isExpanded;

        event.preventDefault();
    }
}