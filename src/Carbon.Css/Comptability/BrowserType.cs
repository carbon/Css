﻿using System;

namespace Carbon.Css
{
    [Flags]
    public enum BrowserType
    {
        Unknown = 0,
        IE = 1,
        Firefox = 2,
        Safari = 4,
        Chrome = 8,
        Opera = 16
    }
}