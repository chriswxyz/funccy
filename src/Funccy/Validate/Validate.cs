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
            public Rule(Func<TData, Task<bool>> pred, Func<TData, TErr> desc)
            {
                Pred = pred;
                Describe = desc;
            }

            public Func<TData, Task<bool>> Pred { get; }
            public Func<TData, TErr> Describe { get; }
        }

        private IImmutableList<Rule> _rules = ImmutableList<Rule>.Empty;

        public Validate<TData, TErr> WithRule(Func<TData, bool> pred, Func<TData, TErr> description)
        {
            var r = new Rule(x => Task.FromResult(pred(x)), description);
            _rules = _rules.Add(r);
            return this;
        }

        public Validate<TData, TErr> WithRule(Func<TData, Task<bool>> pred, Func<TData, TErr> description)
        {
            var r = new Rule(pred, description);
            _rules = _rules.Add(r);
            return this;
        }

        public async Task<OneOf<TData, TErr[]>> Check(TData model)
        {
            Task<bool> RulePassed(Rule r) => r.Pred(model);

            var failed = await _rules.WhereAsync(async r => !(await RulePassed(r)));

            if (!failed.Any()) { return new OneOf<TData, TErr[]>(model); }

            var problems = failed
                .Select(x => x.Describe(model))
                .ToArray();

            return new OneOf<TData, TErr[]>(problems);
        }

        public Task<IEnumerable<OneOf<TData, TErr[]>>> CheckAll(IEnumerable<TData> models)
        {
            return models.Select(Check).WhenAll();
        }
    }
}
