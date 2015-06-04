using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriemAGInspector
{
    public partial class AbiturientCard
    {
        public bool IsCreatedByComisstion { get; set; }
        public int ClassId
        {
            get
            {
                return (int)ddlClass.Id();
            }
            set
            {
                ddlClass.SelectedValue = value;
            }
        }
        public int ProgramId
        {
            get
            {
                return (int)ddlProgram.Id();
            }
            set
            {
                ddlProgram.SelectedValue = value;
            }
        }
        public int ObrazProgramId
        {
            get
            {
                return (int)ddlObrazProgram.Id();
            }
            set
            {
                ddlObrazProgram.SelectedValue = value;
            }
        }
        public int ProfileId
        {
            get
            {
                return (int)ddlProfile.Id();
            }
            set
            {
                ddlProfile.SelectedValue = value;
            }
        }
        public bool HasManualExam { get; set; }
        public int? ManualExamId
        {
            get
            {
                return (int?)ddlManualExam.Id();
            }
            set
            {
                ddlManualExam.SelectedValue = value;
            }
        }
        public bool HostelEduc
        {
            get
            {
                return chbHostelEduc.Checked;
            }
            set
            {
                chbHostelEduc.Checked = value;
            }
        }
        public int PrioritySpecializationId
        {
            get
            {
                return (int)ddlPrioritySpecialization.Id();
            }
            set
            {
                ddlPrioritySpecialization.SelectedValue = value;
            }
        }
    }
}
