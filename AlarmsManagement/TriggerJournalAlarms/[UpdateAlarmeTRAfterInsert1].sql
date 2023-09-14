USE [Alarms]
GO
/****** Object:  Trigger [dbo].[UpdateAlarmRTAfterInsert1]    Script Date: 11/09/2023 10:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[UpdateAlarmRTAfterInsert1]
    ON [dbo].[AlarmLog]
    AFTER insert
    AS
BEGIN

    /*  a.NumberNonRead = CASE WHEN (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID AND j.IsRead = 1)
                      = (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID) THEN 1 ELSE 0 END,  */


    DECLARE @NumberNonRead INT;


    SELECT @NumberNonRead = COUNT(*)
    FROM AlarmLog j
             INNER JOIN INSERTED i ON j.AlarmID = i.AlarmID
    WHERE j.AlarmID = i.AlarmID
      AND j.IsAck = 0

    if (@NumberNonRead > 1)
        begin
            UPDATE a
            SET a.NumberNonRead = @NumberNonRead,
                a.IsActive      = i.IsActive,
                a.TS            = GETDATE(),
                a.Station       = i.Station
            FROM AlarmRT a
                     INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID;
        end;
    else
        begin

            INSERT INTO AlarmRT (AlarmID, IsActive, TS, Station, NumberNonRead)
            SELECT i.AlarmID, 1, GETDATE(), i.Station, 1
            FROM INSERTED i
        End;


END;