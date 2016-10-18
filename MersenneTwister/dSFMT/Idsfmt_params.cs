using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister.dSFMT
{
    public interface Idsfmt_params
    {
        int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }

    public struct dsfmt_params_521 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 521; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 3; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 25; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fbfefff77efff; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffeebfbdfbfdf; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fbfefU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xff77efffU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffeebU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfbdfbfdfU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xcfb393d661638469; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xc166867883ae2adb; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xccaa588000000000; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-521:3-25:fbfefff77efff-ffeebfbdfbfdf"; } }
    }

    public struct dsfmt_params_1279 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 1279; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 9; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000efff7ffddffee; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fbffffff77fff; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000efff7U; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xffddffeeU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fbfffU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfff77fffU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xb66627623d1a31be; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x04b6c51147b6109b; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x7049f2da382a6aeb; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xde4ca84a40000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-1279:9-19:efff7ffddffee-fbffffff77fff"; } }
    }

    public struct dsfmt_params_2203 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 2203; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 7; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fdffff5edbfff; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000f77fffffffbfe; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fdfffU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xf5edbfffU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000f77ffU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfffffbfeU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xb14e907a39338485; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xf98f0735c637ef90; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x8000000000000000; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-2203:7-19:fdffff5edbfff-f77fffffffbfe"; } }
    }

    public struct dsfmt_params_4253 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 4253; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0007b7fffef5feff; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffdffeffefbfc; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0007b7ffU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfef5feffU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffdffU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xeffefbfcU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x80901b5fd7a11c65; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x5a63ff0e7cb0ba74; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x1ad277be12000000; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-4253:19-19:7b7fffef5feff-ffdffeffefbfc"; } }
    }

    public struct dsfmt_params_11213 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 11213; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 37; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffffffdf7fffd; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000dfffffff6bfff; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fffffU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfdf7fffdU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000dffffU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfff6bfffU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xd0ef7b7c75b06793; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x9c50ff4caae0a641; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x8234c51207c80000; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-11213:37-19:ffffffdf7fffd-dfffffff6bfff"; } }
    }

    public struct dsfmt_params_19937 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19937; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 117; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffafffffffb3f; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffdfffc90fffd; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffaffU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfffffb3fU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffdffU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfc90fffdU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x90014964b32f4329; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x3b8d12ac548a7c7a; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x3d84e1ac0dc82880; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-19937:117-19:ffafffffffb3f-ffdfffc90fffd"; } }
    }

    public struct dsfmt_params_44497 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 44497; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 304; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 19; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ff6dfffffffef; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0007ffdddeefff6f; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ff6dfU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xffffffefU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0007ffddU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xdeefff6fU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x75d910f235f6e10e; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x7b32158aedc8e969; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x4c3356b2a0000000; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-44497:304-19:ff6dfffffffef-7ffdddeefff6f"; } }
    }

    public struct dsfmt_params_86243 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 86243; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 231; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 13; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffedff6ffffdf; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffff7fdffff7e; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffedfU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xf6ffffdfU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000ffff7U; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfdffff7eU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x1d553e776b975e68; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x648faadf1416bf91; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x5f2cd03e2758a373; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xc0b7eb8410000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-86243:231-13:ffedff6ffffdf-ffff7fdffff7e"; } }
    }

    public struct dsfmt_params_132049 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 132049; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 371; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 23; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fb9f4eff4bf77; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fffffbfefff37; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fb9f4U; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xeff4bf77U; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000fffffU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xbfefff37U; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x4ce24c0e4e234f3b; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x62612409b5665c2d; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x181232889145d000; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-132049:371-23:fb9f4eff4bf77-fffffbfefff37"; } }
    }

    public struct dsfmt_params_216091 : Idsfmt_params
    {
        public int DSFMT_MEXP { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 216091; } }
        public int DSFMT_POS1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 1890; } }
        public int DSFMT_SL1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 23; } }
        public ulong DSFMT_MSK1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000bf7df7fefcfff; } }
        public ulong DSFMT_MSK2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000e7ffffef737ff; } }
        public uint DSFMT_MSK32_1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000bf7dfU; } }
        public uint DSFMT_MSK32_2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x7fefcfffU; } }
        public uint DSFMT_MSK32_3 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x000e7fffU; } }
        public uint DSFMT_MSK32_4 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xfef737ffU; } }
        public ulong DSFMT_FIX1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0xd7f95a04764c27d7; } }
        public ulong DSFMT_FIX2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x6a483861810bebc2; } }
        public ulong DSFMT_PCV1 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x3af0a8f3d5600000; } }
        public ulong DSFMT_PCV2 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return 0x0000000000000001; } }
        public string DSFMT_IDSTR { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return "dSFMT2-216091:1890-23:bf7df7fefcfff-e7ffffef737ff"; } }
    }
}
