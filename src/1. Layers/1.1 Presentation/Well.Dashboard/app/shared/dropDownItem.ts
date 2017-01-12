export class DropDownItem {
    public description: string;
    public value: string;
    public requiresServerCall: boolean;
    public type: string;

    constructor(description: string = '', value: string = '', requiresServerCall = false, type: string = 'string') {
        this.description = description;
        this.value = value;
        this.type = type;
        this.requiresServerCall = requiresServerCall;
    }
}