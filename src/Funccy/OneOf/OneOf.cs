﻿/* This is a generated file. */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funccy
{

	/// <summary>
    /// A value that could be one of several types.
    /// </summary>
	public class OneOf<TA, TB> : ValueObject<OneOf<TA, TB>>
	{
		private readonly char _tag;
        private readonly TA _A;
        private readonly TB _B;

        public OneOf(TA valueA){
            _tag = 'A';
            _A = valueA;
        }

        public OneOf(TB valueB){
            _tag = 'B';
            _B = valueB;
        }

		public OneOf<TANext, TBNext> Map<TANext, TBNext>(
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB
		){
			switch(_tag){
                case 'A': return new OneOf<TANext, TBNext>(fA(_A));
                case 'B': return new OneOf<TANext, TBNext>(fB(_B));
				default: throw new InvalidOperationException();
			}
		}

		public TResult Extract<TResult>(
            Func<TA, TResult> fA,
            Func<TB, TResult> fB		
		){
			switch(_tag){
                case 'A': return fA(_A);
                case 'B': return fB(_B);
				default: throw new InvalidOperationException();
			}
		}

		protected override IEnumerable<object> GetEqualityComponents(){
			yield return _tag;
            yield return _A;
            yield return _B;
		}
	}


	/// <summary>
    /// A value that could be one of several types.
    /// </summary>
	public class OneOf<TA, TB, TC> : ValueObject<OneOf<TA, TB, TC>>
	{
		private readonly char _tag;
        private readonly TA _A;
        private readonly TB _B;
        private readonly TC _C;

        public OneOf(TA valueA){
            _tag = 'A';
            _A = valueA;
        }

        public OneOf(TB valueB){
            _tag = 'B';
            _B = valueB;
        }

        public OneOf(TC valueC){
            _tag = 'C';
            _C = valueC;
        }

		public OneOf<TANext, TBNext, TCNext> Map<TANext, TBNext, TCNext>(
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB,
            Func<TC, TCNext> fC
		){
			switch(_tag){
                case 'A': return new OneOf<TANext, TBNext, TCNext>(fA(_A));
                case 'B': return new OneOf<TANext, TBNext, TCNext>(fB(_B));
                case 'C': return new OneOf<TANext, TBNext, TCNext>(fC(_C));
				default: throw new InvalidOperationException();
			}
		}

		public TResult Extract<TResult>(
            Func<TA, TResult> fA,
            Func<TB, TResult> fB,
            Func<TC, TResult> fC		
		){
			switch(_tag){
                case 'A': return fA(_A);
                case 'B': return fB(_B);
                case 'C': return fC(_C);
				default: throw new InvalidOperationException();
			}
		}

		protected override IEnumerable<object> GetEqualityComponents(){
			yield return _tag;
            yield return _A;
            yield return _B;
            yield return _C;
		}
	}


	/// <summary>
    /// A value that could be one of several types.
    /// </summary>
	public class OneOf<TA, TB, TC, TD> : ValueObject<OneOf<TA, TB, TC, TD>>
	{
		private readonly char _tag;
        private readonly TA _A;
        private readonly TB _B;
        private readonly TC _C;
        private readonly TD _D;

        public OneOf(TA valueA){
            _tag = 'A';
            _A = valueA;
        }

        public OneOf(TB valueB){
            _tag = 'B';
            _B = valueB;
        }

        public OneOf(TC valueC){
            _tag = 'C';
            _C = valueC;
        }

        public OneOf(TD valueD){
            _tag = 'D';
            _D = valueD;
        }

		public OneOf<TANext, TBNext, TCNext, TDNext> Map<TANext, TBNext, TCNext, TDNext>(
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB,
            Func<TC, TCNext> fC,
            Func<TD, TDNext> fD
		){
			switch(_tag){
                case 'A': return new OneOf<TANext, TBNext, TCNext, TDNext>(fA(_A));
                case 'B': return new OneOf<TANext, TBNext, TCNext, TDNext>(fB(_B));
                case 'C': return new OneOf<TANext, TBNext, TCNext, TDNext>(fC(_C));
                case 'D': return new OneOf<TANext, TBNext, TCNext, TDNext>(fD(_D));
				default: throw new InvalidOperationException();
			}
		}

		public TResult Extract<TResult>(
            Func<TA, TResult> fA,
            Func<TB, TResult> fB,
            Func<TC, TResult> fC,
            Func<TD, TResult> fD		
		){
			switch(_tag){
                case 'A': return fA(_A);
                case 'B': return fB(_B);
                case 'C': return fC(_C);
                case 'D': return fD(_D);
				default: throw new InvalidOperationException();
			}
		}

		protected override IEnumerable<object> GetEqualityComponents(){
			yield return _tag;
            yield return _A;
            yield return _B;
            yield return _C;
            yield return _D;
		}
	}


	/// <summary>
    /// A value that could be one of several types.
    /// </summary>
	public class OneOf<TA, TB, TC, TD, TE> : ValueObject<OneOf<TA, TB, TC, TD, TE>>
	{
		private readonly char _tag;
        private readonly TA _A;
        private readonly TB _B;
        private readonly TC _C;
        private readonly TD _D;
        private readonly TE _E;

        public OneOf(TA valueA){
            _tag = 'A';
            _A = valueA;
        }

        public OneOf(TB valueB){
            _tag = 'B';
            _B = valueB;
        }

        public OneOf(TC valueC){
            _tag = 'C';
            _C = valueC;
        }

        public OneOf(TD valueD){
            _tag = 'D';
            _D = valueD;
        }

        public OneOf(TE valueE){
            _tag = 'E';
            _E = valueE;
        }

		public OneOf<TANext, TBNext, TCNext, TDNext, TENext> Map<TANext, TBNext, TCNext, TDNext, TENext>(
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB,
            Func<TC, TCNext> fC,
            Func<TD, TDNext> fD,
            Func<TE, TENext> fE
		){
			switch(_tag){
                case 'A': return new OneOf<TANext, TBNext, TCNext, TDNext, TENext>(fA(_A));
                case 'B': return new OneOf<TANext, TBNext, TCNext, TDNext, TENext>(fB(_B));
                case 'C': return new OneOf<TANext, TBNext, TCNext, TDNext, TENext>(fC(_C));
                case 'D': return new OneOf<TANext, TBNext, TCNext, TDNext, TENext>(fD(_D));
                case 'E': return new OneOf<TANext, TBNext, TCNext, TDNext, TENext>(fE(_E));
				default: throw new InvalidOperationException();
			}
		}

		public TResult Extract<TResult>(
            Func<TA, TResult> fA,
            Func<TB, TResult> fB,
            Func<TC, TResult> fC,
            Func<TD, TResult> fD,
            Func<TE, TResult> fE		
		){
			switch(_tag){
                case 'A': return fA(_A);
                case 'B': return fB(_B);
                case 'C': return fC(_C);
                case 'D': return fD(_D);
                case 'E': return fE(_E);
				default: throw new InvalidOperationException();
			}
		}

		protected override IEnumerable<object> GetEqualityComponents(){
			yield return _tag;
            yield return _A;
            yield return _B;
            yield return _C;
            yield return _D;
            yield return _E;
		}
	}


	public static class OneOfExtensions {

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

        /// <summary>
        /// Maps a function after awaiting the previous result.
        /// </summary>
		public static OneOf<Task<TANext>, Task<TBNext>, Task<TCNext>> Map<TA, TB, TC, TANext, TBNext, TCNext>(
			this OneOf<Task<TA>, Task<TB>, Task<TC>> oneOf,
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB,
            Func<TC, TCNext> fC)
			{
				return oneOf.Map(
                    async x => fA(await x),
                    async x => fB(await x),
                    async x => fC(await x)
				);
			}

        /// <summary>
        /// Extracts a value after awaiting the previous result.
        /// </summary>
		public static Task<TResult> Extract<TA, TB, TC, TResult>(
			this OneOf<Task<TA>, Task<TB>, Task<TC>> oneOf,
            Func<TA, TResult> fA,
            Func<TB, TResult> fB,
            Func<TC, TResult> fC
				)
			{
				return oneOf.Extract(
                    async x => fA(await x),
                    async x => fB(await x),
                    async x => fC(await x)
				);
			}

        /// <summary>
        /// Maps a function after awaiting the previous result.
        /// </summary>
		public static OneOf<Task<TANext>, Task<TBNext>, Task<TCNext>, Task<TDNext>> Map<TA, TB, TC, TD, TANext, TBNext, TCNext, TDNext>(
			this OneOf<Task<TA>, Task<TB>, Task<TC>, Task<TD>> oneOf,
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB,
            Func<TC, TCNext> fC,
            Func<TD, TDNext> fD)
			{
				return oneOf.Map(
                    async x => fA(await x),
                    async x => fB(await x),
                    async x => fC(await x),
                    async x => fD(await x)
				);
			}

        /// <summary>
        /// Extracts a value after awaiting the previous result.
        /// </summary>
		public static Task<TResult> Extract<TA, TB, TC, TD, TResult>(
			this OneOf<Task<TA>, Task<TB>, Task<TC>, Task<TD>> oneOf,
            Func<TA, TResult> fA,
            Func<TB, TResult> fB,
            Func<TC, TResult> fC,
            Func<TD, TResult> fD
				)
			{
				return oneOf.Extract(
                    async x => fA(await x),
                    async x => fB(await x),
                    async x => fC(await x),
                    async x => fD(await x)
				);
			}

        /// <summary>
        /// Maps a function after awaiting the previous result.
        /// </summary>
		public static OneOf<Task<TANext>, Task<TBNext>, Task<TCNext>, Task<TDNext>, Task<TENext>> Map<TA, TB, TC, TD, TE, TANext, TBNext, TCNext, TDNext, TENext>(
			this OneOf<Task<TA>, Task<TB>, Task<TC>, Task<TD>, Task<TE>> oneOf,
            Func<TA, TANext> fA,
            Func<TB, TBNext> fB,
            Func<TC, TCNext> fC,
            Func<TD, TDNext> fD,
            Func<TE, TENext> fE)
			{
				return oneOf.Map(
                    async x => fA(await x),
                    async x => fB(await x),
                    async x => fC(await x),
                    async x => fD(await x),
                    async x => fE(await x)
				);
			}

        /// <summary>
        /// Extracts a value after awaiting the previous result.
        /// </summary>
		public static Task<TResult> Extract<TA, TB, TC, TD, TE, TResult>(
			this OneOf<Task<TA>, Task<TB>, Task<TC>, Task<TD>, Task<TE>> oneOf,
            Func<TA, TResult> fA,
            Func<TB, TResult> fB,
            Func<TC, TResult> fC,
            Func<TD, TResult> fD,
            Func<TE, TResult> fE
				)
			{
				return oneOf.Extract(
                    async x => fA(await x),
                    async x => fB(await x),
                    async x => fC(await x),
                    async x => fD(await x),
                    async x => fE(await x)
				);
			}
		}
}