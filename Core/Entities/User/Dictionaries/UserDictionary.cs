using Core.Entities.User.Models.DTO.Acts;

namespace Core.Entities.User.Dictionaries;

public class ActionRID
{
	public const string ADMIN_GENERAL_RIGHTS = "ADMIN.GENERAL_RIGHTS";
}

public class ActData
{
	public static DTOAct ADMIN_GENERAL_RIGHTS = new() { RID = ActionRID.ADMIN_GENERAL_RIGHTS };
}

public class ApplicationTypeRID
{
	public const string ROLE = "ROLE";
	public const string USER = "USER";
}

public class SignatureTypeRID
{
	public const string SESSION = "Session";
	public const string EXPLICIT = "Explicit";
	public const string DOUBLE = "Double";
}

public class ApplicationRoleType
{
	public const string SYSTEM_FIVES = "SYSTEM_FIVES";
	public const string SYSTEM_ATS = "SYSTEM_ATS";
	public const string USER = "USER";
}

public class RoleNames
{
	public const string FIVES = "Fives-Administrator";
	public const string ATS = "ATS-Administrator";
	public const string VISITOR = "Visitor";
	public const string OPERATOR = "Operator";
	public const string FORCING = "Forcing";
	public const string SETTINGS = "Settings";
	public const string ADMIN = "Admin";
}

public class SourceAuth
{
	public const string AD = "AD";
	public const string EKIDI = "EKIDI";

	public static string[] GetSources()
	{
		return new[]
		{
			//AD,
			EKIDI
		};
	}
}