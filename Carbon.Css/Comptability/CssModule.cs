namespace Carbon.Css
{
	public class CssModule : CssCompatibility
	{
		private readonly CssModuleType type;
		private readonly float level;

		public CssModule(CssModuleType type, float level, 
			CompatibilityTable? prefixed = null,
			CompatibilityTable? standard = null)
			: base(prefixed, standard)
		{
			this.type = type;
			this.level = level;
		}

		public CssModuleType Type => type;

		public override string ToString() => type + " Level " + level;

		public static readonly CssModule Core1 = new CssModule(CssModuleType.Core, 1,
			standard: new CompatibilityTable { Chrome = 1, Firefox = 1, IE = 6, Safari = 1 }
		);

		public static readonly CssModule Core2 = new CssModule(CssModuleType.Core, 2,
			standard: new CompatibilityTable { Chrome = 1, Firefox = 1, IE = 6, Safari = 1 }
		);

		public static readonly CssModule Core2_1 = new CssModule(CssModuleType.Core, 2.1f,
			standard: new CompatibilityTable { Chrome = 1, Firefox = 1, IE = 8, Safari = 1 }
		);

		#region Animations

		public static readonly CssModule Animations3 = new CssModule(CssModuleType.Animations, 3,
			prefixed : new CompatibilityTable { Chrome = 1, Firefox = 5, Safari = 4 },
			standard : new CompatibilityTable { Chrome = 26, Firefox = 16, IE = 10 }
		);

		#endregion

		#region Background & Borders

		public static readonly CssModule BackgroundsAndBorders3 = new CssModule(CssModuleType.BackgroundsAndBorders, 3);

		#endregion

		#region Color

		public static CssModule Color3 = new CssModule(CssModuleType.Columns, 3f,
			standard: new CompatibilityTable { Chrome = 1, Firefox = 1, IE = 9, Safari = 1.2f }
		);

		#endregion

		#region Columns

		// Columns (Level 3)
		public static readonly CssModule Columns3 = new CssModule(CssModuleType.Columns, 3,
			prefixed: new CompatibilityTable { Chrome = 10, Firefox = 9, Safari = 3 },
			standard: new CompatibilityTable { IE = 10 }
		);

		#endregion

		#region Fonts

		public static readonly CssModule Fonts3 = new CssModule(CssModuleType.Fonts, 3);

		#endregion

		#region Masking

		public static readonly CssModule Masking1 = new CssModule(CssModuleType.Masking, 1);


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

		// IE: prefixed(9), standard(10)

		public static readonly CssModule Transforms3 = new CssModule(CssModuleType.Transforms, 3,
			prefixed: new CompatibilityTable { Chrome = 10, Firefox = 3.5f, IE = 9,  Safari = 4 },
			standard: new CompatibilityTable { Chrome = 36, Firefox = 16, IE = 10  }
		);

		#endregion

		#region Transitions

		// Unsupported in IE9
		// Standard in IE10

		public static readonly CssModule Transitions3 = new CssModule(CssModuleType.Transitions, 3,
			prefixed: new CompatibilityTable { Chrome = 1, Firefox = 4,  Safari = 3 },
			standard: new CompatibilityTable { Chrome = 26, Firefox = 20, IE = 10 }
		) { HasValuePatches = true };

		// TODO: Limit value patch scope to transition

		#endregion

		public static CssModule UI(float level)
		{
			return new CssModule(CssModuleType.UI, level);
		}
	}
}
