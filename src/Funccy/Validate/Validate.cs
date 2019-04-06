using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    /// <summary>
    /// Builds a validator.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TFailure"></typeparam>
    public class Validate<TData, TFailure, TContext, TOutput>
    {
        private readonly Func<
            TData, // The data being validated
            TContext, // The other things needed to validate the data
            Func<TFailure, TOutput>, // A way to report failures
            Task<TOutput> // Return data you've already I/O'd. Or just the original model.
        > validation;

        public Validate(Func<TData, TContext, Func<TFailure, TOutput>, Task<TOutput>> validation)
        {
            this.validation = validation;
        }

        public async Task<OneOf<TOutput, TFailure[]>> Check(TData model, TContext context)
        {
            var failures = new List<TFailure>();

            TOutput report(TFailure fail)
            {
                failures.Add(fail);
                return default;
            };

            var output = await validation(model, context, report);

            if (failures.Any())
            {
                return new OneOf<TOutput, TFailure[]>(failures.ToArray());
            }

            return new OneOf<TOutput, TFailure[]>(output);
        }
    }
}
