#ifndef _HANDPOSEBARRACUDA_LOWPASSFILTER_HLSL_
#define _HANDPOSEBARRACUDA_LOWPASSFILTER_HLSL_

//
// "One Euro" low pass filter
//

float lpf_Alpha(float cutoff, float t_e)
{
    float r = 2 * 3.141592 * cutoff * t_e;
    return r / (r + 1);
}

// float

float lpf_Step_dx(float x, float p_x, float p_dx, float3 params)
{
    float dx = (x - p_x) / params.z;
    return lerp(p_dx, dx, lpf_Alpha(1, params.z));
}

float lpf_Step_x(float x, float p_x, float dx, float3 params)
{
    float cutoff = params.y + params.x * length(dx);
    return lerp(p_x, x, lpf_Alpha(cutoff, params.z));
}

// float2

float2 lpf_Step_dx(float2 x, float2 p_x, float2 p_dx, float3 params)
{
    float2 dx = (x - p_x) / params.z;
    return lerp(p_dx, dx, lpf_Alpha(1, params.z));
}

float2 lpf_Step_x(float2 x, float2 p_x, float2 dx, float3 params)
{
    float cutoff = params.y + params.x * length(dx);
    return lerp(p_x, x, lpf_Alpha(cutoff, params.z));
}

// float3

float3 lpf_Step_dx(float3 x, float3 p_x, float3 p_dx, float3 params)
{
    float3 dx = (x - p_x) / params.z;
    return lerp(p_dx, dx, lpf_Alpha(1, params.z));
}

float3 lpf_Step_x(float3 x, float3 p_x, float3 dx, float3 params)
{
    float cutoff = params.y + params.x * length(dx);
    return lerp(p_x, x, lpf_Alpha(cutoff, params.z));
}

// float4

float4 lpf_Step_dx(float4 x, float4 p_x, float4 p_dx, float3 params)
{
    float4 dx = (x - p_x) / params.z;
    return lerp(p_dx, dx, lpf_Alpha(1, params.z));
}

float4 lpf_Step_x(float4 x, float4 p_x, float4 dx, float3 params)
{
    float cutoff = params.y + params.x * length(dx);
    return lerp(p_x, x, lpf_Alpha(cutoff, params.z));
}

#endif
