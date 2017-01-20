export class DictionaryItem {
    [key: string]: string;
};

export class NavigateQueryParameters{
    constructor (public Filter?: DictionaryItem, public Page?: number) {}
}