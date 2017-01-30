
export interface ISpecification<T> {
    IsSatisfiedBy(candidate: T): boolean;
}

export interface ICompositeSpecification<T> extends ISpecification<T> {
    and(other: ICompositeSpecification<T>): ICompositeSpecification<T>;
    or(other: ICompositeSpecification<T>): ICompositeSpecification<T>;
    not(): ICompositeSpecification<T>;
}

export abstract class CompositeSpecification<T> implements ICompositeSpecification<T> {
    public abstract IsSatisfiedBy(candidate: T): boolean;

    public and(other: ICompositeSpecification<T>): ICompositeSpecification<T> {
        return new AndSpecification<T>(this, other);
    }

    public or(other: ICompositeSpecification<T>): ICompositeSpecification<T> {
        return new OrSpecification<T>(this, other);
    }

    public not(): ICompositeSpecification<T> {
        return new NotSpecification<T>(this);
    }
}

class AndSpecification<T> extends CompositeSpecification<T> {
    constructor(
        public left: ICompositeSpecification<T>,
        public right: ICompositeSpecification<T>) {
        super();
    }

    public IsSatisfiedBy(candidate: T): boolean {
        return this.left.IsSatisfiedBy(candidate)
            && this.right.IsSatisfiedBy(candidate);
    }
}

class OrSpecification<T> extends CompositeSpecification<T> {
    constructor(
        public left: ICompositeSpecification<T>,
        public right: ICompositeSpecification<T>) {
        super();
    }

    public IsSatisfiedBy(candidate: T): boolean {
        return this.left.IsSatisfiedBy(candidate)
            || this.right.IsSatisfiedBy(candidate);
    }
}

class NotSpecification<T> extends CompositeSpecification<T> {
    constructor(
        public spec: ICompositeSpecification<T>) {
        super();
    }

    public IsSatisfiedBy(candidate: T): boolean {
        return !this.spec.IsSatisfiedBy(candidate);
    }
}