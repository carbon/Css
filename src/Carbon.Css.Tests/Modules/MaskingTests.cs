namespace Carbon.Css.Tests
{
    public class MaskingTests
    {
        [Fact]
        public void A()
        {
            var text = @"
//= support Safari >= 5

div {
    mask-image: url('mask.svg');
}

";

            Assert.Equal(@"

div {
  -webkit-mask-image: url('mask.svg');
  mask-image: url('mask.svg');
}
".Trim(), StyleSheet.Parse(text).ToString());




        }
    }
}
