using Xunit;

namespace Carbon.Css.Tests
{
    public class ScssTests
    {
        [Fact]
        public void ChildSelector()
        {
            var ss = StyleSheet.Parse(@"div {
  input,
  textarea {
    display: block;
    font-size: 22px;
    line-height: 40px;
    color: #333;
    width: 100%;
    height: 60px;
    padding: 10px 15px;
    margin: 0;
    border: none;
    box-shadow: none;
    border-radius: 0px;
    border-radius: 2px 2px 0 0;
    -webkit-font-smoothing: antialiased;
    box-sizing: border-box;
  }
}");

            Assert.Single(ss.Children);

            var node = (StyleRule)((StyleRule)ss.Children[0]).Children[0];
            var parent = (StyleRule)node.Parent;

            Assert.Equal("div", parent.Selector.ToString());
            Assert.Equal("input, textarea", node.Selector.ToString());
            var selector = CssWriter.ExpandSelector(node);

            Assert.Equal("div input, div textarea", selector.ToString());

            Assert.Equal(@"div input,
div textarea {
  display: block;
  font-size: 22px;
  line-height: 40px;
  color: #333;
  width: 100%;
  height: 60px;
  padding: 10px 15px;
  margin: 0;
  border: none;
  box-shadow: none;
  border-radius: 0px;
  border-radius: 2px 2px 0 0;
  -webkit-font-smoothing: antialiased;
  box-sizing: border-box;
}", ss.ToString());
        }

      

        [Fact]
        public void DoubleList()
        {
            var sheet = StyleSheet.Parse(@"
a { 
  transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
}");

            sheet.Context.SetCompatibility(BrowserInfo.Chrome1, BrowserInfo.Safari5);

            Assert.Equal(@"a {
  -webkit-transition: -webkit-transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
  transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
}", sheet.ToString());
        }

        [Fact]
        public void DoubleList2()
        {
            var sheet = StyleSheet.Parse(@".form {
  padding-bottom: 3em;
  margin: 15px;
  padding-top: 3em;
  text-align: left;

  .field {
    position: relative;
    margin-bottom: 2em;
  
	label {
      position: absolute;
      opacity: 1;
      visibility: visible;
      display: block;
      font-size: .8em;
      line-height: 14px;
      padding: 0 15px;
      transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out, visibility 0s linear;
      transform: translate(0, -24px);
    }

    &.empty {
      label {
        opacity: 0;
        visibility: hidden;
        transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
        transform: translate(0, -14px);
      }
    }
  }

  textarea { 
    height: 10em !important;
  }

  input,
  textarea {
    display: block;
    font-size: 22px;
    line-height: 40px;
    color: #333;
    width: 100%;
    height: 60px;
    padding: 10px 15px;
    margin: 0;
    border: none;
    box-shadow: none;
    border-radius: 0px;
    border-radius: 2px 2px 0 0;
    -webkit-font-smoothing: antialiased;
    box-sizing: border-box;
  }

  button {
    font-size: 16px;
    border: none;
    color: #fff;
    padding: 12px 50px 9px;
/*    margin-top: 1.3em;*/
    transition: background .2s ease-in-out, border .2s ease-in-out, color .2s ease-in-out;
    border-radius: 2px;
    opacity: 1;
    cursor: pointer;
    -webkit-appearance: none;
    background-size: 200%;
  }

  .message {
    visibility: hidden;
    opacity: 0;
    position: absolute;
    right: 15px;
    top: 15px;
    padding: 7px 15px;
    font-size: .8em;
    line-height: 14px;
    font-style: normal;
    color: rgba(255, 255, 255, 0.8);
    background-color: #ef6469;
    border-radius: 2px;
    opacity: 0;
    transform: translate(0, -34px);
    transition: transform 0.2s ease-in-out, opacity 0.4s ease-in-out;
  }
}");

            sheet.Context.SetCompatibility(BrowserInfo.Chrome26, BrowserInfo.Safari5);

            Assert.Equal(@".form {
  padding-bottom: 3em;
  margin: 15px;
  padding-top: 3em;
  text-align: left;
}
.form .field label {
  position: absolute;
  opacity: 1;
  visibility: visible;
  display: block;
  font-size: 0.8em;
  line-height: 14px;
  padding: 0 15px;
  -webkit-transition: -webkit-transform 0.2s ease-in-out, opacity 0.2s ease-in-out, visibility 0s linear;
  transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out, visibility 0s linear;
  -webkit-transform: translate(0, -24px);
  transform: translate(0, -24px);
}
.form .field.empty label {
  opacity: 0;
  visibility: hidden;
  -webkit-transition: -webkit-transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
  transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
  -webkit-transform: translate(0, -14px);
  transform: translate(0, -14px);
}
.form .field {
  position: relative;
  margin-bottom: 2em;
}
.form textarea { height: 10em !important; }
.form input,
.form textarea {
  display: block;
  font-size: 22px;
  line-height: 40px;
  color: #333;
  width: 100%;
  height: 60px;
  padding: 10px 15px;
  margin: 0;
  border: none;
  -webkit-box-shadow: none;
  box-shadow: none;
  border-radius: 0px;
  border-radius: 2px 2px 0 0;
  -webkit-font-smoothing: antialiased;
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
}
.form button {
  font-size: 16px;
  border: none;
  color: #fff;
  padding: 12px 50px 9px;
  -webkit-transition: background 0.2s ease-in-out, border 0.2s ease-in-out, color 0.2s ease-in-out;
  transition: background 0.2s ease-in-out, border 0.2s ease-in-out, color 0.2s ease-in-out;
  border-radius: 2px;
  opacity: 1;
  cursor: pointer;
  -webkit-appearance: none;
  background-size: 200%;
}
.form .message {
  visibility: hidden;
  opacity: 0;
  position: absolute;
  right: 15px;
  top: 15px;
  padding: 7px 15px;
  font-size: 0.8em;
  line-height: 14px;
  font-style: normal;
  color: rgba(255, 255, 255, 0.8);
  background-color: #ef6469;
  border-radius: 2px;
  opacity: 0;
  -webkit-transform: translate(0, -34px);
  transform: translate(0, -34px);
  -webkit-transition: -webkit-transform 0.2s ease-in-out, opacity 0.4s ease-in-out;
  transition: transform 0.2s ease-in-out, opacity 0.4s ease-in-out;
}", sheet.ToString());
        }




        [Fact]
        public void NestedStyleRecursiveRewriterTest()
        {
            var sheet = StyleSheet.Parse(
@"nav {
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

  i {
    b { color: red; }
  }
}");

            Assert.Equal(
@"nav ul {
  margin: 0;
  padding: 0;
  list-style: none;
}
nav li { display: inline-block; }
nav a {
  display: block;
  padding: 6px 12px;
  text-decoration: none;
}
nav i b { color: red; }", sheet.ToString());

        }
    }
}
