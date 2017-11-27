import { Component, EventEmitter, Input, Output }   from '@angular/core';
import { SecurityService }                          from './services/securityService';

@Component({
    selector: 'ow-button-dropdown',
    templateUrl: 'app/shared/buttonDropDownComponent.html'

})
export class  ButtonDropDownComponent
{
    @Input() public disabled: boolean = false;
    @Output() public onOptionClicked: EventEmitter<string> = new EventEmitter<string>();
    private items: string[] = [];

    constructor(private securityService: SecurityService)
    {
        if (this.securityService.userHasPermission(SecurityService.manuallyCompleteBypass))
        {
            this.items.push('Manually Complete');
            this.items.push('Manually Bypass');
            this.items.push('Recirculate Documents');
        }

        if (this.securityService.userHasPermission(SecurityService.editExceptions))
        {
            this.items.push('Edit Exceptions');
            this.items.push('Submit Exceptions');
        }
    }

    public optionClicked(option: string): void
    {
        this.onOptionClicked.emit(option);
    }
}