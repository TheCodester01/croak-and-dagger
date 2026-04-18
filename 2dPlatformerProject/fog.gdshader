shader_type canvas_item;

// Noise texture
uniform sampler2D noise_texture: repeat_enable, filter_nearest;

// Fog density
uniform float density: hint_range(0.0, 1.0) = 0.9;

// Fog speed
uniform vec2 speed = vec2(0.02, 0.01);

uniform float offset_ratio = 1.0;
uniform float curve_depth = 0.05;
uniform float curve_x_ratio = 1.0;
uniform float curve_x_translation = 0.5;

void vertex() {
	VERTEX.y += 1944.0 * offset_ratio;
}

void fragment() {
	// Make fog move
	vec2 uv = UV + speed * TIME;
	// Sample the noise texture
	float noise = texture(noise_texture, uv).r;
	// Convert the noise from the (0.0, 1.0) range to the (-1.0, 1.0) range
	// and clamp it between 0.0 and 1.0 again
	float fog = noise;//clamp(noise * 2.0 - 1.0, 0.0, 1.0);
	// Apply the fog effect
	if (UV.y > curve_depth * -1.0 * (cos(mix(0.0, PI, mod(UV.x + curve_x_translation, curve_x_ratio) * 1.0 / curve_x_ratio) * 2.0) - 1.0) * 0.5) {
		COLOR.a = fog * density;
	}
	else {
		COLOR.a = 0.0;
	}
}