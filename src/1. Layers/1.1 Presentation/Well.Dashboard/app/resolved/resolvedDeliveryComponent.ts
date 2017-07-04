// import { NavigateQueryParametersService }           from '../shared/NavigateQueryParametersService';
// import { BaseComponent }                            from '../shared/BaseComponent';
// import { Component, OnInit, ViewChild, OnDestroy }  from '@angular/core';
// import { GlobalSettingsService }                    from '../shared/globalSettings';
// import { Router, ActivatedRoute }                   from '@angular/router';
// import { ResolvedDelivery }                         from './resolvedDelivery';
// import { ResolvedDeliveryService }                  from './ResolvedDeliveryService';
// import { DropDownItem }                             from '../shared/dropDownItem';
// import { ContactModal }                             from '../shared/contactModal';
// import { AccountService }                           from '../account/accountService';
// import { IAccount }                                 from '../account/account';
// import { RefreshService }                           from '../shared/refreshService';
// import { SecurityService }                          from '../shared/security/securityService';
// import { OrderByExecutor }                          from '../shared/OrderByExecutor';
// import { Branch }                                   from '../shared/branch/branch';
// import {IObservableAlive}                           from '../shared/IObservableAlive';
// import 'rxjs/Rx';
//
// @Component({
//     selector: 'ow-resolved',
//     templateUrl: './app/resolved/resolveddelivery-list.html',
//     providers: [ResolvedDeliveryService]
// })
// export class ResolvedDeliveryComponent extends BaseComponent implements OnInit, OnDestroy, IObservableAlive
// {
//     public isLoading: boolean = true;
//     public lastRefresh = Date.now();
//     public deliveries = new Array<ResolvedDelivery>();
//     public currentConfigSort: string;
//     public account: IAccount;
//     private orderBy: OrderByExecutor = new OrderByExecutor();
//     public isAlive: boolean = true;
//
//     @ViewChild(ContactModal) public contactModal: ContactModal;
//
//     constructor(
//         protected globalSettingsService: GlobalSettingsService,
//         private resolvedDeliveryService: ResolvedDeliveryService,
//         private accountService: AccountService,
//         private router: Router,
//         private activatedRoute: ActivatedRoute,
//         private refreshService: RefreshService,
//         protected securityService: SecurityService,
//         private nqps: NavigateQueryParametersService)
//     {
//
//         super(nqps, globalSettingsService, securityService);
//         this.options = [
//             new DropDownItem('Route', 'routeNumber'),
//             new DropDownItem('Branch', 'branchId', false, 'number'),
//             new DropDownItem('Invoice No', 'invoiceNumber'),
//             new DropDownItem('Account', 'accountCode'),
//             new DropDownItem('Account Name', 'accountName'),
//             new DropDownItem('Status', 'jobStatus'),
//             new DropDownItem('Assigned', 'assigned'),
//             new DropDownItem('Date', 'deliveryDate', false, 'date')
//         ];
//         this.sortField = 'deliveryDate';
//     }
//
//     public ngOnInit()
//     {
//         super.ngOnInit();
//         this.refreshService.dataRefreshed$
//             .takeWhile(() => this.isAlive)
//             .subscribe(r => this.getDeliveries());
//
//         this.activatedRoute.queryParams
//             .takeWhile(() => this.isAlive)
//             .subscribe(params =>
//             {
//                 this.getDeliveries();
//             });
//     }
//
//     public ngOnDestroy()
//     {
//         super.ngOnDestroy();
//         this.isAlive = false;
//     }
//
//     public getDeliveries()
//     {
//         this.resolvedDeliveryService.getResolvedDeliveries()
//             .takeWhile(() => this.isAlive)
//             .subscribe(deliveries =>
//             {
//                 this.deliveries = deliveries || new Array<ResolvedDelivery>();
//                 this.lastRefresh = Date.now();
//                 this.isLoading = false;
//             },
//             error =>
//             {
//                 this.lastRefresh = Date.now();
//                 this.isLoading = false;
//             });
//     }
//
//     public deliverySelected(delivery): void
//     {
//         this.router.navigate(['/delivery', delivery.id]);
//     }
//
//     public onSortDirectionChanged(isDesc: boolean)
//     {
//         super.onSortDirectionChanged(isDesc);
//         this.deliveries = this.orderBy.Order(this.deliveries, this);
//     }
//
//     public openModal(accountId): void
//     {
//         this.accountService.getAccountByAccountId(accountId)
//             .takeWhile(() => this.isAlive)
//             .subscribe(account =>
//             {
//                 this.account = account;
//                 this.contactModal.show(this.account);
//             });
//     }
//
//     // public getAssignModel(delivery: ResolvedDelivery): AssignModel
//     // {
//     //     const branch: Branch = { id: delivery.branchId } as Branch;
//     //     return new AssignModel(undefined, branch, undefined, this.isReadOnlyUser);
//     // }
//
//     public onAssigned(assigned: boolean)
//     {
//         this.getDeliveries();
//     }
// }