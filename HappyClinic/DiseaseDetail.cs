//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HappyClinic
{
    using System;
    using System.Collections.Generic;
    
    public partial class DiseaseDetail
    {
        public string ExamID { get; set; }
        public string DiseaseID { get; set; }
        public string ID { get; set; }
    
        public virtual Disease Disease { get; set; }
        public virtual ExaminationForm ExaminationForm { get; set; }
    }
}
