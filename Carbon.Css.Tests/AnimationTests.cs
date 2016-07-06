namespace Carbon.Css
{
    using Carbon.Css.Tests;

    using Xunit;

    public class AnimationTests : FixtureBase
    {
        [Fact]
        public void KetframesExpansition1()
        {
            var ss = StyleSheet.Parse(@"
//= support Safari >= 5
.block ::-webkit-input-placeholder { color: #cfcece ; font-weight: 400; }
.block      :-ms-input-placeholder { color: #cfcece ; font-weight: 400; }
.block          ::-moz-placeholder { color: #cfcece ; font-weight: 400; }
.block           :-moz-placeholder { color: #cfcece ; font-weight: 400; }


@keyframes domainProcessing {
 0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
 50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
 100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
}");


            Assert.Equal(
@".block ::-webkit-input-placeholder {
  color: #cfcece;
  font-weight: 400;
}
.block :-ms-input-placeholder {
  color: #cfcece;
  font-weight: 400;
}
.block ::-moz-placeholder {
  color: #cfcece;
  font-weight: 400;
}
.block :-moz-placeholder {
  color: #cfcece;
  font-weight: 400;
}
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
  0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
  50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
  100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
}", ss.ToString());



        }

        [Fact]
        public void Test50()
        {
            var ss = StyleSheet.Parse(
@"//= support Safari >= 6
@keyframes domainProcessing2 {
 0% { border-color: rgba(248, 202, 92, 0.4); }
 20% { border-color: rgba(248, 202, 92, 0.2); }
 100% { border-color: rgba(248, 202, 92, 0.2); }
}

@keyframes domainProcessing {
 0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
 50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
 100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
}");




            Assert.Equal(
@"@-webkit-keyframes domainProcessing2 {
  0% { border-color: rgba(248, 202, 92, 0.4); }
  20% { border-color: rgba(248, 202, 92, 0.2); }
  100% { border-color: rgba(248, 202, 92, 0.2); }
}
@keyframes domainProcessing2 {
  0% { border-color: rgba(248, 202, 92, 0.4); }
  20% { border-color: rgba(248, 202, 92, 0.2); }
  100% { border-color: rgba(248, 202, 92, 0.2); }
}
@-webkit-keyframes domainProcessing {
  0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
  50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
  100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
}
@keyframes domainProcessing {
  0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
  50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
  100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
}", ss.ToString());
        }

        [Fact]
        public void KeyframesTest3()
        {
            var sheet = StyleSheet.Parse(@"
//= support Firefox 3+
//= support Safari 5+
@keyframes planet {
  0%   { transform: translate(0, 0px)rotate(0deg); }
  100% { transform: translate(0, 0px)rotate(-360deg); }
}");

            for (var i = 0; i < 100; i++)
            {

                Assert.Equal(@"@-moz-keyframes planet {
  0% {
    -moz-transform: translate(0, 0px) rotate(0deg);
    transform: translate(0, 0px) rotate(0deg);
  }
  100% {
    -moz-transform: translate(0, 0px) rotate(-360deg);
    transform: translate(0, 0px) rotate(-360deg);
  }
}
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
  0% { transform: translate(0, 0px) rotate(0deg); }
  100% { transform: translate(0, 0px) rotate(-360deg); }
}", sheet.ToString());
            }
        }

        [Fact]
        public void KeyframesTest2()
        {
            var sheet = StyleSheet.Parse(@"
//= support Safari >= 5
@keyframes flicker {
  0%    { opacity: 1; }
  30%   { opacity: .8; }
  60%   { opacity: 1; }
  100%  { opacity: .6; }
}");


            Assert.Equal(
@"@-webkit-keyframes flicker {
  0% { opacity: 1; }
  30% { opacity: .8; }
  60% { opacity: 1; }
  100% { opacity: .6; }
}
@keyframes flicker {
  0% { opacity: 1; }
  30% { opacity: .8; }
  60% { opacity: 1; }
  100% { opacity: .6; }
}", sheet.ToString());
        }

        [Fact]
        public void KeyframesTest()
        {
            var sheet = StyleSheet.Parse(@"@keyframes flicker {
  0%    { opacity: 1; }
  30%   { opacity: .8; }
  60%   { opacity: 1; }
  100%  { opacity: .6; }
}");

            var rule = sheet.Children[0] as KeyframesRule;

            Assert.Equal(RuleType.Keyframes, rule.Type);
            Assert.Equal("flicker", rule.Name);

            Assert.Equal(4, rule.Children.Count);

            Assert.Equal("0%", ((StyleRule)rule.Children[0]).Selector.ToString());
            Assert.Equal("30%", ((StyleRule)rule.Children[1]).Selector.ToString());
            Assert.Equal("60%", ((StyleRule)rule.Children[2]).Selector.ToString());
            Assert.Equal("100%", ((StyleRule)rule.Children[3]).Selector.ToString());

            Assert.Equal(@"@keyframes flicker {
  0% { opacity: 1; }
  30% { opacity: .8; }
  60% { opacity: 1; }
  100% { opacity: .6; }
}", sheet.ToString());
        }
    }
}
