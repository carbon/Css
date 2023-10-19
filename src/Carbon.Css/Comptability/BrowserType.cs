namespace Carbon.Css;

[Flags]
public enum BrowserType
{
    Unknown = 0,
    Chrome  = 1,
    Edge    = 2,
    Firefox = 4, // -moz
    Safari  = 8  // -webkit
}