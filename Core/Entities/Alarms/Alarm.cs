using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms;

// This class is the link between the twincat and the ADS/ATS C#
// Thus why it is placed rather oddly compared to other models.
/*
	TYPE DB_OAK_Alm :
    STRUCT
	    Value : BOOL;
	    ID  : UDINT; // Indentifiant Alarme
        OneShot : BOOL; // Type d'alarme
        TimeStamp   : DT; // Timestamp alarme
	    Status:UDINT; // Status alarme (01=alarme up; 10=alarme down)
    END_STRUCT
    END_TYPE
 */

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct Alarm : IBaseADS<Alarm>
{
	/// <summary>
	///     Type must be 'blittable' to the corresponding PLC Struct Type
	///     See MSDN for MarshalAs and Default Marshalling.
	/// </summary>
	[MarshalAs(UnmanagedType.I1)]
	public bool Value;

	[MarshalAs(UnmanagedType.I1)]
	public bool OneShot;

	public TimestampStruct TimeStamp;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string RID;

	/// <summary>
	///     This is tricky but other notifications are supposed to be converted to something before being processed.
	///     This one does not but we pretend that we do something.
	/// </summary>
	public Alarm ToModel()
	{
		return this;
	}
}