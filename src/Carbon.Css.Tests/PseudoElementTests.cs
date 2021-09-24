namespace Carbon.Css.Tests;

public class PseudoElementTests
{
    [Fact]
    public void A()
    {
        var ss = StyleSheet.Parse(@"
//= support Safari >= 5
.block ::-webkit-input-placeholder { color: #cfcece ; font-weight: 400; }
.block      :-ms-input-placeholder { color: #cfcece ; font-weight: 400; }
.block          ::-moz-placeholder { color: #cfcece ; font-weight: 400; }
.block           :-moz-placeholder { color: #cfcece ; font-weight: 400; }
");


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
}", ss.ToString());
    }
}
