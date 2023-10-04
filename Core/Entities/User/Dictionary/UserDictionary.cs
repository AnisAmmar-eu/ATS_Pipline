using Core.Entities.User.Models.DTO.Acts;

namespace Core.Entities.User.Dictionary;

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
	public const string SYSTEM_EKIUM = "SYSTEM_EKIUM";
	public const string SYSTEM_EKIDI = "SYSTEM_EKIDI";
	public const string USER = "USER";
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