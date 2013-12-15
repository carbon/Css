namespace Carbon.Css
{
	public class CssModule : Compatibility
	{
		private readonly CssModuleType type;
		private readonly float level;

		public CssModule(CssModuleType type, float level)
		{
			this.type = type;
			this.level = level;
		}

		public CssModuleType Type
		{
			get { return type; }
		}

		public override string ToString()
		{
			return type + " Level " + level;
		}

		public static readonly CssModule Core1		= new CssModule(CssModuleType.Core, 1)			{ Standard = new[] { Browser.IE6, Browser.Chrome1, Browser.Firefox1, Browser.Safari1 } };
		public static readonly CssModule Css2		= new CssModule(CssModuleType.Core, 2)			{ Standard = new[] { Browser.IE7, Browser.Chrome1, Browser.Firefox1, Browser.Safari1 } };
		public static readonly CssModule Core21		= new CssModule(CssModuleType.Core, 2.1f)		{ Standard = new[] { Browser.IE8, Browser.Chrome1, Browser.Firefox1, Browser.Safari1 } };

		#region Animations

		public static readonly CssModule Animations3 = new CssModule(CssModuleType.Animations, 3) {
			Prefixed = new[] { Browser.Chrome1,  Browser.Firefox5,  Browser.Safari4 },
			Standard = new[] { Browser.Chrome26, Browser.Firefox16, Browser.IE10 }	
		};

		#endregion

		#region Background & Borders

		public static readonly CssModule BackgroundsAndBorders3 = new CssModule(CssModuleType.BackgroundsAndBorders, 3);

		#endregion

		#region Color

		public static CssModule Color3 = new CssModule(CssModuleType.Columns, 3f) {
			Standard = new[] { Browser.Chrome1, Browser.Firefox1, Browser.Safari(1.2f), Browser.Opera9, Browser.IE9 }
		};

		#endregion

		#region Columns

		// Columns (Level 3)
		public static readonly CssModule Columns3 = new CssModule(CssModuleType.Columns, 3)  {
			Prefixed = new[] { Browser.Chrome10, Browser.Firefox9, Browser.Safari3 },
			Standard = new[] { Browser.IE10, Browser.Opera(11.1f) }
		};

		#endregion

		#region Fonts

		public static readonly CssModule Fonts3 = new CssModule(CssModuleType.Fonts, 3);

		#endregion

		#region Ruby

		public static CssModule Ruby(float level)
		{
			return new CssModule(CssModuleType.Ruby, level);
		}

		#endregion

		#region Text

		public static readonly CssModule Text3 = new CssModule(CssModuleType.Text, 3);

		#endregion

		#region Transforms

		public static readonly CssModule Transforms3 = new CssModule(CssModuleType.Transforms, 3) {
			Prefixed = new[] { Browser.Chrome10, Browser.Firefox(3.5f), Browser.IE9, Browser.Opera(10.5f), Browser.Safari4 },
			Standard = new[] { Browser.Chrome26, Browser.IE10 }
		};

		#endregion

		#region Transitions

		public static readonly CssModule Transitions3 = new CssModule(CssModuleType.Transitions, 3) {
			Prefixed = new[] { Browser.Chrome1, Browser.Firefox4, Browser.Opera(10.6f), Browser.Safari3 },
			Standard = new[] { Browser.Chrome26, Browser.Firefox20, Browser.IE10 }
		};

		#endregion

		public static CssModule UI(float level)
		{
			return new CssModule(CssModuleType.UI, level);
		}
	}
}
