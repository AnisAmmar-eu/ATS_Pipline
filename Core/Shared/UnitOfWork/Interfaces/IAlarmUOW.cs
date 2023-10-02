﻿using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Entities.Alarms.AlarmsCycle.Models.Repositories;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsPLC.Repositories;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Entities.Packets.Repositories;

namespace Core.Shared.UnitOfWork.Interfaces;

/// <summary>
///     Interface IUnitOfWork defines the methods that are required to be implemented by a Unit of Work class.
/// </summary>
public interface IAlarmUOW : IDisposable
{
	IAlarmCRepository AlarmC { get; }
	IAlarmPLCRepository AlarmPLC { get; }
	IAlarmLogRepository AlarmLog { get; }
	IAlarmRTRepository AlarmRT { get; }

	IPacketRepository Packet { get; }
	IAlarmCycleRepository AlarmCycle { get; }

	public object? GetRepoByType(Type repo);

	/// <summary>
	///     Saves changes made in this context to the underlying database.
	/// </summary>
	/// <returns></returns>
	int Commit();

	Task StartTransaction();

	Task CommitTransaction();
}