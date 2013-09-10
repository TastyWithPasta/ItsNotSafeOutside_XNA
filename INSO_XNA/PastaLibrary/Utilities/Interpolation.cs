using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PastaGameLibrary
{
	public interface IPInterpolation<T>
	{
		T GetInterpolation(T from, T to, float ratio);
	}

	public class PLerpInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * ratio;
		}
	}
	public class PSquareInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			ratio = ratio * 2 - 1;
			return from + (to - from) * ratio * ratio;
		}
	}
	public class PSineHalfInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * (float)(Math.Sin(ratio * Math.PI * 0.5f));
		}
	}
	public class PSineInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * (float)(Math.Sin(ratio * Math.PI * 2.0));
		}
	}
	public class PSinePositiveInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * (float)(Math.Sin(ratio * Math.PI * 2.0) + 1) * 0.5f;
		}
	}
	public class PSmoothstepInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * ratio * ratio * (3 - 2 * ratio);
		}
	}
}
