using System.Collections.Generic;
using System.Linq;

namespace Funccy
{
    // Value object implementation from
    // http://enterprisecraftsmanship.com/2017/08/28/value-object-a-better-implementation/

    /// <summary>
    /// Base class for implementing Value Object semantics.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            var valueObject = obj as ValueObject<T>;

            if (valueObject is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return GetEqualityComponents()
                .SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }
    }
}
