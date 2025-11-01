using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniproject02_huffmancoding.Utils
{
    internal class Stats
    {
        public int OriginalSize { get; set; }
        public int CompressedSize { get; set; }
        public double CompressionRatio { get; set; } // Original / Comprimido
        public double ReductionPercentage { get; set; } // % reducción

        public override string ToString()
        {
            return
                "===== ESTADÍSTICAS DE COMPRESIÓN =====\n" +
                $"Tamaño original       : {OriginalSize} bytes\n" +
                $"Tamaño comprimido     : {CompressedSize} bytes\n" +
                $"Ratio de compresión   : {CompressionRatio}\n" +
                $"Reducción             : {ReductionPercentage}%\n" +
                "=====================================";
        }
    }
}
