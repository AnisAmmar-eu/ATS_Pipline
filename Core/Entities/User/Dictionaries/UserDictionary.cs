namespace Core.Entities.User.Dictionaries;

public static class ActionRID
{
	public const string AdminGeneralRights = "ADMIN.GENERAL_RIGHTS";
}

public static class ActData
{
	// public static DTOAct AdminGeneralRights = new() { RID = ActionRID.AdminGeneralRights };
}

public static class ApplicationTypeRID
{
	public const string Role = "ROLE";
	public const string User = "USER";
}

public static class SignatureTypeRID
{
	public const string Session = "Session";
	public const string Explicit = "Explicit";
	public const string Double = "Double";
}

public static class ApplicationRoleType
{
	public const string SystemFives = "SYSTEM_FIVES";
	public const string SystemATS = "SYSTEM_ATS";
	public const string User = "USER";
}

public static class RoleNames
{
	public const string Fives = "Fives-Administrator";
	public const string ATS = "ATS-Administrator";
	public const string Visitor = "Visitor";
	public const string Operator = "Operator";
	public const string Forcing = "Forcing";
	public const string Settings = "Settings";
	public const string Admin = "Admin";
}

public static class SourceAuth
{
	public const string AD = "AD";
	public const string Ekidi = "EKIDI";

	public static string[] GetSources()
	{
		return [/*AD,*/ Ekidi,];
	}
}