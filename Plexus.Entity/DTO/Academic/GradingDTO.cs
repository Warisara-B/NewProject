using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Entity.DTO.Academic
{

    public class CreateGradingDTO
    {
        public string studentCode { get; set; }
        public string studentName { get; set; }
        public string coursesCode { get; set; }
        public string section { get; set; }
        public string mitdtermExam { get; set; }
        public string mitdtermReport { get; set; }
        public string finalExam { get; set; }
        public string final { get; set; }
        public string mitdterm { get; set; }
        public string totalScore { get; set; }
        public string grade { get; set; }
        public string color { get; set; }
        public GradingThresholds gradingThresholds { get; set; }
    }
    public class GradingThresholds
    {

        [DefaultValue(80.00)]
        public decimal A { get; set; } = 80;
        [DefaultValue(75.00)]
        public decimal BPlus { get; set; } = 75;
        [DefaultValue(70.00)]
        public decimal B { get; set; } = 70;
        [DefaultValue(65.00)]
        public decimal CPlus { get; set; } = 65;
        [DefaultValue(60.00)]
        public decimal C { get; set; } = 60;
        [DefaultValue(55.00)]
        public decimal DPlus { get; set; } = 55;
        [DefaultValue(50.00)]
        public decimal D { get; set; } = 50;

        public void AdjustThresholds(string adjustment)
        {
            decimal adjustmentValue;
            if (adjustment == "+1.0")
                adjustmentValue = 1.0m;
            else if (adjustment == "+0.5")
                adjustmentValue = 0.5m;
            else if (adjustment == "-1.0")
                adjustmentValue = -1.0m;
            else if (adjustment == "-0.5")
                adjustmentValue = -0.5m;
            else
                throw new ArgumentException("Invalid adjustment value. Allowed values are: +0.1, +0.5, -0.1, -0.5");

            A += adjustmentValue;
            BPlus += adjustmentValue;
            B += adjustmentValue;
            CPlus += adjustmentValue;
            C += adjustmentValue;
            DPlus += adjustmentValue;
            D += adjustmentValue;
        }

        public void AdjustThreshold(string grade, string adjustmentValue)
        {
            decimal adjustment;
            if (adjustmentValue == "+0.1")
                adjustment = 0.1m;
            else if (adjustmentValue == "+0.5")
                adjustment = 0.5m;
            else if (adjustmentValue == "-0.1")
                adjustment = -0.1m;
            else if (adjustmentValue == "-0.5")
                adjustment = -0.5m;
            else
                throw new ArgumentException("Invalid adjustment value. Allowed values are: +0.1, +0.5, -0.1, -0.5");

            switch (grade.ToUpper())
            {
                case "A":
                    A += adjustment;
                    break;
                case "BPLUS":
                    BPlus += adjustment;
                    break;
                case "B":
                    B += adjustment;
                    break;
                case "CPLUS":
                    CPlus += adjustment;
                    break;
                case "C":
                    C += adjustment;
                    break;
                case "DPLUS":
                    DPlus += adjustment;
                    break;
                case "D":
                    D += adjustment;
                    break;
                default:
                    throw new ArgumentException("Invalid grade value. Allowed values are: A, BPlus, B, CPlus, C, DPlus, D");
            }
        }
    }
    public class GradingDTO : CreateGradingDTO
    {
        public Guid id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
