using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
    public class Validate<T>
    {
        private class Rule
        {
            public Rule(string key, Func<T, Task<bool>> pred, Func<T, string> desc)
            {
                Key = key;
                Pred = pred;
                Describe = desc;
            }

            public string Key { get; }
            public Func<T, Task<bool>> Pred { get; }
            public Func<T, string> Describe { get; }
        }

        private IImmutableList<Rule> _rules = ImmutableList<Rule>.Empty;

        public Validate<T> WithRule(string key, Func<T, bool> pred, Func<T, string> description)
        {
            var r = new Rule(key, x => Task.FromResult(pred(x)), description);
            _rules = _rules.Add(r);
            return this;
        }

        public Validate<T> WithRule(string key, Func<T, Task<bool>> pred, Func<T, string> description)
        {
            var r = new Rule(key, pred, description);
            _rules = _rules.Add(r);
            return this;
        }

        public async Task<OneOf<T, Problem[]>> Check(T model)
        {
            Task<bool> RulePassed(Rule r) => r.Pred(model);

            var failed = await _rules.WhereAsync(async r => !(await RulePassed(r)));

            if (!failed.Any()) { return new OneOf<T, Problem[]>(model); }

            var problems = failed
                .Select(x => new Problem(x.Key, x.Describe(model)))
                .ToArray();

            return new OneOf<T, Problem[]>(problems);
        }

        public Task<IEnumerable<OneOf<T, Problem[]>>> CheckAll(IEnumerable<T> models)
        {
            return models.Select(Check).WhenAll();
        }
    }
}
