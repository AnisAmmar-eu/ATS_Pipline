namespace Core.Entities.Vision.FileSettings.Dictionaries;

public static class FileSettingRID
{
	/*
		Station (S1, S2, S3, S4, S5)
		Sign (All) / Match (S3, S4, S5)
		Cam1 (All), Cam2 (Except S5)
		DX (All), D20 (Except S5)
		Static / Dynamic (Except Match)
	*/

	// Station S1 with Sign
	public const string S1SignCam1DXStatic = "S1SignCam1DXStatic";
	public const string S1SignCam1D20Static = "S1SignCam1D20Static";
	public const string S1SignCam1DXDynamic = "S1SignCam1DXDynamic";
	public const string S1SignCam1D20Dynamic = "S1SignCam1D20Dynamic";
	public const string S1SignCam2DXStatic = "S1SignCam2DXStatic";
	public const string S1SignCam2D20Static = "S1SignCam2D20Static";
	public const string S1SignCam2DXDynamic = "S1SignCam2DXDynamic";
	public const string S1SignCam2D20Dynamic = "S1SignCam2D20Dynamic";

	// Station S2 with Sign
	public const string S2SignCam1DXStatic = "S2SignCam1DXStatic";
	public const string S2SignCam1D20Static = "S2SignCam1D20Static";
	public const string S2SignCam1DXDynamic = "S2SignCam1DXDynamic";
	public const string S2SignCam1D20Dynamic = "S2SignCam1D20Dynamic";
	public const string S2SignCam2DXStatic = "S2SignCam2DXStatic";
	public const string S2SignCam2D20Static = "S2SignCam2D20Static";
	public const string S2SignCam2DXDynamic = "S2SignCam2DXDynamic";
	public const string S2SignCam2D20Dynamic = "S2SignCam2D20Dynamic";

	 // Station S3 with Sign
	public const string S3SignCam1DXStatic = "S3SignCam1DXStatic";
	public const string S3SignCam1D20Static = "S3SignCam1D20Static";
	public const string S3SignCam1DXDynamic = "S3SignCam1DXDynamic";
	public const string S3SignCam1D20Dynamic = "S3SignCam1D20Dynamic";
	public const string S3SignCam2DXStatic = "S3SignCam2DXStatic";
	public const string S3SignCam2D20Static = "S3SignCam2D20Static";
	public const string S3SignCam2DXDynamic = "S3SignCam2DXDynamic";
	public const string S3SignCam2D20Dynamic = "S3SignCam2D20Dynamic";

	// Station S3 with Match
	public const string S3MatchCam1DXDynamic = "S3MatchCam1DXDynamic";
	public const string S3MatchCam1D20Dynamic = "S3MatchCam1D20Dynamic";
	public const string S3MatchCam2DXDynamic = "S3MatchCam2DXDynamic";
	public const string S3MatchCam2D20Dynamic = "S3MatchCam2D20Dynamic";

	 // Station S4 with Sign
	public const string S4SignCam1DXStatic = "S4SignCam1DXStatic";
	public const string S4SignCam1D20Static = "S4SignCam1D20Static";
	public const string S4SignCam1DXDynamic = "S4SignCam1DXDynamic";
	public const string S4SignCam1D20Dynamic = "S4SignCam1D20Dynamic";
	public const string S4SignCam2DXStatic = "S4SignCam2DXStatic";
	public const string S4SignCam2D20Static = "S4SignCam2D20Static";
	public const string S4SignCam2DXDynamic = "S4SignCam2DXDynamic";
	public const string S4SignCam2D20Dynamic = "S4SignCam2D20Dynamic";

	// Station S4 with Match
	public const string S4MatchCam1DXDynamic = "S4MatchCam1DXDynamic";
	public const string S4MatchCam1D20Dynamic = "S4MatchCam1D20Dynamic";
	public const string S4MatchCam2DXDynamic = "S4MatchCam2DXDynamic";
	public const string S4MatchCam2D20Dynamic = "S4MatchCam2D20Dynamic";

	 // Station S5 with Sign
	public const string S5SignCam1DXStatic = "S5SignCam1DXStatic";
	public const string S5SignCam1DXDynamic = "S5SignCam1DXDynamic";

	// Station S5 with Match
	public const string S5MatchCam1DXDynamic = "S5MatchCam1DXDynamic";

	public static List<string> fileSettingRIDs
		= [
			// Station S1 with Sign
			S1SignCam1DXStatic,
			S1SignCam1D20Static,
			S1SignCam1DXDynamic,
			S1SignCam1D20Dynamic,
			S1SignCam2DXStatic,
			S1SignCam2D20Static,
			S1SignCam2DXDynamic,
			S1SignCam2D20Dynamic,

			// Station S2 with Sign
			S2SignCam1DXStatic,
			S2SignCam1D20Static,
			S2SignCam1DXDynamic,
			S2SignCam1D20Dynamic,
			S2SignCam2DXStatic,
			S2SignCam2D20Static,
			S2SignCam2DXDynamic,
			S2SignCam2D20Dynamic,

			// Station S3 with Sign
			S3SignCam1DXStatic,
			S3SignCam1D20Static,
			S3SignCam1DXDynamic,
			S3SignCam1D20Dynamic,
			S3SignCam2DXStatic,
			S3SignCam2D20Static,
			S3SignCam2DXDynamic,
			S3SignCam2D20Dynamic,

			// Station S3 with Match
			S3MatchCam1DXDynamic,
			S3MatchCam1D20Dynamic,
			S3MatchCam2DXDynamic,
			S3MatchCam2D20Dynamic,

			// Station S4 with Sign
			S4SignCam1DXStatic,
			S4SignCam1D20Static,
			S4SignCam1DXDynamic,
			S4SignCam1D20Dynamic,
			S4SignCam2DXStatic,
			S4SignCam2D20Static,
			S4SignCam2DXDynamic,
			S4SignCam2D20Dynamic,

			// Station S4 with Match
			S4MatchCam1DXDynamic,
			S4MatchCam1D20Dynamic,
			S4MatchCam2DXDynamic,
			S4MatchCam2D20Dynamic,

			// Station S5 with Sign
			S5SignCam1DXStatic,
			S5SignCam1DXDynamic,

			// Station S5 with Match
			S5MatchCam1DXDynamic,
	];
}

