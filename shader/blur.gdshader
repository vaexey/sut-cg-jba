shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest_mipmap;
// uniform float lod: hint_range(0.0, 1) = 0.0;

uniform int radius: hint_range(1,10) = 1;
uniform float minAlpha: hint_range(0.0, 1.0) = 0.1;
uniform vec4 tint : source_color = vec4(1.0);

void fragment(){

    //COLOR = texture(screen_texture, SCREEN_UV);
	//COLOR.r = 1.0;
	//COLOR.rgba = vec4(UV, 0.0, 1.0);
	//COLOR.rgba = vec4(1.0, 0.0, 1.0, 1.0);
	//COLOR.rgba = vec4(SCREEN_UV, 0.0, 1.0);
	//COLOR = textureLod(TEXTURE, UV, 300.0);
	//COLOR = texture(TEXTURE, vec2(0.0,0.0));
	//COLOR = texture(TEXTURE, UV);

	//vec4 tex = texture(TEXTURE, UV);
	// //vec4 tex = vec4(1.0, 0.0, 0.0, 1.0);
	// vec2 px = SCREEN_PIXEL_SIZE;
	// //vec2 px = vec2(0.0001, 0.0001);

	// vec2 newUv = SCREEN_UV + (30.0 * px);

	// vec4 scrn = texture(screen_texture, newUv);

	//vec4 mask = vec4(lod,lod,lod,lod);
//
	//COLOR = mix(tex,scrn,mask);
	// COLOR = scrn * 0.5;


	vec2 px = SCREEN_PIXEL_SIZE;

	int begin = -radius;
	int end = radius + 1;

	vec4 color = vec4(0.0, 0.0, 0.0, 0.0);
	float alpha = 0.0;
	int pixels = 0;

	for(int x = begin; x < end; x++)
	{
		for(int y = begin; y < end; y++)
		{
			vec2 diff = vec2(float(x), float(y));
			vec2 uv = SCREEN_UV + diff * px;
			vec4 tex = texture(screen_texture, uv);

			if(tex.a >= minAlpha)
			{
				color += tex;
				pixels++;
			}

			alpha += tex.a;
		}
	}

	float weight = (1.0 / float(pixels));
	float alphaWeight = (1.0 / float(radius * 2 + 1));

	vec4 blur = color * weight;

	//blur.a = alpha / alphaWeight;

	COLOR = blur * tint;
}
