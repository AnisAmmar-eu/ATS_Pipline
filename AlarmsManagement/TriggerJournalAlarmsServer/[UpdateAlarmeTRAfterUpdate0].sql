USE [AlarmsServer]
GO
/****** Object:  Trigger [dbo].[UpdateAlarmRTAfterUpdate0]    Script Date: 11/09/2023 10:23:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateAlarmRTAfterUpdate0]
    ON [dbo].[AlarmLog]
    AFTER update
    AS
    BEGIN


        DECLARE @NbNonAck INT;
        DECLARE @ExistRTLine INT;
        DECLARE @ActiveAlarm INT;

        SET @NbNonAck = 0
        SELECT @NbNonAck = COUNT(*)
        FROM AlarmLog currLog
                 INNER JOIN INSERTED newLog ON currLog.AlarmID = newLog.AlarmID
        WHERE currLog.AlarmID = newLog.AlarmID
          AND currLog.IsAck = 0

        SELECT @ActiveAlarm = COUNT(*)
        FROM AlarmLog currLog
                 INNER JOIN INSERTED newLog ON currLog.AlarmID = newLog.AlarmID
        where (currLog.AlarmID = newLog.AlarmID AND (currLog.IsActive = 1 OR currLog.IsAck = 0))


        SELECT @ExistRTLine = COUNT(*)
        FROM AlarmRT a
                 INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID
        WHERE a.AlarmID = i.AlarmID


        if (@NbNonAck > 0)
            begin
                UPDATE a
                SET a.NbNonAck = @NbNonAck,
                    a.IsActive = i.IsActive,
                    a.TS       = GETDATE(),
                    a.Station  = i.Station,
                    a.TSClear  = GETDATE()
                FROM AlarmRT a
                         INNER JOIN INSERTED i ON a.AlarmID = i.AlarmID;
            end;

        if (@ExistRTLine = 0)
            begin
                INSERT INTO AlarmRT (AlarmID, IsActive, TS, Station, NbNonAck, TSClear)
                SELECT i.AlarmID, 1, GETDATE(), i.Station, 1, GETDATE()
                FROM INSERTED i
            end


        else
            if (@ExistRTLine > 0)
                begin

                    if (@NbNonAck = 0)
                        begin
                            UPDATE a
                            SET a.NbNonAck = 0
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