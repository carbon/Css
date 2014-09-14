﻿namespace Carbon.Css
{
	using System;

	public struct Browser
	{
		private readonly BrowserType type;
		private readonly float version;

		public Browser(BrowserType type, float version)
		{
			this.type = type;
			this.version = version;
		}

		public BrowserType Type
		{
			get { return type; }
		}

		public float Version
		{
			get { return version; }
		}

		public BrowserPrefix Prefix
		{
			get { return GetPrefix(type); }
		}

		public static Browser Chrome(float version)
		{
			return new Browser(BrowserType.Chrome, version);
		}

		public static Browser Firefox(float version)
		{
			return new Browser(BrowserType.Firefox, version);
		}

		public static Browser Safari(float version)
		{
			return new Browser(BrowserType.Safari, version);
		}

		public static Browser Opera(float version)
		{
			return new Browser(BrowserType.Opera, version);
		}

		public static Browser IE(float version)
		{
			return new Browser(BrowserType.IE, version);
		}

		public static readonly Browser Chrome1  = Chrome(1);
		public static readonly Browser Chrome4	= Chrome(4);
		public static readonly Browser Chrome7  = Chrome(7);
		public static readonly Browser Chrome10 = Chrome(10);
		public static readonly Browser Chrome13 = Chrome(13);
		public static readonly Browser Chrome26 = Chrome(26);
		public static readonly Browser Chrome36 = Chrome(36);

		public static readonly Browser Firefox1  = Firefox(1);
		public static readonly Browser Firefox4  = Firefox(4);
		public static readonly Browser Firefox5  = Firefox(5);
		public static readonly Browser Firefox6  = Firefox(6);
		public static readonly Browser Firefox9  = Firefox(9);
		public static readonly Browser Firefox10 = Firefox(10);
		public static readonly Browser Firefox16 = Firefox(16);
		public static readonly Browser Firefox20 = Firefox(20);
		public static readonly Browser Firefox21 = Firefox(21);
		public static readonly Browser Firefox29 = Firefox(29);

		public static readonly Browser IE6  = IE(6);
		public static readonly Browser IE7  = IE(7);
		public static readonly Browser IE8  = IE(8);
		public static readonly Browser IE9  = IE(9);
		public static readonly Browser IE10 = IE(10);
		public static readonly Browser IE11 = IE(11);

		public static readonly Browser Opera4  = Opera(3);
		public static readonly Browser Opera9  = Opera(9);
		public static readonly Browser Opera15 = Opera(15); // Based on Chromium

		public static readonly Browser Safari1 = Safari(1);
		public static readonly Browser Safari3 = Safari(3);
		public static readonly Browser Safari4 = Safari(4);
		public static readonly Browser Safari5 = Safari(5);
		public static readonly Browser Safari6 = Safari(6);

		public static BrowserPrefix GetPrefix(BrowserType type)
		{
			switch (type)
			{
				case BrowserType.Chrome:	return BrowserPrefix.Webkit;
				case BrowserType.Firefox:	return BrowserPrefix.Moz;
				case BrowserType.IE:		return BrowserPrefix.IE;
				case BrowserType.Opera:		return BrowserPrefix.Opera;
				case BrowserType.Safari:	return BrowserPrefix.Webkit;

				default:					throw new Exception("Unexpected browser: " + type);
			}
		}

		public override string ToString()
		{
			return type + "/" + version;
		}
	}

	public struct BrowserPrefix
	{
		public static readonly BrowserPrefix Moz	= new BrowserPrefix(BrowserPrefixKind.Moz);
		public static readonly BrowserPrefix IE		= new BrowserPrefix(BrowserPrefixKind.Ms);
		public static readonly BrowserPrefix Opera	= new BrowserPrefix(BrowserPrefixKind.O);
		public static readonly BrowserPrefix Webkit	= new BrowserPrefix(BrowserPrefixKind.Webkit);

		private readonly BrowserPrefixKind kind;

		public BrowserPrefix(BrowserPrefixKind kind)
		{
			this.kind = kind;
		}

		public BrowserPrefixKind Kind
		{
			get { return kind; }
		}

		public string Text
		{
			get
			{
				switch (kind)
				{
					case BrowserPrefixKind.Moz	  : return "-moz-";
					case BrowserPrefixKind.Ms	  : return "-ms-";
					case BrowserPrefixKind.Webkit : return "-webkit-";
					case BrowserPrefixKind.O      : return "-o-";
					default						  : return "";
				}

			}
		}

		public static implicit operator String(BrowserPrefix d)
		{
			return d.Text;
		}
	}


	[Flags]
	public enum BrowserPrefixKind : byte
	{
		None = 0,
		Moz = 1,
		Ms = 2,
		O = 4,
		Webkit = 8
	}

	[Flags]
	public enum BrowserType : byte
	{
		Unknown = 0,
		IE		= 1,
		Firefox = 2,
		Safari	= 4,
		Chrome	= 8,
		Opera	= 16
	}
}