import {GlobalSettingsService} from '../globalSettings';
import {Component, Input} from '@angular/core';

@Component({
    selector: 'crmLink',
    template: '<span (click)="openLink()">' +
        '<a target="_blank" href="#">{{linkText}}</a> ' +
        '<span class="glyphicon glyphicon-new-window"></span>' +
    '</span>'
})
export class CrmLinkPipe
{
    @Input() public branchId: number;
    @Input() public accountNumber: string;
    @Input() public linkText: string;
    constructor(private globalSettingsService: GlobalSettingsService) {}

    private crmURL(): string
    {
        const urlBase = this.globalSettingsService.globalSettings.crmBaseUrl;

        return encodeURI(urlBase +
            'CallLog/History?branchId=' +
            this.branchId +
            '&accountNumber=' +
            this.accountNumber);
    }

    private openLink()
    {
        window.open(this.crmURL(), '_blank', '');
    }
}

export class AccountReference implements IAccountReference {
    public accountNumber: string;
    public branchId: number;

    constructor(accountNumber: string, branchId: number) {
        this.accountNumber = accountNumber;
        this.branchId = branchId;
    }
}

export interface IAccountReference {
    accountNumber: string;
    branchId: number;
}