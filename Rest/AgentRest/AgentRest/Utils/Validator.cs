using System.Collections.Immutable;

namespace AgentRest.Utils
{
    // to catch a list of errors
    public class Validator<T>
    {
        private readonly T _value;
        private ImmutableList<string> _errors = [];

        private Validator(T value)
        {
            _value = value;
        }

        public static Validator<U> Of<U>(U value) => new Validator<U>(value);

        private Validator(T value, ImmutableList<string> errors)
        {
            _value = value;
            _errors = errors;
        }

        public Validator<T> Validate(Func<T, bool> predicate, string error)
        {
            if (predicate(_value))
            {
                return this;
            }
            return new Validator<T>(_value, [.._errors, error]);
        }

        public Validator<T> Validate<R>(Func<R, bool> predicate, R value, string error)
        {
            if (predicate(value))
            {
                return this;
            }
            return new Validator<T>(_value, [.. _errors, error]);
        }

        public ImmutableList<string> GetErrors() => _errors;

        public bool IsValid () => !_errors.Any();

        public void ThrowFirst()
        {
            if (_errors.Any())
            {
                throw new Exception(_errors.First());
            }
        }

        public void OrThrow(string message)
        {
            throw new Exception(message);
        }
    }
}
