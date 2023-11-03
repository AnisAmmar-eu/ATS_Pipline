USE [AlarmsServer]
GO
/****** Object:  Trigger [dbo].[UpdateAlarmRTAfterInsert1]    Script Date: 11/09/2023 10:23:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateAlarmRTAfterInsert1]
    ON [dbo].[AlarmLog]
    AFTER insert
    AS
    BEGIN

        /*  a.NbNonAck = CASE WHEN (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID AND j.IsAck = 1)
                          = (SELECT COUNT(*) FROM AlarmLog j WHERE j.AlarmID = i.AlarmID) THEN 1 ELSE 0 END,  */


        DECLARE @NbNonAck INT;
        DECLARE @IsAlarmActive INT;
        DECLARE @MostRecentClear DATETIMEOFFSET;
        DECLARE @MostRecentRaise DATETIMEOFFSET;


        SELECT @NbNonAck = COUNT(*)
        FROM AlarmLog j
                 INNER JOIN INSERTED i ON j.AlarmID = i.AlarmID
        WHERE j.AlarmID = i.AlarmID
          AND j.IsAck = 0

        SELECT @IsAlarmActive = COUNT(*)
        FROM AlarmLog currLog
                 INNER JOIN INSERTED newLog ON currLog.AlarmID = newLog.AlarmID
        WHERE currLog.AlarmID = newLog.AlarmID
          AND currLog.IsActive = 1
          AND (currLog.IsActive = 1 OR currLog.IsAck = 0)

        SELECT @MostRecentClear = MAX(log1.TSClear)
        FROM AlarmLog log1
                 INNER JOIN INSERTED log2 ON log1.AlarmID = log2.AlarmID
        WHERE log1.AlarmID = log2.AlarmID

        SELECT @MostRecentRaise = MAX(log1.TSRaised)
        FROM AlarmLog log1
                 INNER JOIN INSERTED log2 ON log1.AlarmID = log2.AlarmID
        WHERE log1.AlarmID = log2.AlarmID

        if (@NbNonAck > 1)
            begin
                if (@IsAlarmActive >= 1)
                    begin
                        UPDATE a
                        SET a.NbNonAck  = @NbNonAck,
                            a.IsActive  = 1,
                            a.TS        = GETDATE(),
                            a.StationID = i.StationID,
                            a.TSRaised  = @MostRecentRaise,
                            a.TSClear   = NULL
                        FROM AlarmRT a
                                 INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID AND i.IsActive = 1
                    end
                else
                    begin
                        UPDATE a
                        SET a.NbNonAck  = @NbNonAck,
                            a.IsActive  = 0,
                            a.TS        = GETDATE(),
                            a.StationID = i.StationID,
                            a.TSRaised  = i.TSRaised,
                            a.TSClear   = i.TSClear
                        FROM AlarmRT a
                                 INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID AND @MostRecentClear = i.TSClear
                    end
            end
        else
            if (@IsAlarmActive >= 1)
                begin
                    INSERT INTO AlarmRT (AlarmID, IsActive, TS, StationID, NbNonAck, TSRaised)
                    SELECT i.AlarmID, 1, GETDATE(), i.StationID, 1, i.TSRaised
                    FROM INSERTED i
                end
            else
                if (@NbNonAck = 1)
                    begin
                        INSERT INTO AlarmRT (AlarmID, IsActive, TS, StationID, NbNonAck, TSRaised, TSClear)
                        SELECT i.AlarmID, 0, GETDATE(), i.StationID, 1, i.TSRaised, i.TSClear
                        FROM INSERTED i
                    end
    END;