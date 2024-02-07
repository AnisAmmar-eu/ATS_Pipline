/*

	This trigger is executed after an insertion into the [dbo].[AlarmLog] table.
	It updates the [AlarmRT] table based on the inserted data.

	If there are more than one non-acknowledged alarms with the same AlarmID, it updates the existing record in [AlarmRT].
	Otherwise, it inserts a new record into [AlarmRT].

	The trigger performs the following actions:
	- Counts the number of non-acknowledged alarms with the same AlarmID.
	- If there are more than one non-acknowledged alarms, it updates the existing record in [AlarmRT].
	- If there is only one non-acknowledged alarm, it inserts a new record into [AlarmRT].

	The columns updated/inserted in [AlarmRT] are:
	- NbNonAck: Number of non-acknowledged alarms with the same AlarmID.
	- IsActive: Active status of the alarm.
	- TS: Timestamp of the update/insert operation.
	- StationID: ID of the station associated with the alarm.
	- TSRaised: Timestamp when the alarm was raised.
	- TSClear: Timestamp when the alarm was cleared (NULL for non-cleared alarms).

	Note: This trigger assumes that the [AlarmRT] table already exists.
*/
USE [Alarms]
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateAlarmRTAfterInsert1]
	ON [dbo].[AlarmLog]
	AFTER insert
	AS
	BEGIN

		DECLARE @NbNonAck INT;

		-- This query selects the count of non-acknowledged alarms related to the inserted alarm record.
		-- It joins the AlarmLog table with the INSERTED table on the AlarmID column to find matching records.
		-- The WHERE clause filters the records based on the AlarmID and IsAck columns.
		-- The result is stored in the variable @NbNonAck.
		SELECT @NbNonAck = COUNT(*)
		FROM AlarmLog currLog
			INNER JOIN INSERTED newLog ON currLog.AlarmID = newLog.AlarmID
		WHERE currLog.AlarmID = newLog.AlarmID
			AND currLog.IsAck = 0

		/*
			This code block updates the AlarmRT table if the @NbNonAck parameter is greater than 1.
			It updates the NbNonAck, IsActive, TS, StationID, TSRaised, and TSClear columns of the AlarmRT table
			based on the corresponding values in the INSERTED table.

			If the @NbNonAck parameter is not greater than 1, it inserts a new record into the AlarmRT table.
			The values for the AlarmID, IsActive, TS, StationID, NbNonAck, and TSRaised columns are selected
			from the INSERTED table.
		*/
		if (@NbNonAck > 1)
			begin
				UPDATE alarmRT
				SET alarmRT.NbNonAck	= @NbNonAck,
					alarmRT.IsActive	= alarmLog.IsActive,
					alarmRT.TS			= GETDATE(),
					alarmRT.StationID	= alarmLog.StationID,
					alarmRT.TSRaised	= alarmLog.TSRaised,
					alarmRT.TSClear		= NULL
				FROM AlarmRT alarmRT
						INNER JOIN INSERTED alarmLog ON alarmRT.AlarmID = alarmLog.AlarmID;
			end;
		else
			begin

				INSERT INTO AlarmRT (AlarmID, IsActive, TS, StationID, NbNonAck, TSRaised)
				SELECT alarmLog.AlarmID, alarmLog.IsActive, GETDATE(), alarmLog.StationID, 1, alarmLog.TSRaised
				FROM INSERTED AlarmLog
			End;


	END;