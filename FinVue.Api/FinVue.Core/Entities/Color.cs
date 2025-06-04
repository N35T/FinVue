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
    public string Hex => "#" + _rgbaColor.ToString("X2");

    public Color(byte red, byte green, byte blue, byte alpha) {
        _rgbaColor = red << 3 * 8 | green << 2 * 8 | blue << 8 | alpha;
    }

    public Color() {
        var rng = new Random();
        byte r = (byte)rng.Next(256);
        byte g = (byte)rng.Next(256);
        byte b = (byte)rng.Next(256);
        _rgbaColor = r << 3 * 8 | g << 2 * 8 | b << 8 | (byte) 255;
    }

    public override string ToString() {
        return Hex;
    }

    private Color(int rgba) {
        _rgbaColor = rgba;
    }

    public int ToDto() {
        return _rgbaColor;
    }

    public static Color FromDto(int dto) {
        return new Color(dto);
    }


    public static bool operator ==(Color? a, Color? b)
        => a?._rgbaColor == b?._rgbaColor;
    
    public static bool operator !=(Color? a, Color? b)
        => !(a == b);
}