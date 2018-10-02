
using Xunit;

namespace Carbon.Css.Tests
{
    public class NestingTests
    {
        [Fact]
        public void SingleNestedReference()
        {
            var css = StyleSheet.Parse("div { &.hide { display: none; } }");

            Assert.Equal("div.hide { display: none; }", css.ToString());
        }

        [Fact]
        public void SingleDoubleNestedReference()
        {
            var css = StyleSheet.Parse(@"
#networkLinks .block {
  .edit {
    opacity: 0;
    
    &:before {
      font-family: 'carbon';
    }
  }
}".Trim());

            var node = (((css.Children[0] as StyleRule).Children[0] as StyleRule)).Children[1] as StyleRule;

            var selector = CssWriter.ExpandSelector(node);

            Assert.True(selector[0][0] is CssReference);
            Assert.True(selector[0][1] is CssString);

            // throw new System.Exception(string.Join(Environment.NewLine, selector[0].Select(a => a.Kind + " " + a.ToString())));

            Assert.Equal(@"
#networkLinks .block .edit:before { font-family: 'carbon'; }
#networkLinks .block .edit { opacity: 0; }
".Trim(), css.ToString());

        }

        [Fact]
        public void NestedSelectorList()
        {
            var css = StyleSheet.Parse(@"
div {
  input,
  textarea {
    display: block;
  } 
}".Trim());


            Assert.Equal(@"
div input,
div textarea { display: block; }
".Trim(), css.ToString());
        }

        [Fact]
        public void SelectorListWithNestedReference()
        {
            var css = StyleSheet.Parse(@"div, span { .hide { display: none; } }");

            Assert.Equal(@"

div .hide,
span .hide { display: none; }

".Trim(), css.ToString());
        }
        
        [Fact]
        public void B()
        {
            var css = StyleSheet.Parse("div { &.hide, &.hidden { display: none; } }");

            Assert.Equal(@"

div.hide,
div.hidden { display: none; }

".Trim(), css.ToString());
        }

        [Fact]
        public void C()
        {
            var css = StyleSheet.Parse(".hide { body & { display: none; } }");

            Assert.Equal("body .hide { display: none; }", css.ToString());
        }

        [Fact]
        public void NestedMultiselector1()
        {
            var css = StyleSheet.Parse(@"

.details {
  max-width: 60rem;

  .description {
    ul {
      list-style: disc;
    }
    p, ul, ol {
      font-size: 1.2em;
    
      &:last-child {
        margin-bottom: 0;
      }
    }
  }
}

");


            Assert.Equal(@"
.details { max-width: 60rem; }
.details .description ul { list-style: disc; }
.details .description p:last-child,
.details .description ul:last-child,
.details .description ol:last-child { margin-bottom: 0; }
.details .description p,
.details .description ul,
.details .description ol { font-size: 1.2em; }

".Trim(), css.ToString());

        }


        [Fact]
        public void NestedStyleRewriterTest()
        {
            var sheet = StyleSheet.Parse(@"

nav {
  display: block;
  ul {
    margin: 0;
    padding: 0;
    list-style: none;
  }

  li { display: inline-block; }

  a {
    display: block;
    padding: 6px 12px;
    text-decoration: none;
  }
}

");

            Assert.Equal(
@"nav { display: block; }
nav ul {
  margin: 0;
  padding: 0;
  list-style: none;
}
nav li { display: inline-block; }
nav a {
  display: block;
  padding: 6px 12px;
  text-decoration: none;
}", sheet.ToString());

        }
    }
}