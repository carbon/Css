﻿namespace Carbon.Css.Tests;

public class AutoPrefixTests
{
    [Fact]
    public void ZoomOut()
    {
        var sheet = StyleSheet.Parse(
            """
            //= support Safari >= 7
            div { cursor: zoom-out }
            """);

        Assert.Equal(
            """
            div {
              cursor: -webkit-zoom-out;
              cursor: zoom-out;
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void GrabSupport()
    {
        var sheet = StyleSheet.Parse(
            """
            //= support Firefox >= 5
            //= support Safari >= 1
            div { cursor: grab }
            """);

        Assert.Equal(
            """
            div {
              cursor: -moz-grab;
              cursor: -webkit-grab;
              cursor: grab;
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }
}