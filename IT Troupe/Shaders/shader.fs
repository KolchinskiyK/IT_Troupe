#version 330

in vec2 fragTexCoord;

out vec4 finalColor;

uniform sampler2D texture0;    // Textur
uniform float time;            // Time
uniform vec4 shaderColor;      // Color
uniform vec2 lightPos;         // Light Position = Player
uniform vec4 lightColor;       // light color
//uniform float lightRadius;     // radius of light

// Rippling
vec2 getWavyCoords(vec2 coords, float time) {
    float wave = sin(coords.y * 10.0 + time * 5.0) * 0.0008; // light horizontal ripple
    coords.x += wave;
    return coords;
}

void main()
{
    // Ripple to Texture
    vec2 wavyCoords = getWavyCoords(fragTexCoord, time);

    // Color from texture
    vec4 texColor = texture(texture0, wavyCoords);

    // Brightness low
    //float brightness = 0.7; // 70% brightness
    //texColor.rgb = brightness;

    // Dynamic Coord
    // vec2 fragPos = fragTexCoord vec2(1280.0, 720.0); // Screen Size

    // Distance
    // float dist = distance(fragPos, lightPos);
    // float dist = Vector2.Distance(fragTexCoord, lightPos);    BADD
    float dist = distance(fragTexCoord * vec2(textureSize(texture0, 0)), lightPos);


    // Light maths
    float intensity = clamp(1.0 - dist / 250, 0.0, 1.0);

    // Light effect
    vec4 lightEffect = lightColor * intensity * 2;

    // Dynamic Brightness
    float dynamicBrightness = 0.4 + 0.1 * sin(time * 3.0);
    texColor.rgb *= dynamicBrightness;

    // Add Color
    //finalColor = texColor shaderColor * clamp(1 + lightEffect, 1.0, 1.5);
    vec3 lightContribution = texColor.rgb * shaderColor.rgb * (lightEffect.rgb);
    finalColor = vec4(lightContribution, texColor.a); // +alpha canal
}