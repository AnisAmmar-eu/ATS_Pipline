using System.Runtime.InteropServices;

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
public struct Alarm
{
    // Type must be 'blittable' to the corresponding PLC Struct Type
    // See MSDN for MarshalAs and Default Marshalling.
    [MarshalAs(UnmanagedType.I1)]
    public bool Value;
    public uint ID;
    [MarshalAs(UnmanagedType.I1)]
    public bool OneShot;

    public uint TimeStamp;
    public uint TimeStampMS;

    public uint Status;
}