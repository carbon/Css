﻿namespace Carbon.Css.Tests;

public class MediaTests
{
    [Fact]
    public void Variable1()
    {
        var sheet = StyleSheet.Parse(
            """
            $tabletBreak: 700px;

            @media (min-width: $tabletBreak) { 
              div { 
                width: 100px;
              }
            }
            """);

        Assert.Equal(
            """
            @media (min-width: 700px) {
              div {
                width: 100px;
              }
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Variable2()
    {
        var sheet = StyleSheet.Parse(
            """
            $breakpoint1: 800px;

            @media screen and (min-width: $breakpoint1) { 
              div { 
                width: 100px;
              }
            }
            """);

        Assert.Equal(
            """
            @media screen and (min-width: 800px) {
              div {
                width: 100px;
              }
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Css4RangeQuery1()
    {
        var sheet = StyleSheet.Parse(
           """
            @media (width <= 30em) { 
              div { 
                width: 100px;
              }
            }
            """);

        Assert.Single(sheet.Children);

        var mediaRule = (MediaRule)sheet.Children[0];

        Assert.Equal("(width <= 30em)", mediaRule.Queries.ToString());
    }

    [Fact]
    public void Css4RangeQuery2()
    {
        var sheet = StyleSheet.Parse(
           """
            @media (30em <= width <= 50em) { 
                div { 
                    width: 100px;
                }
            }
            """);

        Assert.Single(sheet.Children);

        var mediaRule = (MediaRule)sheet.Children[0];

        Assert.Equal("(30em <= width <= 50em)", mediaRule.Queries.ToString());
    }

    [Fact]
    public void Basic()
    {
        var sheet = StyleSheet.Parse(
            """
            @media (min-width: 700px) { 
                div { 
                    width: 100px;
                }
            }
            """);

        Assert.Single(sheet.Children);

        var mediaRule = (MediaRule)sheet.Children[0];

        Assert.Equal("(min-width: 700px)", mediaRule.Queries.ToString());

        var rule = (StyleRule)mediaRule.Children[0];

        Assert.Equal("100px", rule.GetDeclaration("width").Value.ToString());
    }

    [Fact]
    public void Nested()
    {
        var sheet = StyleSheet.Parse(
            """
            @media (min-width: 700px) { 
              div { 
                width: 100px;
                         
                span { width: 50px; }
              }
            }
            """);

        Assert.Single(sheet.Children);

        Assert.Equal(
            """
            @media (min-width: 700px) {
              div {
                width: 100px;
              }
              div span {
                width: 50px;
              }
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact(Skip = "Not implemented yet")]
    public void Nested_2()
    {
        var sheet = StyleSheet.Parse(
            """
            div { 
              @media (min-width: 700px) { 
                span { width: 50px; }
              }
            }
            """);

        Assert.Single(sheet.Children);

        Assert.Equal(
            """
            @media (min-width: 700px) {
              div {
                width: 100px;
              }
              div span {
                width: 50px;
              }
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void MediaMixin1()
    {
        var sheet = StyleSheet.Parse(
            """
            @mixin hi { 
              color: red;
            }

            @mixin blerg { 
              a {
                color: pink;
                &:hover { color: #000; }
              }
            }

            @media (min-width: 700px) { 
              @include blerg;

              div { 
                background-color: $bgColor;
                @include hi;
              }
            }
            """);

        Assert.Equal(2, sheet.Context.Mixins.Count);


        Assert.True(sheet.Context.Mixins.ContainsKey("hi"));
        Assert.True(sheet.Context.Mixins.ContainsKey("blerg"));

        Assert.Single(sheet.Children);

        var rule = sheet.Children[0] as MediaRule;

        Assert.NotNull(rule);

        Assert.Equal("(min-width: 700px)", rule.Queries.ToString());

        var include = rule.Children[0] as IncludeNode;

        Assert.Equal("blerg", include.Name);
    }

    [Fact]
    public void WithMixin()
    {
        var sheet = StyleSheet.Parse(
            """
            $bgColor: orange;

            @mixin hi { 
              color: red;
            }

            @mixin blerg { 
              a {
                color: pink;
                &:hover { color: #000; }
              }
            }

            @media (min-width: 700px) { 
              @include blerg;

              div { 
                background-color: $bgColor;
                @include hi;
              }
            }
            """);

        Assert.Equal(
            """
            @media (min-width: 700px) {
              a {
                color: pink;
              }
              a:hover {
                color: #000;
              }
              div {
                background-color: orange;
                color: red;
              }
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }
}