namespace Carbon.Color
{
	using System;

	public struct Hsla
	{
		private readonly float h;
		private readonly float s;
		private readonly float l;
		private readonly float a;

		public Hsla(float h, float s, float l, float a = 1f)
		{
			this.h = h;
			this.s = s;
			this.l = l;
			this.a = a;
		}

		public float H => h;
		public float S => s;
		public float L => l;
		public float A => a;

		public override string ToString()
		{
			return string.Format("hsla({0},{1}%,{2}%,{3})", (
				/*0*/ (int)Math.Round(H * 360f)),
				/*1*/ (int)Math.Round(S * 100f), 
				/*2*/ (int)Math.Round(L * 100f),
				/*3*/ 1
			);
		}

		public Hsla AdjustLightness(float value)
		{
			return new Hsla(h, s, Constrain(l + value), a);
		}

		public Hsla RotateHue(float degrees)
		{
			return WithHueDegrees(HueDegrees + degrees);
		}

		public Hsla AdjustSaturation(float value)
		{
			// var newValue = s.Lerp(1f, value);

			return WithS(Constrain(s + value));
		}

		public Hsla WithL(float value)
		{
			return new Hsla(h, s, value, a);
		}

		public Hsla WithS(float saturation)
		{
			return new Hsla(h, saturation, l, a);
		}

		public Hsla WithHue(float hue)
		{
			return new Hsla(hue, s, l, a);
		}

		public float HueDegrees
		{
			get 
			{
				// The next thing you need to understand is that we’re taking integer RGB values from 0 to 255 and converting them to decimal values from 0 to 1. 
				
				// The HSL that we get back will thus need to be converted to the normal degree/percent/percent that you’re used to. 
				// The H value returned should be from 0 to 6 so to convert it to degrees you just multiply by 60.
				// H can actually be negative sometimes so if it is just add 360;

				var degrees = h * 60f;
				
				if (degrees < 0) degrees += 360;

				return degrees;
			}
		}

		public Hsla WithHueDegrees(float degrees)
		{
			degrees = (degrees % 360f);

			// - 360f
			var value = degrees / 60f;

			// 0 - 6
			return WithHue(value);
		}

		private float Constrain(float value)
		{
			if (value > 1) return 1f;
			if (value < 0) return 0f;

			return value;
		}

		public WebColor ToRgb()
		{
			byte r, g, b;

			if (s == 0d)
			{
				r = (byte)Math.Round(l * 255f);
				g = (byte)Math.Round(l * 255f);
				b = (byte)Math.Round(l * 255f);
			}
			else
			{
				float t1, t2;
				float th = h / 6.0f;

				if (l < 0.5f)
				{
					t2 = l * (1f + s);
				}
				else
				{
					t2 = (l + s) - (l * s);
				}

				t1 = 2f * l - t2;

				float tr, tg, tb;

				tr = th + (1.0f / 3.0f);
				tg = th;
				tb = th - (1.0f / 3.0f);

				tr = ColorCalc(tr, t1, t2);
				tg = ColorCalc(tg, t1, t2);
				tb = ColorCalc(tb, t1, t2);

				r = (byte)Math.Round(tr * 255f);
				g = (byte)Math.Round(tg * 255f);
				b = (byte)Math.Round(tb * 255f);
			}
			return new WebColor(r, g, b, a);
		}

		public static Hsla FromRgb(WebColor color)
		{
			float r = (color.R / 255f);
			float g = (color.G / 255f);
			float b = (color.B / 255f);

			float min = Math.Min(Math.Min(r, g), b);
			float max = Math.Max(Math.Max(r, g), b);
			float delta = max - min;

			float h = 0;
			float s = 0;
			float l = (float)((max + min) / 2.0f);

			if (delta != 0)
			{
				if (l < 0.5f)
				{
					s = (float)(delta / (max + min));
				}
				else
				{
					s = (float)(delta / (2.0f - max - min));
				}

				if (r == max)
				{
					h = (g - b) / delta;
				}
				else if (g == max)
				{
					h = 2f + (b - r) / delta;
				}
				else if (b == max)
				{
					h = 4f + (r - g) / delta;
				}
			}

			return new Hsla(h, s, l, color.A);
        }

		private static float ColorCalc(float c, float t1, float t2)
		{
			if (c < 0) c += 1f;
			if (c > 1) c -= 1f;
			if (6.0f * c < 1.0f) return t1 + (t2 - t1) * 6.0f * c;
			if (2.0f * c < 1.0f) return t2;
			if (3.0f * c < 2.0f) return t1 + (t2 - t1) * (2.0f / 3.0f - c) * 6.0f;

			return t1;
		}
	}
}