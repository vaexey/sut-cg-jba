shader_type canvas_item;

uniform bool enable = false;

uniform vec4 modulate: source_color = vec4(1.0);

uniform float hue_shift: hint_range(0.0, 1.0) = 0.0;

//uniform float weight: hint_range(0.0, 1.0, 0.1) = 0.5;

uniform float hue_min: hint_range(0.0, 360.0, 0.1) = 0;
uniform float hue_max: hint_range(0.0, 360.0, 0.1) = 0;
uniform float hue_overlap: hint_range(0.1, 360.0, 0.1) = 0.1;

vec4 shift_hue(in vec4 color, in float shift) {
	// The unit gray vector in RGB color space.
	vec3 gray = vec3(0.57735);
	// Project color onto gray axis.
	vec3 projection = gray * dot(gray, color.rgb);
	// Vector from gray axis to original color.
	vec3 U = color.rgb - projection;
	// Vector perpendicular to gray axis and U.
	vec3 V = cross(gray, U);
	// Rotate U and V around the gray axis.
	vec3 shifted = U * cos(shift * 2.0 * PI) + V * sin(shift * 2.0 * PI) + projection;
	return vec4(shifted, color.a);
}

float hue_of(in vec4 color) {
	float minv = min(min(color.r, color.g), color.b);
    float maxv = max(max(color.r, color.g), color.b);

    if (minv == maxv) {
        return 0.0f;
    }

    float hue = 0.0f;
    if (maxv == color.r) {
        hue = (color.g - color.b) / (maxv - minv);
    } else if (maxv == color.g) {
        hue = 2.0f + (color.b - color.r) / (maxv - minv);
    } else {
        hue = 4.0f + (color.r - color.g) / (maxv - minv);
    }

    hue = hue * 60.0f;
    if (hue < 0.0f) hue = hue + 360.0f;

    return hue;
}

void fragment() {
	vec4 original = texture(TEXTURE, UV);

	vec4 shifted = shift_hue(original, hue_shift);

	float weight = 0.0f;
	float hue = hue_of(original);

	if(hue >= hue_min && hue <= hue_max)
		weight = 1.0f;
	else
	{
		float diff = min(abs(hue_min - hue), abs(hue_max - hue));

		if(diff <= hue_overlap)
		{
			weight = 1.0f - diff / hue_overlap;
		}
	}

	if(!enable) weight = 0.0f;

	COLOR = (shifted * weight + original * (1.0f - weight)) * modulate;
}
