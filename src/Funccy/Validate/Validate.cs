using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private class Rule
        {
            public Rule(
                Func<TData, Task<bool>> pred, 
                Func<TData, TErr> desc)
            {
                Pred = pred;
                Describe = desc;
            }

            public Rule(
                Func<TData, bool> condition,
                Func<TData, Task<bool>> pred,
                Func<TData, TErr> desc)
            {
                Condition = condition;
                Pred = pred;
                Describe = desc;
            }

            public Func<TData, bool> Condition { get; } = x => true;
            public Func<TData, Task<bool>> Pred { get; }
            public Func<TData, TErr> Describe { get; }
        }

        private IImmutableList<Rule> _rules = ImmutableList<Rule>.Empty;

        /// <summary>
        /// Adds a rule that always applies to the model.
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must(
            Func<TData, bool> pred,
            Func<TData, TErr> description)
        {
            var r = new Rule(x => Task.FromResult(pred(x)), description);
            _rules = _rules.Add(r);
            return this;
        }

        /// <summary>
        /// Adds an async rule that always applies to the model.
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must(
            Func<TData, Task<bool>> pred,
            Func<TData, TErr> description)
        {
            var r = new Rule(pred, description);
            _rules = _rules.Add(r);
            return this;
        }

        /// <summary>
        /// Adds a conditional rule to the model.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pred"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must(
            Func<TData, bool> condition,
            Func<TData, bool> pred,
            Func<TData, TErr> description)
        {
            var r = new Rule(x => Task.FromResult(pred(x)), description);
            _rules = _rules.Add(r);
            return this;
        }

        /// <summary>
        /// Adds a conditional, async rule to the model.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pred"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Validate<TData, TErr> Must(
            Func<TData, bool> condition,
            Func<TData, Task<bool>> pred,
            Func<TData, TErr> description)
        {
            var r = new Rule(x => true, pred, description);
            _rules = _rules.Add(r);
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
            Task<bool> RulePassed(Rule r) => r.Pred(model);

            var failed = await _rules
                .Where(r => r.Condition(model))
                .WhereAsync(async r => !(await RulePassed(r)));

            if (!failed.Any()) { return new OneOf<TData, TErr[]>(model); }

            var problems = failed
                .Select(x => x.Describe(model))
                .ToArray();

            return new OneOf<TData, TErr[]>(problems);
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
    }
}
