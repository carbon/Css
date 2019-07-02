
using Xunit;

namespace Carbon.Css.Tests
{
    public class CssVisibilityTests
    {
        [Fact]
        public void ValuesDontChange()
        {
            Assert.Equal(1, (byte)CssVisibility.Visible);
            Assert.Equal(2, (byte)CssVisibility.Hidden);
            Assert.Equal(3, (byte)CssVisibility.Collapse);
        }
    }
}