namespace PH.Well.UnitTests.Factories
{
    using System;

    public abstract class EntityFactory<T, U> where T : EntityFactory<T, U>, new() where U : new()
    {
        private readonly U entity = new U();

        public static T New => new T();

        protected U Entity => this.entity;

        public T With(Action<U> expression)
        {
            expression(this.entity);

            return (T)this;
        }

        public U Build()
        {
            return this.entity;
        }
    }
}