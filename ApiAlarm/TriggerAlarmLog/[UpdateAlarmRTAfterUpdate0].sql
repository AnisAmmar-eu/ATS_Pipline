/*
	This trigger is executed after an update operation on the [dbo].[AlarmLog] table.
	It performs the following actions:
	1. Calculates the number of non-acknowledged alarms for the updated alarm.
	2. Checks if there is an existing record for the updated alarm in the [AlarmRT] table.
	3. Updates the [AlarmRT] table if there are non-acknowledged alarms.
	4. Inserts a new record in the [AlarmRT] table if there is no existing record.
	5. Updates or deletes the existing record in the [AlarmRT] table based on the number of non-acknowledged alarms and active alarms.

	Trigger Logic:
	- Calculate the number of non-acknowledged alarms for the updated alarm.
	- Check if there is an existing record for the updated alarm in the [AlarmRT] table.
	- If there are non-acknowledged alarms:
		- Update the existing record in the [AlarmRT] table with the new values.
	- If there is no existing record:
		- Insert a new record in the [AlarmRT] table with the new values.
	- If there is an existing record:
		- If there are no non-acknowledged alarms:
			- Update the existing record in the [AlarmRT] table with the new values.
		- If there are no active alarms:
			- Delete the existing record from the [AlarmRT] table.
*/
USE [Alarms]
GO
/****** Object:  Trigger [dbo].[UpdateAlarmRTAfterUpdate0]    Script Date: 11/09/2023 10:24:30 ******/
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
		FROM AlarmRT currAlarmRT
				INNER JOIN INSERTED newAlarmRT ON currAlarmRT.AlarmID = newAlarmRT.AlarmID
		WHERE currAlarmRT.AlarmID = newAlarmRT.AlarmID

		if (@NbNonAck > 0)
			begin
				UPDATE alarmRT
				SET alarmRT.NbNonAck  = @NbNonAck,
					alarmRT.IsActive  = i.IsActive,
					alarmRT.TS        = GETDATE(),
					alarmRT.StationID = i.StationID,
					alarmRT.TSClear   = GETDATE()
				FROM AlarmRT alarmRT
						INNER JOIN INSERTED i ON alarmRT.AlarmID = i.AlarmID;
			end;

		if (@ExistRTLine = 0)
			begin
				INSERT INTO AlarmRT (AlarmID, IsActive, TS, StationID, NbNonAck, TSClear)
				SELECT i.AlarmID, 1, GETDATE(), i.StationID, 1, GETDATE()
				FROM INSERTED i
			end


		else
			begin
				if (@ExistRTLine > 0)
					begin

						if (@NbNonAck = 0)
							begin
								UPDATE alarmRT
								SET alarmRT.NbNonAck = 0,
									alarmRT.IsActive = CASE WHEN @ActiveAlarm > 0 THEN 1 ELSE 0 END
								FROM AlarmRT alarmRT
										INNER JOIN INSERTED i ON alarmRT.AlarmID = i.AlarmID;
							end;

						if (@ActiveAlarm = 0)
							begin
								delete alarmRT
								from AlarmRT alarmRT
										INNER JOIN INSERTED i ON alarmRT.AlarmID = i.AlarmID;
							end
					End
			End
	END;