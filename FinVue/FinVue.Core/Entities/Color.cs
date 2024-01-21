using System.ComponentModel.DataAnnotations.Schema;

namespace FinVue.Core.Entities; 

public struct Color {
    private int _rgbaColor; // rgba
    
    [NotMapped]
    public byte Red => (byte)(_rgbaColor >> 3 * 8);
    [NotMapped]
    public byte Green => (byte)(_rgbaColor >> 2 * 8);
    [NotMapped]
    public byte Blue => (byte)(_rgbaColor >> 8);
    [NotMapped]
    public byte Alpha => (byte)(_rgbaColor);
    [NotMapped]
    public string Hex => _rgbaColor.ToString("X2");

    public Color(byte red, byte green, byte blue, byte alpha) {
        _rgbaColor = red << 3 * 8 | green << 2 * 8 | blue << 8 | alpha;
    }
}