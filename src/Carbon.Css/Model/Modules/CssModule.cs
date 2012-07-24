namespace Carbon.Css
{
	public class CssModule
	{
		private readonly CssModuleType type;
		private readonly float level;

		public CssModule(CssModuleType type, float level)
		{
			this.type = type;
			this.level = level;
		}

		public static readonly CssModule Css1 = new CssModule(CssModuleType.Core, 1);

		#region Animations

		public static CssModule Animations(float level)
		{
			return new CssModule(CssModuleType.Animations, level);
		}

		public static readonly CssModule Animations3 = new CssModule(CssModuleType.Animations, 3)
		{
			Compatibility = new Compatibility {
				Prefixed = new[] { Browser.Chrome1, Browser.Firefox5, Browser.Safari4 },
				Standard = new[] { Browser.IE10 }
			}
		};

		#endregion

		#region Background & Borders

		public static readonly CssModule BackgroundsAndBorders3 = new CssModule(CssModuleType.BackgroundsAndBorders, 3);

		#endregion

		#region Columns

		public static CssModule Columns(float level)
		{
			return new CssModule(CssModuleType.Columns, level);
		}

		// Columns (Level 3)
		public static readonly CssModule Columns3 = new CssModule(CssModuleType.Columns, 3)
		{
			Compatibility = new Compatibility {
				Prefixed = new[] { Browser.Chrome10, Browser.Firefox9, Browser.Safari3 },
				Standard = new[] { Browser.IE10, Browser.Opera(11.1f) }
			}
		};

		#endregion

		#region Fonts

		public static readonly CssModule Fonts3 = new CssModule(CssModuleType.Fonts, 3);

		#endregion

		public static CssModule Ruby(float level)
		{
			return new CssModule(CssModuleType.Ruby, level);
		}

		#region Text

		public static readonly CssModule Text3 = new CssModule(CssModuleType.Text, 3);

		#endregion

		#region Transforms

		public static readonly CssModule Transforms3 = new CssModule(CssModuleType.Transforms, 3) {
			Compatibility = new Compatibility {
				Prefixed = new[] { Browser.Chrome10, Browser.Firefox(3.5f), Browser.IE9, Browser.Opera(10.5f), Browser.Safari4 },
				Standard = new[] { Browser.IE10 }
			}
		};

		#endregion

		#region Transitions

		public static readonly CssModule Transitions3 = new CssModule(CssModuleType.Transitions, 3) {
			Compatibility = new Compatibility {
				Prefixed = new[] { Browser.Chrome1, Browser.Firefox4, Browser.Opera(10.6f), Browser.Safari3 },
				Standard = new[] { Browser.IE10 }
			}
		};

		#endregion

		public static CssModule UI(float level)
		{
			return new CssModule(CssModuleType.UI, level);
		}

		public Compatibility Compatibility { get; set; }
	}
}
