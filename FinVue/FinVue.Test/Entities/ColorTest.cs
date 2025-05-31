using FinVue.Core.Entities;

namespace FinVue.Test.Entities {
    public class ColorTest {
        [Fact]
        public void ColorByHex() {
            var hex = "#ff0000";
            Color color = new Color(hex);

            Assert.Equal(hex.ToUpper().Substring(1), color.Hex);
        }

        [Fact]
        public void ColorByEmptyHex() {
            var hex = "";
            Color color = new Color(hex);

            Assert.NotNull(color.Hex);
        }

        [Fact]
        public void ColorByLeadingZeroHex() {
            var hex = "#00FF00";
            Color color = new Color(hex);

            Assert.Equal(hex.ToUpper().Substring(1), color.Hex);
        }
    }
}
