# Carbon.Css

A general purpose CSS parser, auto-prefixer, and SCSS compiler for .NET Standard.


# Usage

```
var css = StyleSheet.Parse(@"
//= support Safari 5+
$backgroundColor: #000;

html {
  background: $backgroundColor; 
  font-size: 14px; 
}

");

var writer = new StringWriter();

css.WriteTo(writer);

```
