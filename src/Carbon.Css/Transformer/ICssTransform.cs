namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;

	public interface ICssTransform
	{
		bool Matches(CssRule declaration);

		ChangeSet[] GetChanges(CssRule rule);
	}

	public class IEOpacityTransform : ICssTransform
	{
		public bool Matches(CssRule rule)
		{
			return (rule.Any(b => b.Name == "opacity"));
		}

		public ChangeSet[] GetChanges(CssRule rule)
		{
			var declaration = rule.Get("opacity");
			
			var cssValue = declaration.Value;

			var changes = new ChangeSet();

			changes.Source = declaration;

			int value = -1;

			try { value = (int)(float.Parse(cssValue.ToString()) * 100); }
			catch { }

			if (value > -1)
			{
				// Remove any existing filters
				foreach (var filter in rule.FindHavingPropertyName("filter").Where(f => f.Value.ToString().Contains("alpha")))
				{
					changes.RemoveList.Add(filter);
				}

				changes.AddList.Add(new CssDeclaration("filter", string.Format("alpha(opacity={0})", value.ToString())));
			}

			return new[] { changes };
		}
	}

	public class AddVendorPrefixesTransform : ICssTransform
	{
		public bool Matches(CssRule rule)
		{
			return rule.Any(d => { 
				var info = CssPropertyInfo.Get(d.Name);

				return info != null && info.Compatibility != null && info.Compatibility.Prefixed != null;
			});
		}

		public ChangeSet[] GetChanges(CssRule rule)
		{
			var changeSets = new List<ChangeSet>();

			foreach (var declaration in rule)
			{
				var propInfo = CssPropertyInfo.Get(declaration.Name);

				if (propInfo == null || propInfo.Compatibility == null || propInfo.Compatibility.Prefixed == null) continue;

				var cssValue = declaration.Value;

				var changes = new ChangeSet();

				changes.Source = declaration;

				foreach (var prefixedName in propInfo.Compatibility.GetPrefixes(declaration.Name))
				{
					// Remove existing prefixes
					changes.RemoveList.AddRange(rule.FindHavingPropertyName(prefixedName));

					changes.AddList.Add(new CssDeclaration(prefixedName, cssValue));
				}

				changeSets.Add(changes);
			}

			return changeSets.ToArray();
		}
	}

}