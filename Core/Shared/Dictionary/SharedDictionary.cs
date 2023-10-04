
namespace Core.Shared.Dictionary
{
    #region List
    public static class ListRID
    {
        public const string OPERATION_TYPE = "C_OperationType";
        public const string STATUS_EXECUTION_OPERATION = "C_OperationStatusExecution";
        public const string STEP_TYPE = "C_StepType";
        public const string STATUS_EXECUTION_STEP = "C_StepStatusExecution";
        public const string FORM_TYPE = "C_FormType";
        public const string FORM_SUB_TYPE = "C_FormSubType";
		public const string FIELD_TYPE = "C_FieldType";
		public const string FIELD_UNIT = "C_FieldUnit";
		public const string EQUIPMENT_TYPE = "C_EquipmentType";
		public const string FAMILY = "C_Family";
        public const string DEPARTMENT = "C_Department";
        public const string STATUS_DESIGN = "C_StatusDesign";
        public const string STATUS_PLANIFICATION = "C_StatusPlanification";
        public const string STATUS_CONFORMITY = "C_StatusConformity";
        public const string STATUS_TIME_CONFORMITY = "C_StatusTimeConformity";
        public const string STATUS_DURATION_CONFORMITY = "C_StatusDurationConformity";
        public const string STATUS_EXECUTION_CLOSE = "C_StatusExecutionClose";
        public const string ENTITY_TYPE = "C_EntityType";
        public const string SIGNATURE_TYPE = "C_ActSignatureType";
    }

    #region Choices

    public static class EntityTypeRID
    {
	    public const string MATERIAL_C = "MATERIAL_C";
        public const string OPERATION_C = "OPERATION_C";
        public const string OPERATION_I = "OPERATION_I";
        public const string STEP_C = "STEP_C";
		public const string STEP_I = "STEP_I";
        public const string CHECKLIST_C = "CHECKLIST_C";
        public const string FIELD_C = "FIELD_C";
        public const string FIELD_I = "FIELD_I";
        public const string EQUIPMENT_C = "EQUIPMENT_C";
         public const string KPI_LOSSISSUES = "KPI_LOSSISSUES";
    }

    public static class ActivityStateRID
    {
	    public const string CREATED = "Created";
	    public const string INPROGRESS = "InProgress";
	    public const string FINISHED = "Finished";
    }

    public static class StatusDesignRID
    {
        public const string DRAFT = "Draft";
        public const string PREPRODUCTION = "Preproduction";
        public const string VALIDATION = "Validation";
        public const string RELEASED = "Released";
    }


    public static class StatusConformityRID
    {
        public const string NA = "NA"; // N/A
        public const string COMPLIANT = "Compliant"; // Conforme
        public const string NOT_COMPLIANT = "NotCompliant"; // Non conforme
        public const string CRITICAL = "Critical"; // Critique
    }

    public static class StatusDurationConformityRID
    {
        public const string COMPLIANT = "Compliant";
        public const string SHORT = "Short";
        public const string LONG = "Long";
    }

    public static class StatusTimeConformityRID
    {
        public const string COMPLIANT = "Compliant";
        public const string EARLY = "Early";
        public const string LATE = "Late";
    }

    public static class StatusExecutionCloseRID
    {
        public const string NA = "NA";
        public const string NORMAL = "Normal";
        public const string ABORTED = "Aborted";
        public const string KILLED = "Killed";
    }



	public static class FamilyRID
    {
        public const string NA = "NA";
        public const string FAMILY1 = "Family1";
        public const string FAMILY2 = "Family2";
    }

    public static class DepartmentRID
    {
        public const string NA = "NA";
        public const string DEPARTMENT1 = "Department1";
        public const string DEPARTMENT2 = "Department2";
    }

	public static class LangRID
	{
		public const string FR = "fr";
		public const string EN = "en";
		public const string PT = "pt";
		public const string ZH = "zh";
		

	}

    #endregion

    #endregion

	public enum UploadType
	{
		MEDIA,
		DOCUMENT,
		VOICE
	}

    public static class IncludesArr
    {	
	    public readonly static string[] OTC_INCLUDES =
	    {
		    "LossIssueType"
	    };
	    
	    public readonly static string[] MATERIAL_C_INCLUDES =
	    {
		    "Material_CLangs.Language",
		    "MaterialType.C_Langs.Language",
		    "UnitType.C_Langs.Language"
	    };
	    
	    public readonly static string[] AREA_INCLUDES =
	    {
		    "Parent"
	    };
	    
