using System.Globalization;

namespace Carbon.Css
{
    public sealed class CssModule : CssCompatibility
    {
        public CssModule(
            CssModuleType type, 
            float level,
            CompatibilityTable prefixed = new (),
            CompatibilityTable standard = new ())
            : base(prefixed, standard)
        {
            Type = type;
            Level = level;
        }

        public CssModuleType Type { get; }

        public float Level { get; }

        public override string ToString() => Type + " Level " + Level.ToString(CultureInfo.InvariantCulture);

        public static readonly CssModule Core1 = new (CssModuleType.Core, 1,
            standard: new CompatibilityTable(chrome: 1, firefox: 1, safari: 1)
        );

        public static readonly CssModule Core2 = new (CssModuleType.Core, 2,
            standard: new CompatibilityTable(chrome: 1, edge: 6, firefox: 1, safari: 1)
        );

        public static readonly CssModule Core2_1 = new (CssModuleType.Core, 2.1f,
            standard: new CompatibilityTable(chrome: 1, edge: 8, firefox: 1, safari: 1)
        );

        #region Animations

        public static readonly CssModule Animations3 = new (CssModuleType.Animations, 3,
            prefixed: new CompatibilityTable(chrome: 1,            firefox: 5, safari: 4),
            standard: new CompatibilityTable(chrome: 26, edge: 10, firefox: 16, safari: 9)
        );

        #endregion

        #region Background & Borders

        public static readonly CssModule BackgroundsAndBorders3 = new (CssModuleType.BackgroundsAndBorders, 3);

        #endregion

        #region Color

        public static readonly CssModule Color3 = new (CssModuleType.Color, 3f,
            standard: new CompatibilityTable(chrome: 1, edge: 9, firefox: 1, safari: 1.2f)
        );

        #endregion

        #region Columns

        // Columns (Level 3)
        public static readonly CssModule Columns3 = new (CssModuleType.Columns, 3,
            prefixed: new CompatibilityTable(chrome: 10,           firefox: 9, safari: 7),
            standard: new CompatibilityTable(chrome: 50, edge: 10, firefox: 52, safari: 9)
        );


        #endregion

        #region Flexbox

        // Flexbox (Level 1)
        public static readonly CssModule Flexbox1 = new (
            type     : CssModuleType.Flexbox,
            level    : 1,
            prefixed : new CompatibilityTable(chrome: 21),
            standard : new CompatibilityTable(chrome: 29, edge: 12, firefox: 28, safari: 9)
        );

        #endregion

        #region Fonts

        public static readonly CssModule Fonts3 = new (CssModuleType.Fonts, 3);

        #endregion

        #region Masking

        // https://www.w3.org/TR/css-masking-1/
        // https://caniuse.com/#feat=css-masks
        public static readonly CssModule Masking_1 = new (CssModuleType.Masking, 1,
            prefixed : new CompatibilityTable(chrome: 4, safari: 4),
            standard : new CompatibilityTable(firefox: 53)
        );

        #endregion

        #region Ruby

        public static CssModule Ruby(float level) => new (CssModuleType.Ruby, level);

        public static readonly CssModule Ruby3 = new (CssModuleType.Ruby, 3);

        #endregion

        #region Text

        public static readonly CssModule Text3 = new (CssModuleType.Text, 3);

        #endregion

        #region Transforms

        public static readonly CssModule Transforms3 = new (CssModuleType.Transforms, 3,
            prefixed: new CompatibilityTable(chrome: 10, edge: 9,  firefox: 3.5f, safari: 4),
            standard: new CompatibilityTable(chrome: 36, edge: 10, firefox: 16,   safari: 9)
        );

        #endregion

        #region Transitions

        // Unsupported in IE9
        // Standard in IE10

        public static readonly CssModule Transitions3 = new (CssModuleType.Transitions, 3,
            prefixed: new CompatibilityTable(chrome: 1,            firefox: 4,  safari: 3),
            standard: new CompatibilityTable(chrome: 26, edge: 10, firefox: 20, safari: 9)
        )
        { PatchValues = true };

        // TODO: Limit value patch scope to transition

        #endregion

        public static CssModule UI(float level) => new (CssModuleType.UI, level);
    }
}
