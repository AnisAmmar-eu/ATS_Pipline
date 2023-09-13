USE [AlarmsServer]
GO
/****** Object:  Trigger [dbo].[UpdateAlarmRTAfterInsert1]    Script Date: 11/09/2023 10:23:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateAlarmRTAfterInsert1]
ON [dbo].[Journal]
AFTER insert
AS
BEGIN

   /*  a.NumberNonRead = CASE WHEN (SELECT COUNT(*) FROM Journal j WHERE j.IDAlarm = i.IDAlarm AND j.IsRead = 1)
                     = (SELECT COUNT(*) FROM Journal j WHERE j.IDAlarm = i.IDAlarm) THEN 1 ELSE 0 END,  */


    DECLARE @NumberNonRead INT;




	SELECT @NumberNonRead = COUNT(*) FROM Journal j  INNER JOIN INSERTED i ON j.IDAlarm = i.IDAlarm WHERE j.IDAlarm = i.IDAlarm AND j.IsRead = 0 

	if(@NumberNonRead > 1)
		begin
		UPDATE a
		SET  a.NumberNonRead = @NumberNonRead ,        
			a.status = CASE WHEN i.Status0 IS NULL THEN i.Status1 ELSE i.Status0 END,
			a.ts = GETDATE(),
			a.Station = i.Station
		FROM AlarmRT a
		INNER JOIN INSERTED i ON a.IDAlarm = i.IDAlarm;
		end;
	else 
		begin

		INSERT INTO AlarmRT (IDAlarm,Status, TS, Station, NumberNonRead)
			SELECT i.IDAlarm, CASE WHEN i.Status0 IS NULL THEN i.Status1 ELSE i.Status0 END, GETDATE(), i.Station, 1
			FROM INSERTED i
	End;



END;