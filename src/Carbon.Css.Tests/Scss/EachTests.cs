namespace Carbon.Css.Tests;

public class EachTests
{
    [Fact]
    public void A_to()
    {
        var sheet = StyleSheet.Parse(
            """
            $sizes: 40px, 50px, 80px;

            @each $size in $sizes { 
              .icon-#{$size} { font-size: $size; }
            }
            """);

        Assert.Equal("""
            .icon-40px { font-size: 40px; }
            .icon-50px { font-size: 50px; }
            .icon-80px { font-size: 80px; }
            """, sheet.ToString());
    }

    
    [Fact]
    public void EachMap()
    {
        var sheet = StyleSheet.Parse(
            """
            $font-weights: ("regular": 400, "medium": 500, "bold": 700);
            
            @each $name, $value in $font-weights { 
              .font-#{$name} { font-weight: $value; }
            }
            """);

        var assignment = (CssAssignment)sheet.Children[0];
        var map = (CssMap)assignment.Value;

        Assert.Equal(3, map.Count);
        Assert.Equal("400", map["regular"].ToString());

        Assert.Equal(
            """
            .font-regular { font-weight: 400; }
            .font-medium { font-weight: 500; }
            .font-bold { font-weight: 700; }
            """, sheet.ToString());
    }
    
}
