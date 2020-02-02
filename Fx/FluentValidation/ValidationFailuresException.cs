using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FluentValidation.Results
{
    [Serializable]
    public class ValidationFailuresException : Exception
    {
        public ValidationFailuresException() { }

        public ValidationFailuresException(string message) : base(message) { }

        public ValidationFailuresException(string message, Exception inner) : base(message, inner) { }

        public ValidationFailuresException(IReadOnlyList<ValidationFailure> validationFailures)
        {
            var propertyNames = validationFailures
                .Select(failure => failure.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = validationFailures
                    .Where(failure => failure.PropertyName == propertyName)
                    .Select(failure => failure.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        protected ValidationFailuresException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IDictionary<string, string[]> Failures { get; } = new Dictionary<string, string[]>();
    }
}
