namespace Carbon.Css
{
    public sealed class CssModule : CssCompatibility
    {
        public CssModule(CssModuleType type, float level,
            CompatibilityTable prefixed = new CompatibilityTable(),
            CompatibilityTable standard = new CompatibilityTable())
            : base(prefixed, standard)
        {
            Type = type;
            Level = level;
        }

        public CssModuleType Type { get; }

        public float Level { get; }

        public override string ToString() => Type + " Level " + Level;

        public static readonly CssModule Core1 = new CssModule(CssModuleType.Core, 1,
            standard: new CompatibilityTable(chrome: 1, firefox: 1, safari: 1)
        );

        public static readonly CssModule Core2 = new CssModule(CssModuleType.Core, 2,
            standard: new CompatibilityTable(chrome: 1, firefox: 1, ie: 6, safari: 1)
        );

        public static readonly CssModule Core2_1 = new CssModule(CssModuleType.Core, 2.1f,
            standard: new CompatibilityTable(chrome: 1, firefox: 1, ie: 8, safari: 1)
        );

        #region Animations

        public static readonly CssModule Animations3 = new CssModule(CssModuleType.Animations, 3,
            prefixed: new CompatibilityTable(chrome: 1, firefox: 5, safari: 4),
            standard: new CompatibilityTable(chrome: 26, firefox: 16, ie: 10, safari: 9)
        );

        #endregion

        #region Background & Borders

        public static readonly CssModule BackgroundsAndBorders3 = new CssModule(CssModuleType.BackgroundsAndBorders, 3);

        #endregion

        #region Color

        public static CssModule Color3 = new CssModule(CssModuleType.Color, 3f,
            standard: new CompatibilityTable(chrome: 1, firefox: 1, ie: 9, safari: 1.2f)
        );

        #endregion

        #region Columns

        // Columns (Level 3)
        public static readonly CssModule Columns3 = new CssModule(CssModuleType.Columns, 3,
            prefixed: new CompatibilityTable(chrome: 10, firefox: 9, safari: 7),
            standard: new CompatibilityTable(ie: 10, chrome: 50, safari: 9)
        );


        #endregion

        #region Flexbox

        // Flexbox (Level 1)
        public static readonly CssModule Flexbox1 = new CssModule(
            type     : CssModuleType.Flexbox,
            level    : 1,
            prefixed : new CompatibilityTable(chrome: 21),
            standard : new CompatibilityTable(chrome: 29, firefox: 28, safari: 9)
        );

        #endregion

        #region Fonts

        public static readonly CssModule Fonts3 = new CssModule(CssModuleType.Fonts, 3);

        #endregion

        #region Masking

        // https://www.w3.org/TR/css-masking-1/
        // https://caniuse.com/#feat=css-masks
        public static readonly CssModule Masking_1 = new CssModule(CssModuleType.Masking, 1,
            prefixed : new CompatibilityTable(chrome: 4, safari: 4),
            standard : new CompatibilityTable(firefox: 53, ie: 18)
        );

        #endregion

        #region Ruby

        public static CssModule Ruby(float level) => new CssModule(CssModuleType.Ruby, level);

        public static readonly CssModule Ruby3 = new CssModule(CssModuleType.Ruby, 3);

        #endregion

        #region Text

        public static readonly CssModule Text3 = new CssModule(CssModuleType.Text, 3);

        #endregion

        #region Transforms

        public static readonly CssModule Transforms3 = new CssModule(CssModuleType.Transforms, 3,
            prefixed: new CompatibilityTable(chrome: 10, firefox: 3.5f, ie: 9, safari: 4),
            standard: new CompatibilityTable(chrome: 36, firefox: 16, ie: 10, safari: 9)
        );

        #endregion

        #region Transitions

        // Unsupported in IE9
        // Standard in IE10

        public static readonly CssModule Transitions3 = new CssModule(CssModuleType.Transitions, 3,
            prefixed: new CompatibilityTable(chrome: 1, firefox: 4, safari: 3),
            standard: new CompatibilityTable(chrome: 26, firefox: 20, ie: 10, safari: 9)
        )
        { PatchValues = true };

        // TODO: Limit value patch scope to transition

        #endregion


        public static CssModule UI(float level) => new CssModule(CssModuleType.UI, level);
    }
}
