export class DropDownItem {
    public description: string;
    public value: string;
    public requiresServerCall: boolean;
    public type: string;
    private isDefaultItem: boolean;

    constructor(description = '', value = '', requiresServerCall = false, type = 'string') {
        this.description = description;
        this.value = value;
        this.type = type;
        this.requiresServerCall = requiresServerCall;
        this.isDefaultItem = false;
    }

    public IsDefaultItem(): boolean {
        return this.isDefaultItem;
    }

    public static CreateDefaultOption (): DropDownItem {
        const result = new DropDownItem('Option', '');
        result.isDefaultItem = true;

        return result;
    }
}