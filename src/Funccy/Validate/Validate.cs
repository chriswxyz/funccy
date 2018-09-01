using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public class Validate<TData, TErr>
    {
        private class Rule
        {
            public Rule(string key, Func<TData, Task<bool>> pred, Func<TData, TErr> desc)
            {
                Key = key;
                Pred = pred;
                Describe = desc;
            }

            public string Key { get; }
            public Func<TData, Task<bool>> Pred { get; }
            public Func<TData, TErr> Describe { get; }
        }

        private IImmutableList<Rule> _rules = ImmutableList<Rule>.Empty;

        public Validate<TData, TErr> WithRule(string key, Func<TData, bool> pred, Func<TData, TErr> description)
        {
            var r = new Rule(key, x => Task.FromResult(pred(x)), description);
            _rules = _rules.Add(r);
            return this;
        }

        public Validate<TData, TErr> WithRule(string key, Func<TData, Task<bool>> pred, Func<TData, TErr> description)
        {
            var r = new Rule(key, pred, description);
            _rules = _rules.Add(r);
            return this;
        }

        public async Task<OneOf<TData, Problem<TErr>[]>> Check(TData model)
        {
            Task<bool> RulePassed(Rule r) => r.Pred(model);

            var failed = await _rules.WhereAsync(async r => !(await RulePassed(r)));

            if (!failed.Any()) { return new OneOf<TData, Problem<TErr>[]>(model); }

            var problems = failed
                .Select(x => new Problem<TErr>(x.Key, x.Describe(model)))
                .ToArray();

            return new OneOf<TData, Problem<TErr>[]>(problems);
        }

        public Task<IEnumerable<OneOf<TData, Problem<TErr>[]>>> CheckAll(IEnumerable<TData> models)
        {
            return models.Select(Check).WhenAll();
        }
    }
}
