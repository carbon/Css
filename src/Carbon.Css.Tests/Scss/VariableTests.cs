namespace Carbon.Css.Tests;

public class VariableTests
{
    [Fact]
    public void VariableReferencingVariable()
    {
        var css = StyleSheet.Parse("""
            $red: #f00;
            $borderColor: $red;

            div { color: rgba($borderColor, 0.5); }
            """);

        Assert.Equal("div { color: rgba(255, 0, 0, 0.5); }", css.ToString());
    }

    [Fact]
    public void ReferenceToSelfThrows()
    {
        var sheet = StyleSheet.Parse("""
            $red: #fff;
            $red: $red;	

            div { color: $red; }
            """);

        Assert.Throws<Exception>(() =>
        {
            sheet.ToString();
        });
    }

    [Fact]
    public void ParseAssignment()
    {
        var text = "$color: red;";

        var assignment = StyleSheet.Parse(text).Children[0] as CssAssignment;

        Assert.Equal("color", assignment.Name);

        Assert.Equal("red", assignment.Value.ToString());
    }

    [Fact]
    public void VariableTest1()
    {
        var sheet = StyleSheet.Parse(
            """
            $blue: #dceef7;
            $yellow: #fff5cc;

            body { 
              background-color: $blue;
              color: $yellow;
            }
            """);

        var assignments = sheet.Children.OfType<CssAssignment>();

        Assert.Equal("#dceef7", assignments.First(a => a.Name is "blue").Value.ToString());

        Assert.Equal(2, assignments.Count());

        Assert.Equal(
            """
            body {
              background-color: #dceef7;
              color: #fff5cc;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void VariableTest3()
    {
        var dic = new Dictionary<string, CssValue> {
            ["monster"] = CssValue.Parse("red")
        };

        var sheet = StyleSheet.Parse(
            """
            $blue: #dceef7;
            $yellow: #fff5cc;

            body { 
              background-color: $blue;
              color: $yellow;
              monster: $monster;
            }
            """);

        Assert.Equal(
            """
            body {
              background-color: #dceef7;
              color: #fff5cc;
              monster: red;
            }
            """, sheet.ToString(dic));
    }

    [Fact]
    public void DefinedTests()
    {
        var dic = new Dictionary<string, CssValue>
        {
            ["monster"] = CssValue.Parse("purple")
        };

        // variable-exists
        var sheet = StyleSheet.Parse(
@"@if $monster != undefined {
    body { 
      background-color: red;
    }
}");

        Assert.Equal(@"body { background-color: red; }", sheet.ToString(dic));

        sheet = StyleSheet.Parse(
            """
            @if $monster == undefined {
                body { 
                  background-color: red;
                }
            }
            """);

        Assert.Equal("", sheet.ToString(dic));

        sheet = StyleSheet.Parse("""
            @if $bananas == undefined {
                body { 
                  background-color: red;
                }
            }
            """);

        Assert.Equal("body { background-color: red; }", sheet.ToString(dic));
    }

    [Fact]
    public void VariableTest4()
    {
        var dic = new Dictionary<string, CssValue>
        {
            ["monster"] = CssValue.Parse("purple")
        };

        var sheet = StyleSheet.Parse(
            """
            $blue: #dceef7;
            $yellow: #fff5cc;
            $padding: 10px;
            $padding-right: 20px;
            body { 
              background-color: $blue;
              color: $yellow;
              monster: $monster;
              padding: $padding $padding-right $padding $padding;
            }
            """);

        Assert.Equal(
            """
            body {
              background-color: #dceef7;
              color: #fff5cc;
              monster: purple;
              padding: 10px 20px 10px 10px;
            }
            """, sheet.ToString(dic));
    }

    [Fact]
    public void VariableTest2()
    {
        var css = StyleSheet.Parse(
            """
            $addYellow: #fff5cc;
            $editBlue: #dceef7;

            body { font-size: 14px; opacity: 0.5; }
            .editBlock button.save { background: $addYellow; }
            .editBlock.populated button.save { background: $editBlue; }
            .rotatedBox { box-sizing: border-box; }
            """);

        Assert.Equal(
            """
            body {
              font-size: 14px;
              opacity: 0.5;
            }
            .editBlock button.save { background: #fff5cc; }
            .editBlock.populated button.save { background: #dceef7; }
            .rotatedBox { box-sizing: border-box; }
            """, css.ToString());
    }
}
