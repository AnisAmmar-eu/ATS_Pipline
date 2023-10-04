namespace Core.Entities.User.Models.DTO.Auth.Login;

public partial class DTOLoginResponse
{
	public DTOLoginResponse()
	{
	}

	public DTOLoginResponse(string token)
	{
		Token = token;
	}
}