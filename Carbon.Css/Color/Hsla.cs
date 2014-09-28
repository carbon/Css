namespace Carbon.Css.Color
{
	using System;
	using System.Linq;

	public struct Hsla
	{
		private readonly double h;
		private readonly double s;
		private readonly double l;
		private readonly double a;

		public Hsla(double h, double s, double l, double a = 1d)
		{
			this.h = h;
			this.s = s;
			this.l = l;
			this.a = a;
		}

		public double H
		{
			get { return h; }
		}

		public double S
		{
			get { return s; }
		}

		public double L
		{
			get { return l; }
		}

		public double A
		{
			get { return a; }
		}

		public override string ToString()
		{
			return string.Format("hsla({0},{1}%,{2}%,{3})", (
				/*0*/ (int)Math.Round(H * 360d)),
				/*1*/ (int)Math.Round(S * 100), 
				/*2*/ (int)Math.Round(L * 100),
				/*3*/ 1
			);
		}

		public Hsla Saturate(float value)
		{
			return new Hsla(h, s * (1f + value), l, a);
		}

		public Hsla WithL(double value)
		{
			return new Hsla(h, s, value, a);
		}

		public Hsla WithS(double value)
		{
			return new Hsla(h, value, l, a);
		}


		public WebColor ToRgb()
		{
			byte r, g, b;

			if (s == 0d)
			{
				r = (byte)Math.Round(l * 255d);
				g = (byte)Math.Round(l * 255d);
				b = (byte)Math.Round(l * 255d);
			}
			else
			{
				double t1, t2;
				double th = h / 6.0d;

				if (l < 0.5d)
				{
					t2 = l * (1d + s);
				}
				else
				{
					t2 = (l + s) - (l * s);
				}

				t1 = 2d * l - t2;

				double tr, tg, tb;

				tr = th + (1.0d / 3.0d);
				tg = th;
				tb = th - (1.0d / 3.0d);

				tr = ColorCalc(tr, t1, t2);
				tg = ColorCalc(tg, t1, t2);
				tb = ColorCalc(tb, t1, t2);

				r = (byte)Math.Round(tr * 255d);
				g = (byte)Math.Round(tg * 255d);
				b = (byte)Math.Round(tb * 255d);
			}
			return new WebColor(r, g, b);
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

			return new Hsla(h, s, l, color.Alpha);
        }
       

		private static double ColorCalc(double c, double t1, double t2)
		{

			if (c < 0) c += 1d;
			if (c > 1) c -= 1d;
			if (6.0d * c < 1.0d) return t1 + (t2 - t1) * 6.0d * c;
			if (2.0d * c < 1.0d) return t2;
			if (3.0d * c < 2.0d) return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
			return t1;
		}
	
	}
}
