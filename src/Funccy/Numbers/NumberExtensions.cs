/* This is a generated file. */

using System;

namespace Funccy
{
	public static class NumberExtensions
	{
		public static bool IsNear(this Int16 source, Int16 target, Int16 tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this UInt16 source, UInt16 target, UInt16 tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this Int32 source, Int32 target, Int32 tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this UInt32 source, UInt32 target, UInt32 tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this Int64 source, Int64 target, Int64 tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this UInt64 source, UInt64 target, UInt64 tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this Single source, Single target, Single tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this Double source, Double target, Double tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

		public static bool IsNear(this Decimal source, Decimal target, Decimal tolerance)
		{
			var upper = target + tolerance;
			var lower = target - tolerance;
		
			if(source > upper){ return false; }
			if(source < lower){ return false; }
			return true;
		}

	}
}
