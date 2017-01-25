import * as _ from 'lodash';
import { CompositeSpecification } from './CompositeSpecification';

export class DictionaryItem {
    [key: string]: string;
};

export class NavigateQueryParameters {
    private pageSpec = new HasPageSpecification();
    private filterSpec = new HasFilterSpecification();

    constructor (public Filter?: DictionaryItem, public Page?: number) {}

    public HasFilter(): boolean {
        return this.filterSpec.IsSatisfiedBy(this);
    }

    public HasPageNumber(): boolean {
        return this.pageSpec.IsSatisfiedBy(this);
    }

    public IsValidNavigation(): boolean {
        return this.pageSpec.or(this.filterSpec).IsSatisfiedBy(this);
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

        return !(_.isUndefined(item) || _.isUndefined(item.Filter));
        // if (_.isUndefined(item) || _.isUndefined(item.Filter)) {
        //     return false;
        // }
        //
        // const k = _.keys(item.Filter)[0];
        // const v = item.Filter[k];
        //
        // //     check if the filter has a valid value
        // return !(_.isNull(v) || _.isUndefined(v) || _.trim(v) == '');
    }
}
