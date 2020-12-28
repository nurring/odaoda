using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Thumbnail //: Mat
    {
        private Mat image;
        private CHANNEL channel;
        private ALGORITHM_TYPE algorithmType;
        private COMP_TYPE compType;
        private int compCheck;
        private string fileName;

        public Mat Image { get => image; set => image = value; }
        public CHANNEL Channel { get => channel; set => channel = value; }
        public ALGORITHM_TYPE AlgorithmType { get => algorithmType; set => algorithmType = value; }
        public COMP_TYPE CompType { get => compType; set => compType = value; }
        public int CompCheck { get => compCheck; set => compCheck = value; }
        public string FileName { get => fileName; set => fileName = value; }
    }
    public enum CHANNEL
    {
        R, W, B, G
    }
    public enum ALGORITHM_TYPE
    {
        MIN, MAX, AVG, STD,
        LL, LH, HL, HH, BLEND
    }
    public enum COMP_TYPE
    {
        SEN_FS, SEN_S, SEN_JB
    }
    public enum COMP_CHECK //쓸까..?
    {
        BEFORE, AFTER
    }
}
