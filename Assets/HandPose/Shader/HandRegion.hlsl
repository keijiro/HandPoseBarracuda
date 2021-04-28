#ifndef _HANDPOSEBARRACUDA_HANDREGION_HLSL_
#define _HANDPOSEBARRACUDA_HANDREGION_HLSL_

//
// Hand region tracking structure
//
// size = 24 * 4 byte
//
struct HandRegion
{
    float4 box; // center_x, center_y, size, angle
    float4 dBox;
    float4x4 cropMatrix;
};

#endif
