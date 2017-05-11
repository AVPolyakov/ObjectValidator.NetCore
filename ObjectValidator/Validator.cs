using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectValidator
{
    public interface IValidator<out T>
    {
        T Object { get; }
        ValidationCommand Command { get; }
        string PropertyPrefix { get; }
        Task<List<ErrorInfo>> Validate();
    }

    public class Validator<T> : IValidator<T>
    {
        public ValidationCommand Command { get; }
        public T Object { get; }
        public string PropertyPrefix { get; }

        public Validator(T @object, ValidationCommand command, string propertyPrefix = "")
        {
            Object = @object;
            Command = command;
            PropertyPrefix = propertyPrefix;
        }

        public Task<List<ErrorInfo>> Validate() => Command.Validate();
    }

    public static class ValidatorExtensions
    {
        public static IValidator<T> Validator<T>(this T @object) => new Validator<T>(@object, new ValidationCommand());

        public static IPropertyValidator<T, TProperty> For<T, TProperty>(this IValidator<T> @this, Func<T, TProperty> func,
            string displayName = null)
            => new PropertyValidator<T, TProperty>(@this, func, displayName);
    }
}