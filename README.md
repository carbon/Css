# Carbon.Css

A general purpose CSS parser, auto-prefixer, and SCSS compiler for .NET Standard.


# Usage

```
var css = StyleSheet.Parse(@"
//= support Safari 5+
$backgroundColor:blue;

html {
  background: $backgroundColor="blue"; 
  font-size: 40px; 
}

");

var writer = new StringWriter();

css.WriteTo(writer);

```
