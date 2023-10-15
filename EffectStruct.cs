using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MGRRDat
{
    internal class EffectStruct
    {

        public struct EffectHeader
        {
            public string id; // Assuming fixed size 4
            public uint recordCount;
            public uint recordOffsetsOffset;
            public uint typeOffset;
            public uint typeEndOffset;
            public uint typeSize;
            public uint typeNumber;
        }

        public class TypeGroup
        {
            public uint u_a;
            public string id;
            public uint size;
            public uint offset;
        }

        public class TypeGroups
        {
            public TypeGroup[] types;
        }

        public class MyClass
        {
            public EffectHeader header;
            public uint[] offsets;
            public TypeGroups[] typeGroups;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Part_s
        {
            public short u_a;
            public short u_b;
            public uint u_c;
            public uint u_d;
            public short[] u_e; // Assuming fixed size 8
            public uint[] u_f; // Assuming fixed size 9
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Move_s
        {
            public uint u_a;
            public float offset_x;
            public float offset_y;
            public float offset_z;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public float[] u_b_1; // Assuming fixed size 15
            public float angle;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
            public float[] u_b_2; // Assuming fixed size 15

            public float scale;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public float[] u_c; // Assuming fixed size 16
            public float red;
            public float green;
            public float blue;
            public float alpha;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 42)]
            public float[] u_d; // Assuming fixed size 42



        }

        public struct Emif_s
        {
            public short count;
            public short u_a1;
            public short u_a2;
            public short u_a3;
            public short play_delay;
            public short repeating;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public float[] u_b;
        }

        public struct Tex_s
        {
            public float u_a;
            public short u_b;
            public short u_c;
            public float[] u_d; // Assuming fixed size 4
            public Substruct[] substruct; // Assuming fixed size 2

            public struct Substruct
            {
                public float u_d1;
                public short u_e;
                public char[] u_f; // Assuming fixed size 2
                public float u_g;
                public char[] u_h; // Assuming fixed size 4
                public float[] u_i; // Assuming fixed size 15
            }
        }

        public struct Pssa_s
        {
            public float[] u_a; // Assuming fixed size 24
        }

        public struct Fvwk_s
        {
            public float[] u_a; // Assuming fixed size 20
        }

        public struct Fwk_s
        {
            public short unk_value;
            public short texture_num_1;
            public short texture_num_2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] u_a; // Assuming fixed size 3
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] u_c; // Assuming fixed size 5
        }

        public struct Emmv_s
        {
            public uint u_a;

            public float left_pos1;
            public float top_pos;
            public float unk_pos1;
            public float random_pos1;
            public float top_bottom_random_pos1;
            public float front_back_random_pos1;
            public float left_pos2;
            public float front_pos1;
            public float front_pos2;
            public float left_right_random_pos1;
            public float random_pos2;
            public float front_back_random_pos2;
            public float unk_pos2;
            public float left_pos_random1;
            public float top_pos2;
            public float front_pos3;
            public float unk_pos3;
            public float unk_pos4;
            public float unk_pos5;
            public float unk_pos6; //19 (starts from 0)
            public float unk_pos7;
            public float unk_pos8;
            public float unk_pos9;

            public float unk_pos10;
            public float unk_pos11; //24
            public float unk_pos25;
            public float unk_pos26;
            public float unk_pos27;
            public float unk_pos28;
            public float unk_pos29;
            public float unk_pos30;
            public float unk_pos31;

            public float effect_size;
            public float[] u_b; // Assuming fixed size 107 - count of variables after u_a
        }

        public struct Emfw_s
        {
            public int u_a;
            public short[] u_b; // Assuming fixed size 2
            public int[] u_c; // Assuming fixed size 6
        }

        public struct Mjsg_s
        {
            public char[] u_a; // Assuming fixed size 4
            public float[] u_b; // Assuming fixed size 4
            public char[] u_c; // Assuming fixed size 4
            public int[] u_d; // Assuming fixed size 2
        }

        public struct Mjcm_s
        {
            public short[] u_a; // Assuming fixed size 40
        }
    }
}
