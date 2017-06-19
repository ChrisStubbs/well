import * as _ from 'lodash';
import { CompositeSpecification } from './CompositeSpecification';

export class DictionaryItem {
    [key: string]: string;
}

export class NavigateQueryParameters
{
    private pageSpec = new HasPageSpecification();
    private filterSpec = new HasFilterSpecification();
    private sortSpec = new HasSortSpecification();

    constructor (public Filter?: DictionaryItem, public Page?: number, public Sort?: string) { }

    public HasFilter(): boolean {
        return this.filterSpec.IsSatisfiedBy(this);
    }

    public HasPageNumber(): boolean {
        return this.pageSpec.IsSatisfiedBy(this);
    }

    public HasSort(): boolean {
        return this.sortSpec.IsSatisfiedBy(this);
    }

    public IsValidNavigation(): boolean {
        return this.pageSpec.or(this.filterSpec).or(this.sortSpec).IsSatisfiedBy(this);
    }
}

class HasSortSpecification extends CompositeSpecification<NavigateQueryParameters> {
    public IsSatisfiedBy(item: NavigateQueryParameters): boolean {
        return !(_.isNull(item) || _.isUndefined(item)
        || _.isNull(item.Sort) || _.isUndefined(item.Sort));
    }
}

class HasPageSpecification extends CompositeSpecification<NavigateQueryParameters> {
    public IsSatisfiedBy(item: NavigateQueryParameters): boolean {
        return !(_.isNull(item) || _.isUndefined(item)
        || _.isNull(item.Page) || _.isUndefined(item.Page));
    }
}

class HasFilterSpecification extends CompositeSpecification<NavigateQueryParameters> {
    public IsSatisfiedBy (item: NavigateQueryParameters): boolean {

        return !(_.isNull(item) || _.isUndefined(item) || _.isUndefined(item.Filter));
    }
}
