USE [Alarms]
GO
/****** Object:  Trigger [dbo].[UpdateAlarmRTAfterUpdate0]    Script Date: 11/09/2023 10:24:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[UpdateAlarmRTAfterUpdate0]
    ON [dbo].[AlarmLog]
    AFTER update
    AS
BEGIN

    /*  a.NumberNonRead = CASE WHEN (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID AND j.IsRead = 1)
                      = (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID) THEN 1 ELSE 0 END,  */


    DECLARE @NumberNonRead INT;
    DECLARE @ExistRTLine INT;
    DECLARE @ActiveAlarm INT;

    SET @NumberNonRead = 0
    SELECT @NumberNonRead = COUNT(*)
    FROM AlarmLog log
             INNER JOIN INSERTED newLog ON log.AlarmID = newLog.AlarmID
    WHERE log.AlarmID = newLog.AlarmID
      AND log.IsAck = 0

    SELECT @ActiveAlarm = COUNT(*)
    FROM AlarmLog log
             INNER JOIN INSERTED newLog ON log.AlarmID = newLog.AlarmID
    where (log.AlarmID = newLog.AlarmID AND (log.IsActive = 1 OR log.IsAck = 0))


    SELECT @ExistRTLine = COUNT(*)
    FROM AlarmRT a
             INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID
    WHERE a.AlarmID = i.AlarmID


    if (@NumberNonRead > 0)
        begin
            UPDATE a
            SET a.NumberNonRead = @NumberNonRead,
                a.IsActive      = i.IsActive,
                a.TS            = GETDATE(),
                a.Station       = i.Station
            FROM AlarmRT a
                     INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID;
        end;

    if (@ExistRTLine = 0)
        begin
            INSERT INTO AlarmRT (AlarmID, IsActive, TS, Station, NumberNonRead)
            SELECT i.AlarmID, 1, GETDATE(), i.Station, 1
            FROM INSERTED i
        end


    else
        if (@ExistRTLine > 0)
            begin

                if (@NumberNonRead = 0)
                    begin
                        UPDATE a
                        SET a.NumberNonRead = 0
                        FROM AlarmRT a
                                 INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID;
                    end;

                if (@ActiveAlarm = 0)
                    begin
                        delete a
                        from AlarmRT a
                                 INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID;
                    end
            End

END;