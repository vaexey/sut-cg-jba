shader_type canvas_item;

uniform vec4 tint_default : source_color = vec4(1.0);
uniform vec4 tint_enabled : source_color = vec4(1.0);
uniform bool tint_enable = false; 

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest_mipmap;

void fragment() {
	vec4 tint = tint_enable ? tint_enabled : tint_default;
	
	COLOR = texture(screen_texture, SCREEN_UV) * tint;
}
