import {GlobalSettingsService} from '../globalSettings';
import {Component, Input} from '@angular/core';

@Component({
    selector: 'crmLink',
    template: '<span (click)="openLink($event)" style="cursor: pointer">' +
        '<a target="_blank" href="#">{{linkText}}</a> ' +
        '<span class="glyphicon glyphicon-new-window"></span>' +
    '</span>',
})
export class CrmLinkController
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

    private openLink(event: any)
    {
        window.open(this.crmURL(), '_blank', '');

        event.preventDefault();
    }
}