	    public readonly static string[] WORKING_ORDER_INCLUDES =
	    {
		    "St_Design.C_Langs.Language",
		    "St_Design",
		    "EquipmentI",
		    "EquipmentC",
		    "MaterialC",
		    "ActivityState",
		    "Area"
	    };
	    
	    public readonly static string[] WORKING_ORDER_KPI =
	    {
		    "WorkingOrder"
	    };
	    
	    public readonly static string[] SCHEDULE_C_INCLUDES =
	    {
		    "St_Design"
	    };
	    
	    public readonly static string[] SCHEDULE_I_INCLUDES =
	    {
		    "Equipment_I",
		    "Schedule_C.St_Design",
		    "ActivityState"
	    };
	    
	    public readonly static string[] SHIFT_KPI_INCLUDES =
	    {
		    "ShiftI"
	    };
	    
	    public readonly static string[] SHIFT_C_INCLUDES =
	    {
		    "Schedule_C",
		    "St_Design",
		    "St_Shift"
	    };
	    
	    public readonly static string[] SHIFT_I_INCLUDES =
	    {
			"Schedule_I.Equipment_I",
			"Schedule_I.ActivityState",
			"Shift_C.St_Design",
			"Shift_C.St_Shift",
			"Shift_C.Schedule_C",
			"ActivityState"
	    };
	    
	    
	    public readonly static string[] LOSS_ISSUE_INCLUDES =
	    {
		    "LossReason",
		    "EquipmentI",
		    "LossIssueType",
		    "WorkingOrder",
		    "WorkingOrderKPI",
		    "ShiftI",
		    "ShiftKPI",
		    "Area"
	    };
	    
	    public readonly static string[] LOSS_REASON_RELATION_INCLUDES =
	    {
		    "LossReason",
		    "LossReasonTree"
	    };

	    public readonly static string[] LOSS_REASON_TREE_INCLUDES =
	    {
		    "St_Design.C_Langs.Language"
	    };

	    public readonly static string[] OPERATION_C_INCLUDES =
        {
			"Operation_CLangs.Language",
			"Type.C_Langs.Language",
			"St_Design.C_Langs.Language",
			"Family.C_Langs.Language",
			"Department.C_Langs.Language"
		};

        public readonly static string[] OPERATION_I_INCLUDES =
        {
			"St_Exe.C_Langs.Language",
			"St_ExeClose.C_Langs.Language",
			"StatusConformity.C_Langs.Language"
		};

		public readonly static string[] STEP_C_INCLUDES =
		{
			"Buttons_C.Button_CLangs.Language",
			"Buttons_C.ButtonConnections",
			"Step_CLangs.Language",
			"Type.C_Langs.Language",
			"StatusConformity.C_Langs.Language"
		};

        public readonly static string[] STEP_I_INCLUDES =
        {
			"St_Exe.C_Langs.Language",
			"St_ExeClose.C_Langs.Language",
			"StatusConformity.C_Langs.Language",
			"ButtonsNextAsStep_I.Button_C.Button_CLangs.Language",
			"ButtonsNextAsNextStep_I.Step_I"
			/*"Step_C.Operation_C",*/
            /*"User"*/
        };

        public readonly static string[] FORM_C_INCLUDES =
        {
			"Form_CLangs.Language",
			"Type.C_Langs.Language",
			"Family.C_Langs.Language",
			"St_Design.C_Langs.Language"
		};

        public readonly static string[] FORM_I_INCLUDES =
        {
			"St_Exe.C_Langs.Language",
			"StatusConformity.C_Langs.Language",
			"SubType.C_Langs.Language"
		};

		public readonly static string[] FIELD_C_INCLUDES =
        {
			"Field_CLangs.Language",
			"Type.C_Langs.Language",
			"Type.Format.C_Langs.Language",
			"St_Design.C_Langs.Language",
			"Options.StatusConformity",
			"Unit"
		};

		public readonly static string[] FIELD_I_INCLUDES =
		{
			"StatusConformity.C_Langs.Language",
			"Value.Field_CSelect_Option"
		};

		public readonly static string[] EQUIPMENT_C_INCLUDES =
        {
            "Type.C_Langs.Language",
            "St_Design.C_Langs.Language",
            "Equipment_CLangs.Language"
        };

		public readonly static string[] EQUIPMENT_I_INCLUDES =
		{
		};

        public readonly static string [] KPI_LOSSISSUES_INCLUDES = 
        {
            "EquipmentI"
        };

        public readonly static string [] KPI_INCLUDES = 
        {
            "Type.C_Langs.Language",
            "WorkingOrder",
            "ActivityState"
        };
        public readonly static string[] EQUIPMENT_KPI_INCLUDES =
        { 
        };
    }
}
