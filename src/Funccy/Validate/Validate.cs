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
    /// <typeparam name="TErr"></typeparam>
    public class Validate<TData, TErr>
    {
        private IList<ConditionAsync> asyncConditions = new List<ConditionAsync>();
        private IList<ConditionResultAsync> asyncResultConditions = new List<ConditionResultAsync>();

        private IList<SimpleRuleAsync> simpleAsyncRules = new List<SimpleRuleAsync>();
        private IList<ResultRuleAsync> asyncResultRules = new List<ResultRuleAsync>();

        private IList<Composition> composedValidators = new List<Composition>();

        /// <summary>
        /// Creates a subvalidation which applies when the predicate is true.
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public Validate<TData, TErr> When(
            Func<TData, bool> pred,
            Func<Validate<TData, TErr>, Validate<TData, TErr>> rules)
        {
            var rule = new ConditionAsync(pred.Defer(), rules);
            asyncConditions.Add(rule);
            return this;
        }

        /// <summary>
        /// Creates a subvalidation which applies when the predicate is true.
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public Validate<TData, TErr> When(
            Func<TData, Task<bool>> pred,
            Func<Validate<TData, TErr>, Validate<TData, TErr>> rules)
        {
            var rule = new ConditionAsync(pred, rules);
            asyncConditions.Add(rule);
            return this;
        }

        /// <summary>
        /// Creates a subvalidation which applies when the predicate is true.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="getResult"></param>
        /// <param name="getSuccess"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public Validate<TData, TErr> When<TResult>(
            Func<TData, TResult> getResult,
            Func<TResult, bool> getSuccess,
            Func<Validate<TData, TErr>, TResult, Validate<TData, TErr>> rules)
        {
            var rule = new ConditionResultAsync(
                model => getResult.Defer()(model).CastTask<TResult, object>(),
                result => getSuccess((TResult)result),
                (v, result) => rules(v, (TResult)result));
            asyncResultConditions.Add(rule);
            return this;
        }

        /// <summary>
        /// Creates a subvalidation which applies when the predicate is true.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="getResult"></param>
        /// <param name="getSuccess"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public Validate<TData, TErr> When<TResult>(
            Func<TData, Task<TResult>> getResult,
            Func<TResult, bool> getSuccess,
            Func<Validate<TData, TErr>, TResult, Validate<TData, TErr>> rules)
        {
            var rule = new ConditionResultAsync(
                async model => await getResult(model),
                result => getSuccess((TResult)result),
                (v, result) => rules(v, (TResult)result));
            asyncResultConditions.Add(rule);
            return this;
        }

        /// <summary>
        /// Creates a rule that must be satisfied.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="getResult"></param>
        /// <param name="getSuccess"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must<TResult>(
            Func<TData, TResult> getResult,
            Func<TResult, bool> getSuccess,
            Func<TData, TResult, TErr> describe)
        {
            var rule = new ResultRuleAsync(
                getResult.CastReturn<TData, TResult, object>().Defer(),
                result => getSuccess((TResult)result),
                (model, result) => describe(model, (TResult)result));
            asyncResultRules.Add(rule);
            return this;
        }

        /// <summary>
        /// Creates a rule that must be satisfied.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="getResult"></param>
        /// <param name="getSuccess"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must<TResult>(
            Func<TData, Task<TResult>> getResult,
            Func<TResult, bool> getSuccess,
            Func<TData, TResult, TErr> describe)
        {
            var rule = new ResultRuleAsync(
                async m => await getResult(m),
                result => getSuccess((TResult)result),
                (model, result) => describe(model, (TResult)result));
            asyncResultRules.Add(rule);
            return this;
        }

        /// <summary>
        /// Creates a rule that must be satisfied.
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must(
            Func<TData, bool> pred,
            Func<TData, TErr> description)
        {
            var rule = new SimpleRuleAsync(pred.Defer(), description);
            simpleAsyncRules.Add(rule);
            return this;
        }

        /// <summary>
        /// Composes the rules from an existing validator into this one.
        /// </summary>
        /// <typeparam name="TData2"></typeparam>
        /// <param name="selector"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Compose<TData2>(Func<TData, TData2> selector, Validate<TData2, TErr> validator)
        {
            var rule = new Composition(
                async x => (await validator.Check(selector(x))).Extract(a => new TErr[0], b => b));
            composedValidators.Add(rule);
            return this;
        }

        /// <summary>
        /// Creates a rule that must be satisfied.
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must(
            Func<TData, Task<bool>> pred,
            Func<TData, TErr> describe)
        {
            var rule = new SimpleRuleAsync(pred, describe);
            simpleAsyncRules.Add(rule);
            return this;
        }

        /// <summary>
        /// Validates a model, and returns either the model or a list
        /// of errors.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OneOf<TData, TErr[]>> Check(TData model)
        {
            // 0. base case: we have no rules
            var noRules = simpleAsyncRules.None()
                && asyncResultRules.None();

            var noCompose = composedValidators.None();

            if (noRules && noCompose)
            {
                return new OneOf<TData, TErr[]>(model);
            }

            // 1. resolve subvalidation (when blocks)

            // holds conditional stuff so we don't have to reset it on this model.
            var subvalidator = new Validate<TData, TErr>();

            var passedAsyncConditions = await asyncConditions.WhereAsync(x => x.Pred(model));
            foreach (var p in passedAsyncConditions)
            {
                var returnedValidator = p.AddRules(subvalidator);
                if (returnedValidator != subvalidator)
                {
                    // TODO some better way to prevent mutations on this so we don't have
                    // to throw any exceptions
                    throw new InvalidOperationException($"Please use the provided validator reference.");
                }
            }

            var passedAsyncResultConditions = await (await asyncResultConditions
                .Select(x => x.GetResultPlusSuccess(model))
                .WhereAsync(async x => (await x).Success))
                .WhenAll();

            foreach (var (Success, Result, Condition) in passedAsyncResultConditions)
            {
                var returnedValidator = Condition.AddRules(subvalidator, Result);
                if (returnedValidator != subvalidator)
                {
                    throw new InvalidOperationException($"Please use the provided validator reference.");
                }
            }

            var specificErrors = (await subvalidator.Check(model))
                .Extract(a => new TErr[0], b => b);

            // 2. check mixed-in validation

            var composedErrors = (await composedValidators
                .Select(x => x.Checker(model))
                .WhenAll())
                .SelectMany(x => x);

            // 3. Check the actual rules

            var failed2 = (await simpleAsyncRules
                .WhereNotAsync(x => x.Pred(model)))
                .Select(x => x.Describe(model));

            var failed4 = (await (await asyncResultRules
                .Select(x => x.GetResultPlusSuccess(model))
                .WhereNotAsync(async x => (await x).Success))
                .WhenAll())
                .Select(x => x.Condition.Describe(model, x.Result));

            // 4. tally and report

            var allErrors = specificErrors
                .Concat(composedErrors)
                .Concat(failed2)
                .Concat(failed4)
                .ToArray();

            if (allErrors.None())
            {
                return new OneOf<TData, TErr[]>(model);
            }

            return new OneOf<TData, TErr[]>(allErrors);
        }

        /// <summary>
        /// Validates all given models, and returns a list of model or error lists.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Task<IEnumerable<OneOf<TData, TErr[]>>> CheckAll(IEnumerable<TData> models)
        {
            return models.Select(Check).WhenAll();
        }

        private class ConditionAsync
        {
            public ConditionAsync(
                Func<TData, Task<bool>> pred,
                Func<Validate<TData, TErr>, Validate<TData, TErr>> rules)
            {
                Pred = pred;
                AddRules = rules;
            }

            public Func<TData, Task<bool>> Pred { get; }
            public Func<Validate<TData, TErr>, Validate<TData, TErr>> AddRules { get; }
        }

        private class ConditionResultAsync
        {
            public ConditionResultAsync(
                Func<TData, Task<object>> getResult,
                Func<object, bool> getSuccess,
                Func<Validate<TData, TErr>, object, Validate<TData, TErr>> rules)
            {
                GetResult = getResult;
                GetSuccess = getSuccess;
                AddRules = rules;
            }

            public Func<TData, Task<object>> GetResult { get; }
            public Func<object, bool> GetSuccess { get; }
            public Func<Validate<TData, TErr>, object, Validate<TData, TErr>> AddRules { get; }

            public async Task<(bool Success, object Result, ConditionResultAsync Condition)> GetResultPlusSuccess(TData model)
            {
                var result = await GetResult(model);
                var success = GetSuccess(result);
                return (success, result, this);
            }
        }

        private class SimpleRuleAsync
        {
            public SimpleRuleAsync(
                Func<TData, Task<bool>> pred,
                Func<TData, TErr> describe)
            {
                Pred = pred;
                Describe = describe;
            }

            public Func<TData, Task<bool>> Pred { get; }
            public Func<TData, TErr> Describe { get; }
        }

        private class ResultRuleAsync
        {
            public ResultRuleAsync(
                Func<TData, Task<object>> getResult,
                Func<object, bool> getSuccess,
                Func<TData, object, TErr> describe)
            {
                GetResult = getResult;
                GetSuccess = getSuccess;
                Describe = describe;
            }

            public Func<TData, Task<object>> GetResult { get; }
            public Func<object, bool> GetSuccess { get; }
            public Func<TData, object, TErr> Describe { get; }

            public async Task<(bool Success, object Result, ResultRuleAsync Condition)> GetResultPlusSuccess(TData model)
            {
                var result = await GetResult(model);
                var success = GetSuccess(result);
                return (success, result, this);
            }
        }

        private class Composition
        {
            public Composition(Func<TData, Task<TErr[]>> checker)
            {
                Checker = checker;
            }

            public Func<TData, Task<TErr[]>> Checker { get; }
        }
    }

    public static class ValidateExtensions
    {
        /// <summary>
        /// Creates a rule that must be satisfied, as well as a subvalidator that applies after it is satisfied.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="validate"></param>
        /// <param name="pred"></param>
        /// <param name="describe"></param>
        /// <param name="subsection"></param>
        /// <returns></returns>
        public static Validate<TData, TErr> Prereq<TData, TErr>(
            this Validate<TData, TErr> validate,
            Func<TData, bool> pred,
            Func<TData, TErr> describe,
            Func<Validate<TData, TErr>, Validate<TData, TErr>> subsection)
        {
            return validate
                .Must(pred, describe)
                .When(pred.Not(), subsection);
        }

        /// <summary>
        /// Creates a rule that must be satisfied, as well as a subvalidator that applies after it is satisfied.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="validate"></param>
        /// <param name="getResult"></param>
        /// <param name="getSuccess"></param>
        /// <param name="describe"></param>
        /// <param name="subsection"></param>
        /// <returns></returns>
        public static Validate<TData, TErr> Prereq<TData, TResult, TErr>(
            this Validate<TData, TErr> validate,
            Func<TData, TResult> getResult,
            Func<TResult, bool> getSuccess,
            Func<TData, TResult, TErr> describe,
            Func<Validate<TData, TErr>, TResult, Validate<TData, TErr>> subsection)
        {
            return validate
                .Must(getResult, getSuccess, describe)
                .When(getResult, getSuccess.Not(), subsection);
        }
    }
}
