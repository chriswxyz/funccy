using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funccy
{
	public static class OneOfExtensions2
	{
		/// <summary>
		/// Filters the elements of the collection where the value is type A.
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TB"></typeparam>
		/// <param name="coll"></param>
		/// <returns></returns>
		public static IEnumerable<TA> WhereA<TA, TB>(this IEnumerable<OneOf<TA, TB>> coll)
		{
			return coll
				.Where(x => x.Extract(a => true, b => false))
				.Select(x => x.Extract(a => a, b => throw new InvalidOperationException()));
		}

		/// <summary>
		/// Filters the elements of the collection where the value is type B.
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TB"></typeparam>
		/// <param name="coll"></param>
		/// <returns></returns>
		public static IEnumerable<TB> WhereB<TA, TB>(this IEnumerable<OneOf<TA, TB>> coll)
		{
			return coll
				.Where(x => x.Extract(a => false, b => true))
				.Select(x => x.Extract(a => throw new NotSupportedException(), b => b));

		}

		public static IEnumerable<TNext> ExtractAll<TA, TB, TNext>(
			this IEnumerable<OneOf<TA, TB>> oneOfs,
			Func<TA, TNext> a,
			Func<TB, TNext> b)
		{
			return oneOfs.Select(x => x.Extract(a, b));
		}

		/// <summary>
		/// Splits the elements of the collection based on type.
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TB"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="coll"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public static TResult Partition<TA, TB, TResult>(
			this IEnumerable<OneOf<TA, TB>> coll,
			Func<IEnumerable<TA>, IEnumerable<TB>, TResult> f)
		{
			var As = coll.WhereA();
			var Bs = coll.WhereB();
			return f(As, Bs);
		}

		/// <summary>
		/// Lifts an inner Task out of the OneOf.
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TB"></typeparam>
		/// <param name="oneOf"></param>
		/// <returns></returns>
		public static async Task<OneOf<TA, TB>> LiftTask<TA, TB>(this OneOf<Task<TA>, Task<TB>> oneOf)
		{
			var isA = oneOf.Extract(x => true, x => false);

			if (isA)
			{
				var a = await oneOf.Extract(x => x, x => throw new InvalidOperationException());
				return new OneOf<TA, TB>(a);
			}

			var b = await oneOf.Extract(x => throw new InvalidOperationException(), x => x);
			return new OneOf<TA, TB>(b);
		}

		/// <summary>
		/// Lifts an inner Task out of the OneOf.
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TB"></typeparam>
		/// <param name="oneOf"></param>
		/// <returns></returns>
		public static async Task<OneOf<TA, TB>> LiftTask<TA, TB>(this OneOf<Task<TA>, TB> oneOf)
		{
			var isA = oneOf.Extract(x => true, x => false);

			if (isA)
			{
				var a = await oneOf.Extract(x => x, x => throw new InvalidOperationException());
				return new OneOf<TA, TB>(a);
			}

			var b = oneOf.Extract(x => throw new InvalidOperationException(), x => x);
			return new OneOf<TA, TB>(b);
		}

		/// <summary>
		/// Lifts an inner Task out of the OneOf.
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TB"></typeparam>
		/// <param name="oneOf"></param>
		/// <returns></returns>
		public static async Task<OneOf<TA, TB>> LiftTask<TA, TB>(this OneOf<TA, Task<TB>> oneOf)
		{
			var isA = oneOf.Extract(x => true, x => false);

			if (isA)
			{
				var a = oneOf.Extract(x => x, x => throw new InvalidOperationException());
				return new OneOf<TA, TB>(a);
			}

			var b = await oneOf.Extract(x => throw new InvalidOperationException(), x => x);
			return new OneOf<TA, TB>(b);
		}

		/// <summary>
		/// Maps a function after awaiting the previous result.
		/// </summary>
		public static OneOf<Task<TANext>, Task<TBNext>> Map<TA, TB, TANext, TBNext>(
			this OneOf<Task<TA>, Task<TB>> oneOf,
			Func<TA, TANext> fA,
			Func<TB, TBNext> fB)
		{
			return oneOf.Map(
				async x => fA(await x),
				async x => fB(await x)
			);
		}

		/// <summary>
		/// Extracts a value after awaiting the previous result.
		/// </summary>
		public static Task<TResult> Extract<TA, TB, TResult>(
			this OneOf<Task<TA>, Task<TB>> oneOf,
			Func<TA, TResult> fA,
			Func<TB, TResult> fB
				)
		{
			return oneOf.Extract(
				async x => fA(await x),
				async x => fB(await x)
			);
		}
	}
}
