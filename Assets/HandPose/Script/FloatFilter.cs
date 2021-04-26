using Unity.Mathematics;

namespace MediaPipe.HandPose {

//
// "One euro" low pass filter for Unity.Mathematics classes
//

readonly struct FloatFilter
{
    #region Public members

    public float Beta { get; }
    public float MinCutoff { get; }
    public float Value => _prev.x;

    public FloatFilter(float beta, float minCutoff, float x = 0)
      => (Beta, MinCutoff, _prev) = (beta, minCutoff, (x, 0));

    public FloatFilter(FloatFilter filter, float x)
      => (Beta, MinCutoff, _prev) = (filter.Beta, filter.MinCutoff, (x, 0));

    public FloatFilter Next(float x, float t_e)
    {
        if (t_e < 1e-6f) return this; // safeguard

        var dx = (x - _prev.x) / t_e;
        var dx_res = math.lerp(_prev.dx, dx, Alpha(t_e, 1));

        var cutoff = MinCutoff + Beta * math.length(dx_res);
        var x_res = math.lerp(_prev.x, x, Alpha(t_e, cutoff));

        return new FloatFilter(Beta, MinCutoff, x_res, dx_res);
    }

    #endregion

    #region Private members

    readonly (float x, float dx) _prev;

    FloatFilter(float beta, float min_cutoff, float x, float dx)
      => (Beta, MinCutoff, _prev) = (beta, min_cutoff, (x, dx));

    static float Alpha(float t_e, float cutoff)
    {
        var r = 2 * math.PI * cutoff * t_e;
        return r / (r + 1);
    }

    #endregion
}

readonly struct Float4Filter
{
    #region Public members

    public float Beta { get; }
    public float MinCutoff { get; }
    public float4 Value => _prev.x;

    public Float4Filter(float beta, float minCutoff)
      => (Beta, MinCutoff, _prev) = (beta, minCutoff, (0, 0));

    public Float4Filter(float beta, float minCutoff, float4 x)
      => (Beta, MinCutoff, _prev) = (beta, minCutoff, (x, 0));

    public Float4Filter(Float4Filter filter, float4 x)
      => (Beta, MinCutoff, _prev) = (filter.Beta, filter.MinCutoff, (x, 0));

    public Float4Filter Next(float4 x, float t_e)
    {
        if (t_e < 1e-6f) return this; // safeguard

        var dx = (x - _prev.x) / t_e;
        var dx_res = math.lerp(_prev.dx, dx, Alpha(t_e, 1));

        var cutoff = MinCutoff + Beta * math.length(dx_res);
        var x_res = math.lerp(_prev.x, x, Alpha(t_e, cutoff));

        return new Float4Filter(Beta, MinCutoff, x_res, dx_res);
    }

    #endregion

    #region Private members

    readonly (float4 x, float4 dx) _prev;

    Float4Filter(float beta, float min_cutoff, float4 x, float4 dx)
      => (Beta, MinCutoff, _prev) = (beta, min_cutoff, (x, dx));

    static float Alpha(float t_e, float cutoff)
    {
        var r = 2 * math.PI * cutoff * t_e;
        return r / (r + 1);
    }

    #endregion
}

} // namespace MediaPipe.HandPose
