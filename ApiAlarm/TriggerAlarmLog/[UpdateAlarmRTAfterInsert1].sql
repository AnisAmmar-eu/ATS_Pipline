USE [Alarms]
GO
/****** Object:  Trigger [dbo].[UpdateAlarmRTAfterInsert1]    Script Date: 11/09/2023 10:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateAlarmRTAfterInsert1]
    ON [dbo].[AlarmLog]
    AFTER insert
    AS
    BEGIN

        /*  a.NbNonAck = CASE WHEN (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID AND j.IsRead = 1)
                          = (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID) THEN 1 ELSE 0 END,  */


        DECLARE @NbNonAck INT;


        SELECT @NbNonAck = COUNT(*)
        FROM AlarmLog j
                 INNER JOIN INSERTED i ON j.AlarmID = i.AlarmID
        WHERE j.AlarmID = i.AlarmID
          AND j.IsAck = 0

        if (@NbNonAck > 1)
            begin
                UPDATE a
                SET a.NbNonAck  = @NbNonAck,
                    a.IsActive  = i.IsActive,
                    a.TS        = GETDATE(),
                    a.StationID = i.StationID,
                    a.TSRaised  = GETDATE(),
                    a.TSClear   = NULL
                FROM AlarmRT a
                         INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID;
            end;
        else
            begin

                INSERT INTO AlarmRT (AlarmID, IsActive, TS, StationID, NbNonAck, TSRaised)
                SELECT i.AlarmID, i.IsActive, GETDATE(), i.StationID, 1, GETDATE()
                FROM INSERTED i
            End;


    END;