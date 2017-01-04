import { Component, OnInit }  from '@angular/core';
import { Audit } from './audit';
import {AuditService} from './auditService';
import {DropDownItem} from '../shared/dropDownItem';
import { FilterOption } from '../shared/filterOption';
import {OrderArrowComponent} from '../shared/orderbyArrow';
import * as lodash from 'lodash';

@Component({
    templateUrl: './app/audit/audit-list.html'
})
export class AuditComponent implements OnInit {
    isLoading: boolean = true;
    audits: Audit[] = [];
    currentConfigSort: string;

    pageSize: number = 10;

    filterOptions: DropDownItem[] = [
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "account"),
        new DropDownItem("Delivery Date", "deliveryDate", false, "date"),
        new DropDownItem("Audit By", "auditBy"),
        new DropDownItem("Audit Date", "auditDate", false, "date")
    ];

    filterOption: FilterOption;

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    constructor(private auditService: AuditService) {}

    ngOnInit(): void {
            this.auditService.getAudits()
                .subscribe(a => {
                    this.audits = a;
                    this.isLoading = false;
                });
    }

    sortDirection(sortDirection): void {
        this.currentConfigSort = sortDirection === true ? '+auditDate' : '-auditDate';
        var sortString = this.currentConfigSort === '+dateTime' ? 'asc' : 'desc';
        lodash.sortBy(this.audits, ['dateTime'], [sortString]);
    }

    onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
    }
}

