using System;
using System.Drawing;

namespace NanoHass.Support {
    // Given H,S,L in range of 0-1
    // Returns a Color (RGB struct) in range of 0-255
    public static class HslColor {
        public static Color FromHsl( float hue, float saturationPercent, float lightnessPercent, int alpha = 255 ) {
            if( hue < 0 || hue > 360 ) {
                throw new ArgumentException( $"Value of '{hue}' is not valid for '{nameof( hue )}'. '{nameof( hue )}' should be greater than or equal to 0 and less than or equal to 360.", nameof( hue ) );
            }
            if( saturationPercent < 0 || saturationPercent > 100 ) {
                throw new ArgumentException( $"Value of '{saturationPercent}' is not valid for '{nameof( saturationPercent )}'. '{nameof( saturationPercent )}' should be greater than or equal to 0 and less than or equal to 100.", nameof( saturationPercent ) );
            }
            if( lightnessPercent < 0 || lightnessPercent > 100 ) {
                throw new ArgumentException( $"Value of '{lightnessPercent}' is not valid for '{nameof( lightnessPercent )}'. '{nameof( lightnessPercent )}' should be greater than or equal to 0 and less than or equal to 100.", nameof( lightnessPercent ) );
            }

            float saturation = saturationPercent / 100f;
            float lightness = lightnessPercent / 100f;

            hue /= 60f;
            float q = (!(lightness <= 0.5)) ? 
                (lightness + saturation - (lightness * saturation)) : 
                (lightness * (saturation + 1f));
            float p = (lightness * 2f) - q;

            float HueToRgb( float p, float q, float t ) {
                if( t < 0f ) {
                    t += 6f;
                }
                if( t >= 6f ) {
                    t -= 6f;
                }

                if( t < 1f ) {
                    return ( ( q - p ) * t ) + p;
                }

                if( t < 3f ) {
                    return q;
                }

                if( t < 4f ) {
                    return ( ( q - p ) * ( 4f - t ) ) + p;
                }

                return p;
            }

            return Color.FromArgb( alpha,
                (int)Math.Round( 255f * HueToRgb( p, q, hue + 2f )),
                (int)Math.Round( 255f * HueToRgb( p, q, hue )),
                (int)Math.Round( 255f * HueToRgb( p, q, hue - 2f )));
        }
    }
}
