using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistorUWP
{
    class ResistorOptimizer
    {
        private readonly double[] E96 = { 10.0, 10.2, 10.5, 10.7, 11.0, 11.3,
            11.5, 11.8, 12.1, 12.4, 12.7, 13.0, 13.3, 13.7, 14.0, 14.3, 14.7,
            15.0, 15.4, 15.8, 16.2, 16.5, 16.9, 17.4, 17.8, 18.2, 18.7, 19.1,
            19.6, 20.0, 20.5, 21.0, 21.5, 22.1, 22.6, 23.2, 23.7, 24.3, 24.9,
            25.5, 26.1, 26.7, 27.4, 28.0, 28.7, 29.4, 30.1, 30.9, 31.6, 32.4,
            33.2, 34.0, 34.8, 35.7, 36.5, 37.4, 38.3, 39.2, 40.2, 41.2, 42.2,
            43.2, 44.2, 45.3, 46.4, 47.5, 48.7, 49.9, 51.1, 52.3, 53.6, 54.9,
            56.2, 57.6, 59.0, 60.4, 61.9, 63.4, 64.9, 66.5, 68.1, 69.8, 71.5,
            73.2, 75.0, 76.8, 78.7, 80.6, 82.5, 84.5, 86.6, 88.7, 90.9, 93.1,
            95.3, 97.6 };

        private readonly double[] E48 = { 10.0, 10.5, 11.0, 11.5, 12.1,
            12.7, 13.3, 14.0, 14.7, 15.4, 16.2, 16.9, 17.8, 18.7, 19.6, 20.5,
            21.5, 22.6, 23.7, 24.9, 26.1, 27.4, 28.7, 30.1, 31.6, 33.2, 34.8,
            36.5, 38.3, 40.2, 42.2, 44.2, 46.4, 48.7, 51.1, 53.6, 56.2, 59.0,
            61.9, 64.9, 68.1, 71.5, 75.0, 78.7, 82.5, 86.6, 90.9, 95.3 };

        private readonly double[] E24 = { 10, 11, 12, 13, 15, 16, 18,
            20, 22, 24, 27, 30, 33, 36, 39, 43, 47, 51, 56, 62, 68, 75, 82,
            91 };

        private readonly double[] E12 = {10, 12,15, 18, 22, 27, 33, 39,
            47, 56, 68, 82};

        private readonly double[] Multipliers = { 1.0, 10.0, 100.0, 1000.0,
            10000.0, 100000.0, 1000000.0 };

        public ResistorDividerValues values { get; set; }

        public ResistorDividerValues bruteForce(int percent, double fraction)
        {
            double[] resistors;
            double bestR1 = 0.0;
            double bestR2 = 0.0;
            double bestRatio = 0.0;
            double currentRatio = 0.0;
            double currentR1 = 0.0;
            double currentR2 = 0.0;
            double bestError = 9999999.9;
            double error = 0.0;
            double actualPercent = 0;
            this.values = new ResistorDividerValues();

            switch (percent)
            {
                case 1:
                    resistors = E96;
                    actualPercent = 0.01;
                    break;
                case 2:
                    resistors = E48;
                    actualPercent = 0.02;
                    break;
                case 5:
                    resistors = E24;
                    actualPercent = 0.05;
                    break;
                default:
                    resistors = E12;
                    actualPercent = 0.1;
                    break;
            }

            for (int i = 0; i < Multipliers.Length; i++)
            {
                for (int j = 0; j < resistors.Length; j++)
                {
                    for (int k = 0; k < Multipliers.Length; k++)
                    {
                        for (int l = 0; l < resistors.Length; l++)
                        {
                            currentR1 = resistors[j] * Multipliers[i];
                            currentR2 = resistors[l] * Multipliers[k];
                            currentRatio = divider(currentR1, currentR2);
                            error = Math.Abs(fraction - currentRatio);

                            if (error < bestError)
                            {
                                bestError = error;
                                bestR1 = currentR1;
                                bestR2 = currentR2;
                                bestRatio = currentRatio;
                                if (error == 0)
                                {
                                    this.values.R1 = bestR1;
                                    this.values.R2 = bestR2;
                                    this.values.ratio = bestRatio;
                                    this.values.error = bestError;
                                    this.values.worstCaseError1 = lowError(actualPercent, bestR1, bestR2);
                                    this.values.worstCaseError2 = highError(actualPercent, bestR1, bestR2);
                                    return this.values;
                                }
                            }
                        }
                    }

                }
            }

            this.values.R1 = bestR1;
            this.values.R2 = bestR2;
            this.values.ratio = bestRatio;
            this.values.error = bestError;
            this.values.worstCaseError1 = lowError(actualPercent, bestR1, bestR2);
            this.values.worstCaseError2 = highError(actualPercent, bestR1, bestR2);
            return this.values;
        }

        private double highError(double percent, double r1, double r2)
        {
            double rd = ((1 + percent) * r1) / (r1 * (1 + percent) + r2 * (1 - percent));
            return rd;
        }

        private double lowError(double percent, double r1, double r2)
        {
            double rd = ((1 - percent) * r1) / (r1 * (1 - percent) + r2 * (1 + percent));
            return rd;
        }

        private double divider(double r1, double r2)
        {
            return (r1 / (r1 + r2));
        }

    }

    public class ResistorDividerValues
    {
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double ratio { get; set; }
        public double error { get; set; }
        public double worstCaseError1 { get; set; }
        public double worstCaseError2 { get; set; }
    }
}