public static class FileSettingPath
{
	// Station S1 with Sign
	public const string S1SignCam1DXStatic = @"C:\atsVision\S1SignCam1DXStatic.txt";
	public const string S1SignCam1D20Static = @"C:\atsVision\S1SignCam1D20Static.txt";

	public const string S1SignCam1DXDynamic = @"C:\atsVision\S1SignCam1DXDynamic.txt";
	public const string S1SignCam1D20Dynamic = @"C:\atsVision\S1SignCam1D20Dynamic.txt";
	public const string S1SignCam2DXStatic = @"C:\atsVision\S1SignCam2DXStatic.txt";
	public const string S1SignCam2D20Static = @"C:\atsVision\S1SignCam2D20Static.txt";
	public const string S1SignCam2DXDynamic = @"C:\atsVision\S1SignCam2DXDynamic.txt";
	public const string S1SignCam2D20Dynamic = @"C:\atsVision\S1SignCam2D20Dynamic.txt";

	// Station S2 with Sign
	public const string S2SignCam1DXStatic = @"C:\atsVision\S2SignCam1DXStatic.txt";
	public const string S2SignCam1D20Static = @"C:\atsVision\S2SignCam1D20Static.txt";
	public const string S2SignCam1DXDynamic = @"C:\atsVision\S2SignCam1DXDynamic.txt";
	public const string S2SignCam1D20Dynamic = @"C:\atsVision\S2SignCam1D20Dynamic.txt";
	public const string S2SignCam2DXStatic = @"C:\atsVision\S2SignCam2DXStatic.txt";
	public const string S2SignCam2D20Static = @"C:\atsVision\S2SignCam2D20Static.txt";
	public const string S2SignCam2DXDynamic = @"C:\atsVision\S2SignCam2DXDynamic.txt";
	public const string S2SignCam2D20Dynamic = @"C:\atsVision\S2SignCam2D20Dynamic.txt";

	// Station S3 with Sign
	public const string S3SignCam1DXStatic = @"C:\atsVision\S3SignCam1DXStatic.txt";
	public const string S3SignCam1D20Static = @"C:\atsVision\S3SignCam1D20Static.txt";
	public const string S3SignCam1DXDynamic = @"C:\atsVision\S3SignCam1DXDynamic.txt";
	public const string S3SignCam1D20Dynamic = @"C:\atsVision\S3SignCam1D20Dynamic.txt";
	public const string S3SignCam2DXStatic = @"C:\atsVision\S3SignCam2DXStatic.txt";
	public const string S3SignCam2D20Static = @"C:\atsVision\S3SignCam2D20Static.txt";
	public const string S3SignCam2DXDynamic = @"C:\atsVision\S3SignCam2DXDynamic.txt";
	public const string S3SignCam2D20Dynamic = @"C:\atsVision\S3SignCam2D20Dynamic.txt";

	// Station S3 with Match
	public const string S3MatchCam1DXDynamic = @"C:\atsVision\S3MatchCam1DXDynamic.txt";
	public const string S3MatchCam1D20Dynamic = @"C:\atsVision\S3MatchCam1D20Dynamic.txt";
	public const string S3MatchCam2DXDynamic = @"C:\atsVision\S3MatchCam2DXDynamic.txt";
	public const string S3MatchCam2D20Dynamic = @"C:\atsVision\S3MatchCam2D20Dynamic.txt";

	// Station S4 with Sign
	public const string S4SignCam1DXStatic = @"C:\atsVision\S4SignCam1DXStatic.txt";
	public const string S4SignCam1D20Static = @"C:\atsVision\S4SignCam1D20Static.txt";
	public const string S4SignCam1DXDynamic = @"C:\atsVision\S4SignCam1DXDynamic.txt";
	public const string S4SignCam1D20Dynamic = @"C:\atsVision\S4SignCam1D20Dynamic.txt";
	public const string S4SignCam2DXStatic = @"C:\atsVision\S4SignCam2DXStatic.txt";
	public const string S4SignCam2D20Static = @"C:\atsVision\S4SignCam2D20Static.txt";
	public const string S4SignCam2DXDynamic = @"C:\atsVision\S4SignCam2DXDynamic.txt";
	public const string S4SignCam2D20Dynamic = @"C:\atsVision\S4SignCam2D20Dynamic.txt";

	// Station S4 with Match
	public const string S4MatchCam1DXDynamic = @"C:\atsVision\S4MatchCam1DXDynamic.txt";
	public const string S4MatchCam1D20Dynamic = @"C:\atsVision\S4MatchCam1D20Dynamic.txt";
	public const string S4MatchCam2DXDynamic = @"C:\atsVision\S4MatchCam2DXDynamic.txt";
	public const string S4MatchCam2D20Dynamic = @"C:\atsVision\S4MatchCam2D20Dynamic.txt";

	// Station S5 with Sign
	public const string S5SignCam1DXStatic = @"C:\atsVision\S5SignCam1DXStatic.txt";
	public const string S5SignCam1DXDynamic = @"C:\atsVision\S5SignCam1DXDynamic.txt";

	// Station S5 with Match
	public const string S5MatchCam1DXDynamic = @"C:\atsVision\S5MatchCam1DXDynamic.txt";
}