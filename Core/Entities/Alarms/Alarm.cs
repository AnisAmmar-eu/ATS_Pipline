using System.Runtime.InteropServices;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Microsoft.AspNetCore.Identity;

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
public struct Alarm : IBaseADS<AlarmPLC, Alarm>
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

    public Alarm(bool value)
    {
	    Value = true;
	    ID = 2005;
	    OneShot = false;
	    TimeStamp = 0;
	    TimeStampMS = 0;
	    Status = 2;
    }
    public AlarmPLC ToModel()
    {
	    return new AlarmPLC(this);
    }
}