namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using System.Text;

	public class ChangeSet
	{
		private readonly List<CssDeclaration> add = new List<CssDeclaration>();
		private readonly List<CssDeclaration> remove = new List<CssDeclaration>();

		public CssDeclaration Source { get; set; }

		public List<CssDeclaration> AddList
		{
			get { return add; }
		}

		public List<CssDeclaration> RemoveList
		{
			get { return remove; }
		}
	}

	public class DefaultRuleTransformer
	{
		private readonly List<ICssTransform> transforms = new List<ICssTransform> {
			new IEOpacityTransform(),
			new AddVendorPrefixesTransform()
		};

		public void Transform(CssRule rule)
		{
			foreach(var transform in transforms)
			{
				if(transform.Matches(rule))
				{
					foreach (var changes in transform.GetChanges(rule))
					{
						var indexOf = rule.Block.Declarations.IndexOf(changes.Source);

						foreach (var add in changes.AddList)
						{
							rule.Block.Declarations.Insert(indexOf, add);
						}

						foreach (var remove in changes.RemoveList)
						{
							rule.Block.Remove(remove);
						}
					}
				}
			}
		}
	}
}
