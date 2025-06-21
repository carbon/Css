﻿namespace Carbon.Css.Tests;

public class CssAnimationTests
{
    [Fact]
    public void KeyframesExpansion1()
    {
        var ss = StyleSheet.Parse(
            """
            //= support Safari >= 5

            @keyframes domainProcessing {
             0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
             50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
             100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
            }
            """);

        Assert.Equal(
            """
            @-webkit-keyframes domainProcessing {
              0% {
                -webkit-box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6);
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6);
              }
              50% {
                -webkit-box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2);
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
              100% {
                -webkit-box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2);
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
            }
            @keyframes domainProcessing {
              0% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6);
              }
              50% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
              100% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
            }
            """, ss.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test2()
    {
        var ss = StyleSheet.Parse(
            """
            //= support Safari >= 6

            @keyframes domainProcessing2 {
             0% { border-color: rgba(248, 202, 92, 0.4); }
             20% { border-color: rgba(248, 202, 92, 0.2); }
             100% { border-color: rgba(248, 202, 92, 0.2); }
            }

            @keyframes domainProcessing {
             0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
             50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
             100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
            }
            """);

        Assert.Equal(
            """
            @-webkit-keyframes domainProcessing2 {
              0% {
                border-color: rgba(248, 202, 92, 0.4);
              }
              20% {
                border-color: rgba(248, 202, 92, 0.2);
              }
              100% {
                border-color: rgba(248, 202, 92, 0.2);
              }
            }
            @keyframes domainProcessing2 {
              0% {
                border-color: rgba(248, 202, 92, 0.4);
              }
              20% {
                border-color: rgba(248, 202, 92, 0.2);
              }
              100% {
                border-color: rgba(248, 202, 92, 0.2);
              }
            }
            @-webkit-keyframes domainProcessing {
              0% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6);
              }
              50% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
              100% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
            }
            @keyframes domainProcessing {
              0% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6);
              }
              50% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
              100% {
                box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2);
              }
            }
            """, ss.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void KeyframesTest3()
    {
        var sheet = StyleSheet.Parse(
            """
            //= support Safari 5+

            @keyframes planet {
              0%   {
                transform: translate(0, 0px) rotate(0deg);
              }
              100% {
                transform: translate(0, 0px) rotate(-360deg);
              }
            }
            """);

        for (var i = 0; i < 100; i++)
        {
            Assert.Equal(
                """
                @-webkit-keyframes planet {
                  0% {
                    -webkit-transform: translate(0, 0px) rotate(0deg);
                    transform: translate(0, 0px) rotate(0deg);
                  }
                  100% {
                    -webkit-transform: translate(0, 0px) rotate(-360deg);
                    transform: translate(0, 0px) rotate(-360deg);
                  }
                }
                @keyframes planet {
                  0% {
                    transform: translate(0, 0px) rotate(0deg);
                  }
                  100% {
                    transform: translate(0, 0px) rotate(-360deg);
                  }
                }
                """, sheet.ToString(), ignoreLineEndingDifferences: true);
        }
    }

    [Fact]
    public void KeyframesTest()
    {
        var sheet = StyleSheet.Parse(
            """
            @keyframes flicker {
              0%    { opacity: 1; }
              30%   { opacity: .8; }
              60%   { opacity: 1; }
              100%  { opacity: .6; }
            }
            """);

        var rule = (KeyframesRule)sheet.Children[0];

        Assert.Equal(RuleType.Keyframes, rule.Type);
        Assert.Equal("flicker", rule.Name);

        Assert.Equal(4, rule.Children.Count);

        Assert.Equal("0%",   ((StyleRule)rule.Children[0]).Selector.ToString());
        Assert.Equal("30%",  ((StyleRule)rule.Children[1]).Selector.ToString());
        Assert.Equal("60%",  ((StyleRule)rule.Children[2]).Selector.ToString());
        Assert.Equal("100%", ((StyleRule)rule.Children[3]).Selector.ToString());

        Assert.Equal(
            """
            @keyframes flicker {
              0% {
                opacity: 1;
              }
              30% {
                opacity: 0.8;
              }
              60% {
                opacity: 1;
              }
              100% {
                opacity: 0.6;
              }
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }
}